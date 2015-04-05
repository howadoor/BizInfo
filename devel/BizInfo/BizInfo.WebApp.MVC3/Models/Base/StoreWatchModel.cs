using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.Base
{
    public class StoreWatchModel
    {
        public StoreWatchModel(Tenant tenant)
        {
            Watches = tenant.WatchSet.ToArray();
        }

        public Watch[] Watches
        {
            get; private set;
        }

        public int MaxCountOfWatches
        {
            get { return 5; }
        }
    }
}