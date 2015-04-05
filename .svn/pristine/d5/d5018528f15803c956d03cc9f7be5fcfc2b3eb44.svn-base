using System;

namespace Perenis.Core.Pattern
{
    /// <summary>
    /// Implements the visitor pattern.
    /// </summary>
    /// <typeparam name="TVisitorContext">The type of the context.</typeparam>
    /// <typeparam name="TAcceptor">The type of the associated acceptor.</typeparam>
    /// <typeparam name="TFilter">The type of the associated filter.</typeparam>
    public abstract class Visitor<TVisitorContext, TAcceptor, TFilter> : IVisitorContextAware<TVisitorContext>
        where TVisitorContext : IVisitorContext
        where TAcceptor : class, IAcceptor<TVisitorContext>
        where TFilter : class, IFilter<TVisitorContext>
    {
        private TAcceptor acceptor;

        private TFilter filter;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        protected Visitor()
        {
            Filter = GetDefaultFilter();
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="acceptor">The acceptor of objects visited by this instance.</param>
        protected Visitor(TAcceptor acceptor)
            : this()
        {
            if (acceptor == null) throw new ArgumentNullException("acceptor");
            Acceptor = acceptor;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="acceptor">The acceptor of objects visited by this instance.</param>
        /// <param name="filter">The filter of objects visited by this instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="acceptor"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="filter"/> is a null reference.</exception>
        protected Visitor(TAcceptor acceptor, TFilter filter)
        {
            if (acceptor == null) throw new ArgumentNullException("acceptor");
            if (filter == null) throw new ArgumentNullException("filter");

            Acceptor = acceptor;
            Filter = filter;
        }

        /// <summary>
        /// The acceptor of objects visited by this instance.
        /// </summary>
        public TAcceptor Acceptor
        {
            get { return acceptor; }
            set
            {
                acceptor = value;
                if (acceptor != null) acceptor.Context = Context;
            }
        }

        /// <summary>
        /// The filter of objects visitable by this instance.
        /// </summary>
        public TFilter Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                if (filter != null) filter.Context = Context;
            }
        }

        /// <summary>
        /// Provides the default filter.
        /// </summary>
        /// <returns></returns>
        protected abstract TFilter GetDefaultFilter();

        /// <summary>
        /// Checks that this instance is properly configured.
        /// </summary>
        protected virtual void EnsureConfigured()
        {
            if (Acceptor == null) throw new InvalidOperationException("Acceptor isn't set");
            if (Filter == null) throw new InvalidOperationException("Filter isn't set");
        }

        #region ------ Implementation of the IVisitorContextAware interface -----------------------

        private TVisitorContext context;

        public TVisitorContext Context
        {
            get { return context; }
            set
            {
                context = value;
                if (Acceptor != null) Acceptor.Context = context;
                if (Filter != null) Filter.Context = context;
            }
        }

        #endregion
    }
}