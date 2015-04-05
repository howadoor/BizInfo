using System.Collections;
using System.Collections.Generic;

namespace Perenis.Core.General
{
    /// <summary>
    /// List of objects, just for Spring XML IoC configuration
    /// </summary>
    public class Objects : List<object>
    {
        public Objects(IList objects) : base(objects.Count)
        {
            foreach (object presenter in objects) Add(presenter);
        }
    }
}