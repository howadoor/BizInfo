using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Storage;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.SpecialServices
{
    /// <summary>
    /// Extracts mails from stored infos
    /// </summary>
    public class MailExtractor : IBizInfoProcessor, IDisposable
    {
        /// <summary>
        /// Internal collection of mails
        /// </summary>
        private Dictionary<string, HashSet<string>> mails = new Dictionary<string, HashSet<string>>();

        #region IBizInfoProcessor Members

        public void Process(IBizInfo info, IBizInfoStorage storage)
        {
            CollectMails(((EntityStorage) storage).modelContainer.TagSet.Where(tg => tg.Id == info.SourceTagId).Single(), info.StructuredContentDocument.Mails);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes all internal structures
        /// </summary>
        public void Dispose()
        {
            mails = null;
        }

        #endregion

        /// <summary>
        /// Adds all <see cref="mailsToAdd"/> from <see cref="sourceTag"/> to the internal structures
        /// </summary>
        /// <param name="sourceTag"></param>
        /// <param name="mailsToAdd">Mails to be added</param>
        private void CollectMails(Tag sourceTag, IEnumerable<string> mailsToAdd)
        {
            var mailsList = GetMailsList(sourceTag);
            foreach (var mailToAdd in mailsToAdd) mailsList.Add(mailToAdd);
        }

        /// <summary>
        /// Returns list of mails related to <see cref="sourceTag"/>. If no such list exists, creates a new one.
        /// </summary>
        /// <param name="sourceTag"></param>
        /// <returns></returns>
        private HashSet<string> GetMailsList(Tag sourceTag)
        {
            lock (this)
            {
                HashSet<string> mailsList;
                if (!mails.TryGetValue(sourceTag.Name, out mailsList))
                {
                    mails[sourceTag.Name] = mailsList = new HashSet<string>();
                }
                return mailsList;
            }
        }

        /// <summary>
        /// Saves all collected mails to lists in <see cref="targetFolder"/>
        /// </summary>
        /// <param name="targetFolder">Target folder where lists should be placed</param>
        /// <param name="filenameCreation">Function creating file name from source tag name. If <c>null</c>, <see cref="CreateListFilename"/> is used as a default</param>
        public void SaveLists(string targetFolder, Func<string, string> filenameCreation = null)
        {
            if (filenameCreation == null) filenameCreation = CreateListFilename;
            foreach (var mailList in mails)
            {
                // get sorted list of mails
                var currentMailList = mailList.Value.ToList();
                currentMailList.Sort();
                // write to target file
                var targetFilename = Path.Combine(targetFolder, filenameCreation(mailList.Key));
                using (var targetFile = new StreamWriter(targetFilename))
                {
                    foreach (var mail in currentMailList)
                    {
                        targetFile.WriteLine(mail);
                    }
                }
            }
        }

        /// <summary>
        /// Function used to create filename of mail list file in <see cref="SaveLists"/> method;
        /// </summary>
        /// <param name="sourceTagName"></param>
        /// <returns></returns>
        public static string CreateListFilename(string sourceTagName)
        {
            return string.Format("{0}.addresses.txt", sourceTagName);
        }

        /// <summary>
        /// Clears all internal structures
        /// </summary>
        public void Clear()
        {
            mails.Clear();
        }
    }
}