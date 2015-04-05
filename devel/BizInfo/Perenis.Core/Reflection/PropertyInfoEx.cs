using System;
using System.Globalization;
using System.Reflection;
using Perenis.Core.Exceptions;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// <see cref="PropertyInfo"/> manipulation helpers.
    /// </summary>
    public static class PropertyInfoEx
    {
        /// <summary>
        /// Calls the <see cref="PropertyInfo.GetValue(object,object[])"/> method and in case 
        /// a <see cref="TargetInvocationException"/> is thrown, rethrows the unboxed and wrapped
        /// real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Get</c> method.</exception>
        /// <remarks>
        /// The <c>index</c> parameter is set to null. See <see cref="PropertyInfo.GetValue(object,object[])"/> 
        /// for more details on the semantics of this method and its parameters.
        /// </remarks>
        public static object GetValueUnbox(this PropertyInfo propertyInfo, object obj)
        {
            try
            {
                return propertyInfo.GetPropertyInfoWithGetMethodOrThrow().GetValue(obj, null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }

        /// <summary>
        /// Calls the <see cref="PropertyInfo.GetValue(object,object[])"/> method and in case 
        /// a <see cref="TargetInvocationException"/> is thrown, rethrows the unboxed and wrapped
        /// real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <returns>The return value casted to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Get</c> method.</exception>
        /// <remarks>
        /// The <c>index</c> parameter is set to null. See <see cref="PropertyInfo.GetValue(object,object[])"/> 
        /// for more details on the semantics of this method and its parameters.
        /// </remarks>
        public static T GetValueUnbox<T>(this PropertyInfo propertyInfo, object obj)
        {
            try
            {
                return (T) propertyInfo.GetPropertyInfoWithGetMethodOrThrow().GetValue(obj, null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }

        /// <summary>
        /// Calls the <see cref="PropertyInfo.GetValue(object,object[])"/> method and in case 
        /// a <see cref="TargetInvocationException"/> is thrown, rethrows the unboxed and wrapped
        /// real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Get</c> method.</exception>
        /// <remarks>
        /// See <see cref="PropertyInfo.GetValue(object,object[])"/> for more details on the semantics 
        /// of this method and its parameters.
        /// </remarks>
        public static object GetValueUnbox(this PropertyInfo propertyInfo, object obj, object[] index)
        {
            try
            {
                return propertyInfo.GetPropertyInfoWithGetMethodOrThrow().GetValue(obj, index);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }

        /// <summary>
        /// Calls the <see cref="PropertyInfo.GetValue(object,object[])"/> method and in case 
        /// a <see cref="TargetInvocationException"/> is thrown, rethrows the unboxed and wrapped
        /// real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <returns>The return value casted to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Get</c> method.</exception>
        /// <remarks>
        /// See <see cref="PropertyInfo.GetValue(object,object[])"/> for more details on the semantics 
        /// of this method and its parameters.
        /// </remarks>
        public static T GetValueUnbox<T>(this PropertyInfo propertyInfo, object obj, object[] index)
        {
            try
            {
                return (T) propertyInfo.GetPropertyInfoWithGetMethodOrThrow().GetValue(obj, index);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }

        /// <summary>
        /// Calls the <see cref="PropertyInfo.GetValue(object,System.Reflection.BindingFlags,System.Reflection.Binder,object[],System.Globalization.CultureInfo)"/> 
        /// method and in case a <see cref="TargetInvocationException"/> is thrown, rethrows the 
        /// unboxed and wrapped real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Get</c> method.</exception>
        /// <remarks>
        /// See <see cref="PropertyInfo.GetValue(object,object[])"/> for more details on the semantics 
        /// of this method and its parameters.
        /// </remarks>
        public static object GetValueUnbox(this PropertyInfo propertyInfo, object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            try
            {
                return propertyInfo.GetPropertyInfoWithGetMethodOrThrow().GetValue(obj, invokeAttr, binder, index, culture);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }

        /// <summary>
        /// Calls the <see cref="PropertyInfo.GetValue(object,System.Reflection.BindingFlags,System.Reflection.Binder,object[],System.Globalization.CultureInfo)"/> 
        /// method and in case a <see cref="TargetInvocationException"/> is thrown, rethrows the 
        /// unboxed and wrapped real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <returns>The return value casted to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Get</c> method.</exception>
        /// <remarks>
        /// See <see cref="PropertyInfo.GetValue(object,object[])"/> for more details on the semantics 
        /// of this method and its parameters.
        /// </remarks>
        public static T GetValueUnbox<T>(this PropertyInfo propertyInfo, object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            try
            {
                return (T) propertyInfo.GetPropertyInfoWithGetMethodOrThrow().GetValue(obj, invokeAttr, binder, index, culture);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }

        /// <summary>
        /// Calls the <see cref="PropertyInfo.SetValue(object,object[])"/> method and in case 
        /// a <see cref="TargetInvocationException"/> is thrown, rethrows the unboxed and wrapped
        /// real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Set</c> method.</exception>
        /// <remarks>
        /// The <c>index</c> parameter is set to null. See <see cref="PropertyInfo.SetValue(object,object[])"/> 
        /// for more details on the semantics of this method and its parameters.
        /// </remarks>
        public static void SetValueUnbox(this PropertyInfo propertyInfo, object obj, object value)
        {
            try
            {
                propertyInfo.GetPropertyInfoWithSetMethodOrThrow().SetValue(obj, value, null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }

        /// <summary>
        /// Calls the <see cref="PropertyInfo.SetValue(object,object[])"/> method and in case 
        /// a <see cref="TargetInvocationException"/> is thrown, rethrows the unboxed and wrapped
        /// real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Set</c> method.</exception>
        /// <remarks>
        /// See <see cref="PropertyInfo.SetValue(object,object[])"/> for more details on the semantics 
        /// of this method and its parameters.
        /// </remarks>
        public static void SetValueUnbox(this PropertyInfo propertyInfo, object obj, object value, object[] index)
        {
            try
            {
                propertyInfo.GetPropertyInfoWithSetMethodOrThrow().SetValue(obj, value, index);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }

        /// <summary>
        /// Calls the <see cref="PropertyInfo.SetValue(object,System.Reflection.BindingFlags,System.Reflection.Binder,object[],System.Globalization.CultureInfo)"/> 
        /// method and in case a <see cref="TargetInvocationException"/> is thrown, rethrows the 
        /// unboxed and wrapped real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Set</c> method.</exception>
        /// <remarks>
        /// See <see cref="PropertyInfo.SetValue(object,object[])"/> for more details on the semantics 
        /// of this method and its parameters.
        /// </remarks>
        public static void SetValueUnbox(this PropertyInfo propertyInfo, object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            try
            {
                propertyInfo.GetPropertyInfoWithSetMethodOrThrow().SetValue(obj, value, invokeAttr, binder, index, culture);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }

        /// <summary>
        /// Looks-up a <see cref="PropertyInfo"/> instance, starting from the given <paramref name="propertyInfo"/> down
        /// along its declaring type's inheritance hierarchy, that has a <c>Get</c> method.
        /// </summary>
        /// <param name="propertyInfo">Metadata of a property.</param>
        /// <returns>Either the given <paramref name="propertyInfo"/> or a <see cref="PropertyInfo"/>
        /// of the corresponding property declared in one of the declaring type's ancestors.</returns>
        public static PropertyInfo GetPropertyInfoWithGetMethod(this PropertyInfo propertyInfo)
        {
            while (propertyInfo != null && !propertyInfo.CanRead)
            {
                Type baseType = propertyInfo.DeclaringType.BaseType;
                propertyInfo = baseType != null ? baseType.GetProperty(propertyInfo.Name) : null;
            }
            return propertyInfo;
        }

        /// <summary>
        /// Looks-up a <see cref="PropertyInfo"/> instance, starting from the given <paramref name="propertyInfo"/> down
        /// along its declaring type's inheritance hierarchy, that has a <c>Set</c> method.
        /// </summary>
        /// <param name="propertyInfo">Metadata of a property.</param>
        /// <returns>Either the given <paramref name="propertyInfo"/> or a <see cref="PropertyInfo"/>
        /// of the corresponding property declared in one of the declaring type's ancestors.</returns>
        public static PropertyInfo GetPropertyInfoWithSetMethod(this PropertyInfo propertyInfo)
        {
            while (propertyInfo != null && !propertyInfo.CanWrite)
            {
                Type baseType = propertyInfo.DeclaringType.BaseType;
                propertyInfo = baseType != null ? baseType.GetProperty(propertyInfo.Name) : null;
            }
            return propertyInfo;
        }

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// See <see cref="GetPropertyInfoWithGetMethod"/> for description.
        /// </summary>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Get</c> method.</exception>
        private static PropertyInfo GetPropertyInfoWithGetMethodOrThrow(this PropertyInfo propertyInfo)
        {
            PropertyInfo orgPropertyInfo = propertyInfo;
            propertyInfo = propertyInfo.GetPropertyInfoWithGetMethod();
            if (propertyInfo == null) throw new ArgumentException(String.Format("“Get” method was not found in property “{0}.{1}”.", orgPropertyInfo.DeclaringType.FullName, orgPropertyInfo.Name));
            return propertyInfo;
        }

        /// <summary>
        /// See <see cref="GetPropertyInfoWithSetMethod"/> for description.
        /// </summary>
        /// <exception cref="ArgumentException">When neither <paramref name="propertyInfo"/> nor any
        /// of its ancestors has a <c>Set</c> method.</exception>
        private static PropertyInfo GetPropertyInfoWithSetMethodOrThrow(this PropertyInfo propertyInfo)
        {
            PropertyInfo orgPropertyInfo = propertyInfo;
            propertyInfo = propertyInfo.GetPropertyInfoWithSetMethod();
            if (propertyInfo == null) throw new ArgumentException(String.Format("“Set” method was not found in property “{0}.{1}”.", orgPropertyInfo.DeclaringType.FullName, orgPropertyInfo.Name));
            return propertyInfo;
        }

        #endregion

        #region ------ Unit tests of the PropertyInfoEx class -------------------------------------------

#if NUNIT

    /// <summary>
    /// Unit tests of the <see cref="PropertyInfoEx"/> class.
    /// </summary>
        [TestFixture]
        public class PropertyInfoEx_Tests
        {
            private class T1
            {
                public virtual string PS
                {
                    set { }
                }

                public virtual string PG
                {
                    get { return "T1"; }
                }
            }

            private class T2<T> : T1
            {
                public virtual T P1 { get; set; }

                public virtual int P2 { get; set; }
            }

            private class T3 : T2<int>
            {
                public override int P1
                {
                    get { return 1; }
                }

                public override int P2
                {
                    get { return 2; }
                    set { base.P2 = value + 2; }
                }

                public override string PS
                {
                    set { base.PS = value + "T3"; }
                }

                public override string PG
                {
                    get { return "T3"; }
                }
            }

            private PropertyInfo T1PS = typeof(T1).GetProperty("PS");
            private PropertyInfo T1PG = typeof(T1).GetProperty("PG");
            private PropertyInfo T2intP1 = typeof(T2<int>).GetProperty("P1");
            private PropertyInfo T2intP2 = typeof(T2<int>).GetProperty("P2");
            private PropertyInfo T3PS = typeof(T3).GetProperty("PS");
            private PropertyInfo T3PG = typeof(T3).GetProperty("PG");
            private PropertyInfo T3P1 = typeof(T3).GetProperty("P1");
            private PropertyInfo T3P2 = typeof(T3).GetProperty("P2");

            /// <summary>
            /// Tests of the <see name="GetPropertyInfoWithGetMethod"/> method.
            /// </summary>
            [Test]
            public void TestGetPropertyInfoWithGetMethod()
            {
                Assert.AreEqual(null, T1PS.GetPropertyInfoWithGetMethod());
                Assert.AreEqual(T1PG, T1PG.GetPropertyInfoWithGetMethod());
                Assert.AreEqual(T2intP1, T2intP1.GetPropertyInfoWithGetMethod());
                Assert.AreEqual(T2intP2, T2intP2.GetPropertyInfoWithGetMethod());
                Assert.AreEqual(null, T3PS.GetPropertyInfoWithGetMethod());
                Assert.AreEqual(T3PG, T3PG.GetPropertyInfoWithGetMethod());
                Assert.AreEqual(T3P1, T3P1.GetPropertyInfoWithGetMethod());
                Assert.AreEqual(T3P2, T3P2.GetPropertyInfoWithGetMethod());
            }

            /// <summary>
            /// Tests of the <see name="GetPropertyInfoWithSetMethod"/> method.
            /// </summary>
            [Test]
            public void TestGetPropertyInfoWithSetMethod()
            {
                Assert.AreEqual(T1PS, T1PS.GetPropertyInfoWithSetMethod());
                Assert.AreEqual(null, T1PG.GetPropertyInfoWithSetMethod());
                Assert.AreEqual(T2intP1, T2intP1.GetPropertyInfoWithSetMethod());
                Assert.AreEqual(T2intP2, T2intP2.GetPropertyInfoWithSetMethod());
                Assert.AreEqual(T3PS, T3PS.GetPropertyInfoWithSetMethod());
                Assert.AreEqual(null, T3PG.GetPropertyInfoWithSetMethod());
                Assert.AreEqual(T2intP1, T3P1.GetPropertyInfoWithSetMethod());
                Assert.AreEqual(T3P2, T3P2.GetPropertyInfoWithSetMethod());
            }
        }

#endif

        #endregion
    }
}