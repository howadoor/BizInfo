using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.WebApp.Data
{
    public static class DataSource
    {
        public static BizInfoModelContainer container;

        public static BizInfoModelContainer Container
        {
            get
            {
                if (container == null) container = new BizInfoModelContainer();
                return container;
            }
        }
    }
}