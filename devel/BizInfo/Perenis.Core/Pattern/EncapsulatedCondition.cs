using System;

namespace Perenis.Core.Pattern
{
    /// <summary>
    /// Provides access to an executable condition.
    /// </summary>
    public interface IEncapsulatedCondition
    {
        /// <summary>
        /// Executes the condition.
        /// </summary>
        /// <param name="obj">The object passed to the condition for evaluation.</param>
        /// <returns>The result of the evaluation.</returns>
        bool Execute(object obj);
    }

    /// <summary>
    /// Encapsulates an executable condition represented by a <see cref="Predicate{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the </typeparam>
    public class EncapsulatedCondition<T> : IEncapsulatedCondition
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="condition">The condition to be encapsulated.</param>
        public EncapsulatedCondition(Predicate<T> condition)
        {
            Condition = condition;
        }

        #region ------ Implementation of the IEncapsulatedCondition interface -------------------------

        bool IEncapsulatedCondition.Execute(object obj)
        {
            // make sure the context is of the right type
            var obj2 = obj as T;
            if (obj2 == null) return false;

            // evaluate the condition
            return Condition(obj2);
        }

        #endregion

        /// <summary>
        /// The encapsulated condition.
        /// </summary>
        public Predicate<T> Condition { get; set; }

        /// <summary>
        /// Executes the condition.
        /// </summary>
        /// <param name="obj">The object passed to the condition for evaluation.</param>
        /// <returns>The result of the evaluation.</returns>
        public bool Execute(T obj)
        {
            return ((IEncapsulatedCondition) this).Execute(obj);
        }
    }
}