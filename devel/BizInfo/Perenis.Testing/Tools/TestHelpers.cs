using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Perenis.Testing.Tools
{
    /// <summary>
    /// Helpers methods used in testing
    /// </summary>
    public static class TestHelpers
    {
        /// <summary>
        /// Checks if exception of type <see cref="TException"/> is thrown by <see cref="action"/>
        /// </summary>
        /// <typeparam name="TException">Type of exception expected</typeparam>
        /// <param name="action">Action which should throw an exception</param>
        /// <returns>Exception thrown by <see cref="action"/></returns>
        public static TException AssertThrows<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException ex)
            {
                return ex;
            }
            Assert.Fail("Expected exception was not thrown");
            return null;
        }

        /// <summary>
        /// Checks if content of two enumerables is the same
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="items1"></param>
        /// <param name="items2"></param>
        public static void AssertAreEqual<TItem>(IEnumerable<TItem> items1, IEnumerable<TItem> items2)
        {
            Assert.AreEqual(items1.Count(), items2.Count());
            Assert.IsTrue(items1.All(item1 => items2.Contains(item1)));
        }
    }
}
