using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Perenis.Core.Reflection;

namespace BizInfo.Harvesting.Services.Processing.Fragments
{
    public class DbMaintenanceTools
    {
        /// <summary>
        /// Timeout for long operations in seconds
        /// </summary>
        public static int timeout = 4800;

        /// <summary>
        /// Recomputes all occurencies
        /// </summary>
        public static void RecomputeOccurencies()
        {
            var sqlCommandString = Assembly.GetExecutingAssembly().GetManifestResourceString("BizInfo.Harvesting.Services.Scripts.occurency_computing.sql", Encoding.ASCII);
            ExecuteSqlCommand(sqlCommandString);
        }

        /// <summary>
        /// Refreshes indexes
        /// </summary>
        public static void RefreshIndexes()
        {
            var sqlCommandString = Assembly.GetExecutingAssembly().GetManifestResourceString("BizInfo.Harvesting.Services.Scripts.refresh_indexes.sql", Encoding.ASCII);
            ExecuteSqlCommand(sqlCommandString);
        }

        private static void ExecuteSqlCommand(string sqlCommandString)
        {
            using (var connection = new SqlConnection(string.Format(@"Data Source=.\BIZINFO_EX;Initial Catalog=BizInfo;Integrated Security=True;Connection Timeout={0};MultipleActiveResultSets=True", timeout)))
            {
                connection.Open();
                using (var command = new SqlCommand(sqlCommandString, connection))
                {
                    command.CommandTimeout = timeout;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}