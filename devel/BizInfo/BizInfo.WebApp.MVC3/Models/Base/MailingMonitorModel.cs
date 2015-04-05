using System.Linq;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.Base
{
    public class MailingMonitorModel
    {
        public MailingMonitorModel()
        {
            Update();
        }

        public SentMessages[] Items { get; private set; }

        private void Update()
        {
            using (var repository = new BizInfoModelContainer())
            {
                Items = repository.SentMessagesSet.OrderByDescending(sm => sm.SendTime).Take(100).ToArray();
            }
        }
    }
}