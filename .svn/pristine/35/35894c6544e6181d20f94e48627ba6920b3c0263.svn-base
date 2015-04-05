using System;
using System.Collections.Generic;
using System.Linq;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Services.Core;
using BizInfo.Model.Entities;
using BizInfo.Model.Entities.Extensions;

namespace BizInfo.Harvesting.Services.Messaging
{
    /// <summary>
    /// Periodicaly search the database for new items then sends it to the users by mail
    /// </summary>
    public class NewItemsSender
    {
        private int maxItemsInOneMail = 100;
        private Daemon newItemsSenderDaemon;
        private DaemonStatusLogger newItemsSenderDaemonLogger;

        /// <summary>
        /// Creates new instance of <see cref="NewItemsSender"/> and starts deamon which periodicaly looks for new items in db and sendes it to the users
        /// </summary>
        public NewItemsSender()
        {
            Start();
        }

        /// <summary>
        /// Starts deamon which periodicaly looks for new items in db and sendes it to the users
        /// </summary>
        private void Start()
        {
            newItemsSenderDaemon = new Daemon {Action = FindNewItemsAndSend, Period = TimeSpan.FromMinutes(5)};
            newItemsSenderDaemonLogger = new DaemonStatusLogger {DaemonName = "Mailing new informations to tenant"};
            newItemsSenderDaemonLogger.Attach(newItemsSenderDaemon);
            newItemsSenderDaemon.Start();
        }

        /// <summary>
        /// Enumerates all users in db, looks for new items then sends it to the tenant
        /// </summary>
        private void FindNewItemsAndSend()
        {
            using (var repository = new BizInfoModelContainer {CommandTimeout = 120})
            {
                var tenants = repository.TenantSet.ToArray();
                foreach (var tenant in tenants)
                {
                    if (ShouldBeProcessed(tenant))
                    {
                        var countingSender = new CountingSender();
                        try
                        {
                            FindNewItemsAndSend(tenant, repository, countingSender);
                        }
                        finally 
                        {
                            if (countingSender.CountOfInfoSent > 0)
                            {
                                var sentMessage = repository.SentMessagesSet.CreateObject();
                                sentMessage.SendTime = DateTime.Now;
                                sentMessage.ContainedInfoCount = countingSender.CountOfInfoSent;
                                //sentMessage.UserId = tenant.Id;
                                sentMessage.Mail = tenant.Mail;
                                repository.SentMessagesSet.AddObject(sentMessage);
                                repository.SaveChanges();
                            }
                            repository.SaveChanges();
                        }
                    }
                }
            }
        }

        private void FindNewItemsAndSend(Tenant tenant, BizInfoModelContainer repository, IMessageSender sender)
        {
            try
            {
                using (this.LogOperation(string.Format("Finding new items to be sent to {0}", tenant.Id)))
                {
                    if (tenant.LastInfoSentId <= 0)
                    {
                        var newestInfo = repository.InfoSet.OrderByDescending(i => i.Id).FirstOrDefault();
                        if (newestInfo != null) tenant.LastInfoSentId = newestInfo.Id;
                        this.LogInfo(string.Format("No items sent to {0} yet, last finding will start with info id {1}", tenant.Id, tenant.LastInfoSentId));
                        return;
                    }
                    var criteria = GetPreparedActiveWatchedCriteria(tenant);
                    var infoToSend = GetInfoToSend(criteria, repository);
                    this.LogInfo(string.Format("Found {0} items to send, using search criteria {1}", infoToSend.Length, criteria));
                    SendToUser(tenant, infoToSend, sender);
                }
            }
            catch (Exception exception)
            {
                this.LogInfo(string.Format("FindNewItemsAndSend failed: {0}\r\n{1}", exception.Message, exception.StackTrace));
                throw;
            }
        }

        private IEnumerable<SearchingCriteriaModel> GetPreparedActiveWatchedCriteria(Tenant tenant)
        {
            var criterii = TenantEx.GetActiveWatchedCriteria(tenant).Select(criteria =>
                                                                {
                                                                    criteria.MinId = tenant.LastInfoSentId;
                                                                    return criteria;
                                                                });
            return criterii;
        }

        /// <summary>
        /// Sends <see cref="infoToSend"/> to the <see cref="tenant"/>
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="infoToSend"></param>
        private void SendToUser(Tenant tenant, Info[] infoToSend, IMessageSender sender)
        {
            if (infoToSend.Length == 0) return;
            // If infos should be sent in separated messages, do it
            if (infoToSend.Length > 1 && !tenant.IsListOfInfoSendingEnabled)
            {
                this.LogInfo("Sending each info in one message");
                foreach (var info in infoToSend)
                {
                    SendToUser(tenant, new[] {info}, sender);
                }
                return;
            }
            if (maxItemsInOneMail > 0 && infoToSend.Length > maxItemsInOneMail)
            {
                this.LogInfo(string.Format("Segmenting message, maximum count {0} info in one", maxItemsInOneMail));
                for (var initialIndex = 0; initialIndex < infoToSend.Length; initialIndex += maxItemsInOneMail)
                {
                    var length = Math.Min((infoToSend.Length - initialIndex), maxItemsInOneMail);
                    var infoSegment = new Info[length];
                    Array.Copy(infoToSend, initialIndex, infoSegment, 0, length);
                    SendToUser(tenant, infoSegment, sender);
                }
                return;
            }
            // Send infos as a list (in one message)
            using (this.LogOperation(string.Format("Sending {0} infos to {1} (tenant {2})", infoToSend.Length, tenant.Mail, tenant.Id)))
            {
                sender.SendInfo(tenant, infoToSend);
                tenant.LastInfoSentId = Math.Max(infoToSend.Select(i => i.Id).Max(), tenant.LastInfoSentId);
                tenant.LastMailSentTime = DateTime.Now;
                this.LogInfo(string.Format("Successfully sent {0} infos to {1} (tenant {2}), next search will start with id {3}", infoToSend.Length, tenant.Mail, tenant.Id, tenant.LastInfoSentId));
            }
        }

        private Info[] GetInfoToSend(IEnumerable<SearchingCriteriaModel> criteria, BizInfoModelContainer repository)
        {
            var infoToSend = repository.ExecuteStoreQuery<Info>(criteria.GetQuery());
            return infoToSend.ToArray();
        }

        /// <summary>
        /// Checks if <see cref="tenant"/> should be processed - new items should be found and sent to the tenant.
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        private bool ShouldBeProcessed(Tenant tenant)
        {
            return !string.IsNullOrEmpty(tenant.Mail) && tenant.HasAnyActiveWatches()
                   && ((tenant.MinimumIntervalOfMailInMinutes <= 0) || (DateTime.Now > (tenant.LastMailSentTime + TimeSpan.FromMinutes(tenant.MinimumIntervalOfMailInMinutes))));
        }

        #region Nested type: CountingSender

        private class CountingSender : IMessageSender
        {
            private readonly IMessageSender sender = new MailMessageSender {MessageCreator = new MessageCreator()};
            internal int CountOfInfoSent { get; private set; }

            #region IMessageSender Members

            public void SendInfo(Tenant tenant, IEnumerable<Info> info)
            {
                sender.SendInfo(tenant, info);
                CountOfInfoSent += info.Count();
            }

            #endregion
        }

        #endregion
    }
}