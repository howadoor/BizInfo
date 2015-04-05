using System;

namespace Perenis.Core.Pattern
{
    /// <summary>
    /// Provides a factory for a specific visitor pattern and manages it's execution context.
    /// </summary>
    public abstract class VisitorDriver<TVisitorDriver, TIVisitorContext, TVisitorContext, TVisitor, TAcceptor, TFilter>
        where TVisitorDriver : VisitorDriver<TVisitorDriver, TIVisitorContext, TVisitorContext, TVisitor, TAcceptor, TFilter>, new()
        where TIVisitorContext : IVisitorContext
        where TVisitorContext : TIVisitorContext, new()
        where TVisitor : Visitor<TVisitorContext, TAcceptor, TFilter>, new()
        where TAcceptor : class, IAcceptor<TVisitorContext>
        where TFilter : class, IFilter<TVisitorContext>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks>
        /// When using this constructor, no visitor is associated with the new instance.
        /// </remarks>
        protected VisitorDriver()
        {
            // create the execution context
            Context = new TVisitorContext();
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference.</exception>
        /// <remarks>
        /// When using this constructor, the user is reponsible to bind the supplied <paramref name="visitor"/>
        /// instance to an acceptor and optionally to a filter.
        /// </remarks>
        protected VisitorDriver(TVisitor visitor)
            : this()
        {
            if (visitor == null) throw new ArgumentNullException("visitor");

            Visitor = visitor;
            Visitor.Context = (TVisitorContext) Context;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="acceptor">The acceptor object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="acceptor"/> is a null reference.</exception>
        /// <remarks>
        /// When using this constructor, the supplied <paramref name="visitor"/> is bound with the
        /// given <paramref name="acceptor"/>; the filter left default.
        /// </remarks>
        protected VisitorDriver(TVisitor visitor, TAcceptor acceptor)
            : this()
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            if (acceptor == null) throw new ArgumentNullException("acceptor");

            Visitor = visitor;
            Visitor.Context = (TVisitorContext) Context;
            Visitor.Acceptor = acceptor;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="acceptor">The acceptor object.</param>
        /// <param name="filter">The filter object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="acceptor"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="filter"/> is a null reference.</exception>
        /// <remarks>
        /// When using this constructor, the supplied <paramref name="visitor"/> is bound with the
        /// given <paramref name="acceptor"/> and filter.
        /// </remarks>
        protected VisitorDriver(TVisitor visitor, TAcceptor acceptor, TFilter filter)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            if (acceptor == null) throw new ArgumentNullException("acceptor");
            if (filter == null) throw new ArgumentNullException("filter");

            Visitor = visitor;
            Visitor.Context = (TVisitorContext) Context;
            Visitor.Acceptor = acceptor;
            Visitor.Filter = filter;
        }

        /// <summary>
        /// The execution context of the visitor pattern instance.
        /// </summary>
        public TIVisitorContext Context { get; private set; }

        /// <summary>
        /// The visitor instance.
        /// </summary>
        public TVisitor Visitor { get; set; }

        /// <summary>
        /// The corresponding acceptor instance.
        /// </summary>
        public TAcceptor Acceptor
        {
            get { return Visitor.Acceptor; }
        }

        /// <summary>
        /// The corresponding filter instance.
        /// </summary>
        public TFilter Filter
        {
            get { return Visitor.Filter; }
        }

        /// <summary>
        /// Creates a new visitor pattern instance, and initializes it's context.
        /// </summary>
        /// <param name="acceptor">The acceptor object.</param>
        /// <returns>The driver of the visitor pattern instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="acceptor"/> is a null reference.</exception>
        /// TODO factor out the factory concern, for example by moving the factory methods out into separate factory class
        /// TODO consider a case when the descendant's construction involves mandatory construction parameters
        protected static TVisitorDriver CreateBase(TAcceptor acceptor)
        {
            if (acceptor == null) throw new ArgumentNullException("acceptor");

            var visitorDriver = new TVisitorDriver();
            visitorDriver.Visitor = new TVisitor();
            visitorDriver.Visitor.Context = (TVisitorContext) visitorDriver.Context;
            visitorDriver.Visitor.Acceptor = acceptor;
            return visitorDriver;
        }

        /// <summary>
        /// Creates a new visitor pattern instance, and initializes it's context.
        /// </summary>
        /// <param name="acceptor">The acceptor object.</param>
        /// <param name="filter">The filter object.</param>
        /// <returns>The driver of the visitor pattern instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="acceptor"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="filter"/> is a null reference.</exception>
        protected static TVisitorDriver CreateBase(TAcceptor acceptor, TFilter filter)
        {
            if (acceptor == null) throw new ArgumentNullException("acceptor");
            if (filter == null) throw new ArgumentNullException("filter");

            var visitorDriver = new TVisitorDriver();
            visitorDriver.Visitor = new TVisitor();
            visitorDriver.Visitor.Context = (TVisitorContext) visitorDriver.Context;
            visitorDriver.Visitor.Acceptor = acceptor;
            visitorDriver.Visitor.Filter = filter;
            return visitorDriver;
        }
    }
}