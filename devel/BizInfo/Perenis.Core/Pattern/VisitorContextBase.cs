using System;
using System.Collections.Generic;

namespace Perenis.Core.Pattern
{
    /// <summary>
    /// Provides a base visitor context implementation with a simple scope management.
    /// </summary>
    /// <typeparam name="TScope">The type of the scope object.</typeparam>
    public class VisitorContextBase<TScope> : IVisitorContext
        where TScope : class
    {
        #region ------ Scope management -----------------------------------------------------------

        private readonly Stack<TScope> scopeStack = new Stack<TScope>();

        /// <summary>
        /// The stack of scopes entered by the visitor pattern.
        /// </summary>
        public Stack<TScope> ScopeStack
        {
            get { return scopeStack; }
        }

        /// <summary>
        /// Enters the given <paramref name="scope"/> (i.e. pushed it on top of the <see cref="ScopeStack"/>).
        /// </summary>
        /// <param name="scope">The scope being entered.</param>
        public virtual void EnterScope(TScope scope)
        {
            ScopeStack.Push(scope);
        }

        /// <summary>
        /// Leaves the current scope ensuring it's equal to the expected <paramref name="scope"/> 
        /// (i.e. pops it from the top of the <see cref="ScopeStack"/>).        
        /// </summary>
        /// <param name="scope">The expected scope being left.</param>
        /// <exception cref="InvalidOperationException">Either when the <see cref="ScopeStack"/> is
        /// empty or the top-most scope isn't the expected <paramref name="scope"/>. It this case 
        /// the <see cref="ScopeStack"/> is left intact.</exception>
        public virtual void LeaveScope(TScope scope)
        {
            if (ScopeStack.Count == 0 || ScopeStack.Peek() != scope) throw new InvalidOperationException();
            ScopeStack.Pop();
        }

        #endregion
    }
}