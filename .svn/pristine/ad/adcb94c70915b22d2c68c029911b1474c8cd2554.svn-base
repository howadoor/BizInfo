using System;
using System.Text;
using Perenis.Core.Pattern;

#if NUNIT
using NUnit.Framework;
#endif

namespace Perenis.Core.Component
{
    /// <summary>
    /// Provides name-resolution service for enumeration values.
    /// </summary>
    /// <remarks>
    /// The EBNF syntax of the name constructed for an enumeration value is as follows:
    /// <code>
    /// { Enclosing class name '.' } Enumeration type name '.' Enumeration field name
    /// </code>
    /// </remarks>
    public class EnumStructuredNamingResolver : Singleton<EnumStructuredNamingResolver>, IStructuredNamingResolver<object>
    {
        #region ------ Implementation of the IStructuredNamingResolver interface ------------------

        public string GetStructuredName(object nameObject)
        {
            if (nameObject == null) throw new ArgumentNullException("nameObject");
            if (!nameObject.GetType().IsEnum) return null;

            var strBuilder = new StringBuilder();

            // use the name of the enumeration constant as the final component of the policy name
            strBuilder.Append(nameObject.ToString());

            // use short names of embedded type names as components of the policy name
            Type type = nameObject.GetType();
            while (type != null && type != typeof (Object) && type != typeof (object))
            {
                strBuilder.Insert(0, ".");
                strBuilder.Insert(0, type.Name);
                type = type.DeclaringType;
            }

            return strBuilder.ToString();
        }

        #endregion
    }

    #region ------ Unit tests of the EnumStructuredNamingResolver class ---------------------------

#if NUNIT

    /// <summary>
    /// Unit tests of the <see cref="EnumStructuredNamingResolver"/> class.
    /// </summary>
    [TestFixture]
    public class EnumStructuredNamingResolver_Tests
    {
        private class Container
        {
            public enum StandardEnum
            {
                A,
                B,
                C,
            }

            [Flags]
            public enum FlagsEnum
            {
                X = 0x01,
                Y = 0x02,
                Z = 0x04,
            }
        }

        [Test]
        public void Test()
        {
            Assert.AreEqual("EnumStructuredNamingResolver_Tests.Container.StandardEnum.A", EnumStructuredNamingResolver.Instance.GetStructuredName(Container.StandardEnum.A));
            Assert.AreEqual("EnumStructuredNamingResolver_Tests.Container.StandardEnum.B", EnumStructuredNamingResolver.Instance.GetStructuredName(Container.StandardEnum.B));
            Assert.AreEqual("EnumStructuredNamingResolver_Tests.Container.StandardEnum.C", EnumStructuredNamingResolver.Instance.GetStructuredName(Container.StandardEnum.C));
            Assert.AreEqual("EnumStructuredNamingResolver_Tests.Container.FlagsEnum.X", EnumStructuredNamingResolver.Instance.GetStructuredName(Container.FlagsEnum.X));
            Assert.AreEqual("EnumStructuredNamingResolver_Tests.Container.FlagsEnum.X, Y", EnumStructuredNamingResolver.Instance.GetStructuredName(Container.FlagsEnum.X | Container.FlagsEnum.Y));
            Assert.AreEqual("EnumStructuredNamingResolver_Tests.Container.FlagsEnum.X, Y, Z", EnumStructuredNamingResolver.Instance.GetStructuredName(Container.FlagsEnum.X | Container.FlagsEnum.Y | Container.FlagsEnum.Z));
            Assert.AreEqual("EnumStructuredNamingResolver_Tests.Container.FlagsEnum.128", EnumStructuredNamingResolver.Instance.GetStructuredName((Container.FlagsEnum)128));
            Assert.AreEqual(null, EnumStructuredNamingResolver.Instance.GetStructuredName(128));
        }
    }

#endif

    #endregion
}