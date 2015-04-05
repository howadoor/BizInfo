using System.Collections.Generic;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.Base
{
    public class WebSourcesModel
    {
        public IEnumerable<Info> WebSources { get; set; }
        public BizInfoModelContainer Container { get; set; }
    }
}