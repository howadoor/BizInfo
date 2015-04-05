using System.Linq;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.Base
{
    public class SearchingMonitorModel
    {
        public SearchingMonitorModel()
        {
            Update();
        }

        public SearchingLog[] Items { get; private set; }

        private void Update()
        {
            using (var repository = new BizInfoModelContainer())
            {
                Items = repository.SearchingLogSet.OrderByDescending(sl => sl.Start).Take(100).ToArray();
            }
        }
    }
}