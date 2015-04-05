using System.Data.EntityClient;
using System.Data.SqlServerCe;
using System.IO;
using BizInfo.Harvesting.Services.Storage;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace BizInfo.Harvesting.Services.Tests.Scouting
{
    internal class TestEntityStorage : EntityStorage
    {
        private TestEntityStorage(EntityConnection connection)
            : base(connection)
        {
        }

        public override int GetCategory(string name, int? parentCategory)
        {
            lock (this)
            {
                var categoryKey = parentCategory.HasValue ? string.Format("{0}^{1}", name, parentCategory) : name;
                int categoryId;
                if (cachedCategories.TryGetValue(categoryKey, out categoryId)) return categoryId;
                categoryId = cachedCategories.Count;
                cachedCategories[categoryKey] = categoryId;
                return categoryId;
            }
        }

        private static void InitializeEnterpriseLibrary()
        {
            if (EnterpriseLibraryContainer.Current == null) return;

            var builder = new ConfigurationSourceBuilder();

            builder.ConfigureLogging()
                .WithOptions
                .DoNotRevertImpersonation()
                .LogToCategoryNamed("Security")
                .SendTo.FlatFile("Security Log File")
                .FormatWith(new FormatterBuilder()
                                .TextFormatterNamed("Text Formatter")
                                .UsingTemplate("Timestamp: {timestamp}...{newline})}"))
                .ToFile("security.log")
                .SendTo.EventLog("Formatted EventLog TraceListener")
                .FormatWithSharedFormatter("Text Formatter")
                .ToLog("Application")
                .LogToCategoryNamed("General")
                .WithOptions.SetAsDefaultCategory()
                .SendTo.SharedListenerNamed("Formatted EventLog TraceListener");

            var configSource = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(configSource);
            EnterpriseLibraryContainer.Current = EnterpriseLibraryContainer.CreateDefaultContainer(configSource);
        }

        public static TestEntityStorage Create(string databaseFilename = @"c:\BizInfoTest.sdf")
        {
            InitializeEnterpriseLibrary();
            var entityBuilder = new EntityConnectionStringBuilder();

            entityBuilder.Provider = "System.Data.SqlServerCe.4.0";
            if (File.Exists(databaseFilename))
            {
                File.Delete(databaseFilename);
            }
            entityBuilder.ProviderConnectionString = string.Format("data source=\"{0}\"", databaseFilename);
            entityBuilder.Metadata = "res://*/Entities.BizInfo.csdl|res://*/Entities.BizInfo.ssdl|res://*/Entities.BizInfo.msl";
            var en = new SqlCeEngine(entityBuilder.ProviderConnectionString);
            en.CreateDatabase();

            var connection = new EntityConnection(entityBuilder.ToString());
            return new TestEntityStorage(connection);
        }
    }
}