using System;
using System.Collections;
using System.Collections.Generic;
using BizInfo.Model.Entities;

namespace BizInfo.Harvesting.Services.Processing.CompanyScoreTools
{
    /// <summary>
    /// TODO: Consider do not load everything to the memory. Maybe better use it like cache?
    /// </summary>
    internal class FragmentsOccurencyDictionary : Dictionary<string, int>
    {
        internal FragmentsOccurencyDictionary()
        {
            ReloadFromContainer();
        }

        private void ReloadFromContainer()
        {
            lock (this)
            {
                Clear();
                using (var container = new BizInfoModelContainer())
                {
                    foreach (var occurency in container.OccurencySet)
                    {
                        Add(occurency.Fragment, occurency.Count);
                    }
                }
            }
        }
    }
}