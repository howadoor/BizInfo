using System;
using System.Reflection;

#if NUNIT
using NUnit.Framework;
#endif

namespace Perenis.Core.Reflection
{
    public static class PropertyFieldAbstractor
    {
        /// <summary>
        /// Retrieves the value of a property or field recursively based on the input qualified name
        /// <paramref name="qName"/>.
        /// The qualifed name allows to retrieve values of inner propeties and fields.
        /// Example:
        /// class InnerInstance
        /// {
        ///     public int foo;
        /// }
        /// 
        /// class OuterInstance 
        /// {
        ///     public InnerInstance InnerInstance { get; set; }
        /// }
        /// 
        /// void foo() 
        /// {
        ///     OuterInstance outerInstance = new ...
        ///     int fooValue = (int) GetValue(outerInstance, "InnerInstance.foo");
        /// }
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="qName"></param>
        /// <returns></returns>
        public static object GetValue(this object obj, string qName)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (qName == null)
                throw new ArgumentNullException("qName");

            if (string.IsNullOrEmpty(qName))
                throw new ArgumentException("Qualified name may not be an empty string", "qName");

            int firstDot = qName.IndexOf(".");
            string localMemberName = firstDot != -1 ? qName.Substring(0, firstDot) : qName;

            object result = null;

            // try a property
            PropertyInfo property = obj.GetType().GetProperty(localMemberName);
            if (property != null)
            {
                result = property.GetValue(obj, null);
            }
            else
            {
                // then a field
                FieldInfo field = obj.GetType().GetField(localMemberName);
                if (field != null)
                    result = field.GetValue(obj);
            }

            if (result != null)
            {
                string qualifiedNameLeft = firstDot != -1 ? qName.Substring(firstDot + 1) : null;
                if (!string.IsNullOrEmpty(qualifiedNameLeft))
                    result = GetValue(result, qualifiedNameLeft);
            }

            return result;
        }
    }

#if NUNIT

    #region ------ Unit tests ---------------------------------------------------------------------

    [TestFixture]
    public class MailerTests
    {
        [Test]
        public void TestGetValueQualified()
        {
            DummyData o = new DummyData();
            o.Var2 = 3;
            o.DummyDataItem.Var5 = 7;

            int var2 = (int) o.GetValue("Var2");
            Assert.AreEqual(o.Var2, var2);

            int var5 = (int) o.GetValue("DummyDataItem.Var5");
            Assert.AreEqual(o.DummyDataItem.Var5, var5);

            int var6 = (int)PropertyFieldAbstractor.GetValue(o, "DummyDataItem.Inner.Var6");
            Assert.AreEqual(o.DummyDataItem.Inner.Var6, var6);
        }

        class Inner
        {
            public int Var6 = 11;
        }

        class DummyDataItem
        {
            public string Var4 { get; set; }
            public int Var5 { get; set; }
            public Inner Inner = new Inner();
        }

        class DummyData
        {
            public string Var1 { get; set; }
            public int Var2 { get; set; }
            public DummyDataItem DummyDataItem = new DummyDataItem();
            public List<DummyDataItem> Enum3 { get; set; }
        }
    }

    #endregion

#endif
}