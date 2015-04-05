using System.Collections;
using System.Collections.Generic;
using Spring.Context;
using Spring.Context.Support;

namespace Perenis.Core.Spring
{
    /// <summary>
    /// Collects all implementors of TType from given IApplicationContext (or root application context)
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public class AllImplementors<TType> : List<TType>
        where TType : class
    {
        public AllImplementors() : this(ContextRegistry.GetContext())
        {
        }

        public AllImplementors(IApplicationContext context)
        {
            ICollection implementors = context.GetObjectsOfType(typeof (TType)).Values;
            Clear();
            foreach (object implementor in implementors) Add(implementor as TType);
        }
    }
}