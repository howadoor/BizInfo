using System;
using System.Collections.Generic;

#if NUNIT
using NUnit.Framework;
#endif

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Imlpements <see cref="IComparable"/> on multiple <see cref="IComparable"/> instances.
    /// </summary>
    /// <remarks>
    /// The return values of <see cref="CompareTo(CompoundKey)"/> are as follows:
    /// 1 if the method argument is <c>null</c>
    /// number of key components of this subtracted by number of key components of the argument if the numbers don't equal
    /// components[i].CompareTo(other.components[i]) if the result is other than zero
    /// 0 otherwise
    /// </remarks>
    public class CompoundKey : IComparable, IComparable<CompoundKey>, IEquatable<CompoundKey>
    {
        private readonly IList<IComparable> components;

        #region Implementation of IComparable<CompoundKey>

        public int CompareTo(CompoundKey other)
        {
            return CompareTo((object) other);
        }

        #endregion

        #region Implementation of IEquatable<CompoundKey>

        public bool Equals(CompoundKey other)
        {
            return CompareTo(other) == 0;
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="components"></param>
        public CompoundKey(IEnumerable<IComparable> components)
        {
            if (components == null) throw new ArgumentNullException("components");
            this.components = new List<IComparable>(components);
            if (this.components.Count == 0) throw new ArgumentException("At least one key component must be supplied", "components");
            foreach (IComparable component in components) if (component == null) throw new ArgumentException("None of the key components may be null");
        }

        #region Implementation of IComparable

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (!(obj is CompoundKey)) throw new ArgumentException("Compatible type expected", "obj");

            var other = (CompoundKey) obj;
            int result = components.Count - other.components.Count;
            if (result != 0) return result;

            for (int i = 0; i < components.Count; i++)
            {
                result = components[i].CompareTo(other.components[i]);
                if (result != 0) break;
            }

            return result;
        }

        #endregion
    }

    #region ------ Unit Tests ---------------------------------------------------------------------

#if NUNIT
    [TestFixture]
    public class CompoundKeyTests
    {
        [Test]
        public void Test()
        {
            string comp1 = "aaa";
            string comp2 = "bbb";
            string comp3 = "ccc";

            CompoundKey key1 = new CompoundKey(new [] {comp1, comp2, comp3});
            CompoundKey key2 = new CompoundKey(new[] { comp1, comp2, comp3 });
            CompoundKey key3 = new CompoundKey(new[] { comp1, comp3 });
            CompoundKey key4 = new CompoundKey(new[] { comp1, comp3, comp2 });
            CompoundKey key5 = new CompoundKey(new[] { comp3, comp2, comp1 });

            Assert.IsTrue(key1.CompareTo(key2) == 0);
            Assert.IsTrue(key1.CompareTo(key3) > 0);
            Assert.IsTrue(key3.CompareTo(key4) < 0);
            Assert.IsTrue(key1.CompareTo(key4) < 0);
            Assert.IsTrue(key4.CompareTo(key5) < 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestComponentsCountConstraint()
        {
            CompoundKey key1 = new CompoundKey(new IComparable[0]);
            CompoundKey key2 = new CompoundKey(new IComparable[0]);
            key1.CompareTo(key2);
        }
    }
#endif

    #endregion
}