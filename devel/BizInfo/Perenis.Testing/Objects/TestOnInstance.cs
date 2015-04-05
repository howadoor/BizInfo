using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perenis.Testing.Tools;

namespace Perenis.Testing.Objects
{
    /// <summary>
    /// Represents test which is performed on instance of <see cref="TInstance"/> type.
    /// </summary>
    /// <remarks>
    /// Base class for tests which creates instance of some type, performs test method on it then disposes it.
    /// </remarks>
    /// <typeparam name="TInstance">Type of the instance created for using in test(s)</typeparam>
    [TestClass]
    public abstract class TestOnInstance<TInstance> where TInstance : class
    {
        /// <summary>
        /// Test of creation and disposing instance of <see cref="TInstance"/>
        /// </summary>
        // TODO: Temporarily disabled [TestMethod] attribute because it is not able due to specific behavior of MSTest
        // [TestMethod]
        public void TestInstanceCreationAndDisposing()
        {
            PerformTest<object>(DoNothing);
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="object">Argument of any type</param>
        protected void DoNothing(object @object)
        {
            // intentionaly does nothing
        }

        /// <summary>
        /// Creates an instance of <see cref="TTestArgument"/> is created, <see cref="test"/> performed on it then it is disposed.
        /// </summary>
        /// <param name="test">Action performing test</param>
        protected void PerformTest<TTestArgument>(Action<TTestArgument> test) where TTestArgument : class
        {
            Assert.IsNotNull(test);
            // Check if test throws an exception with argument of null
            TestHelpers.AssertThrows<Exception>(() => test(null));
            // Create instance, check the type and perform test
            var instance = CreateInstance();
            Assert.IsNotNull(instance);
            Assert.IsTrue(instance is TTestArgument);
            try
            {
                test(instance as TTestArgument);
            }
            finally
            {
                DisposeInstance(instance);
            }
        }

        /// <summary>
        /// Responsible for disposing <see cref="instance"/>
        /// </summary>
        /// <param name="instance">Instance of <see cref="TInstance"/> to be disposed</param>
        public abstract void DisposeInstance(TInstance instance);

        /// <summary>
        /// Creates new instance of <see cref="TInstance"/>
        /// </summary>
        /// <returns></returns>
        public abstract TInstance CreateInstance();
    }
}