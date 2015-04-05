using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Perenis.Core.Pattern;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Provides access to attributes of a specific element (an <see cref="Assembly"/>, 
    /// <see cref="MemberInfo"/>, <see cref="Module"/>, or <see cref="ParameterInfo"/> instance)
    /// and allows applying dynamically created attributes to the element.
    /// </summary>
    /// <remarks>
    /// It's recommended to call one of the <see cref="AttributeManagerRegistry.Get"/> methods to gain 
    /// access to a specific <see cref="AttributeManager"/> instance and ensure consistent use dynamically
    /// applied attributes. Use the <see cref="AttributeManagerAttribute"/> to override the type of 
    /// the attribute manager to be used for a specific element.
    /// </remarks>
    /// <example>
    /// For example, a user-defined attribute manager may initialize dynamic attributes, or define 
    /// properties for easier access to specific attributes.
    /// <code>
    /// <![CDATA[
    /// // Represents an enhanced attribute manager providing direct access to [MyAttribute] instances.
    /// public class MyAttributeManager
    /// {
    ///     // Provides an attribute manager instance for a statically specified type implementing the IMyInterface.
    ///     public static MyAttributeManager Get&lt;T&gt;() where T : IMyInterface
    ///     {
    ///         return (MyAttributeManager)AttributeManagerRegistry.Instance.Get&lt;T&gt;();
    ///     }
    /// 
    ///     // Provides an attribute manager instance for a dynamically specified type implementing the IMyInterface.
    ///     public static MyAttributeManager Get(IMyInterface object)
    ///     {
    ///         return (MyAttributeManager)AttributeManagerRegistry.Instance.Get(object.GetType());
    ///     }
    /// 
    ///     // Provides direct access to an instance of the [MyAttribute] (if any).
    ///     public MyAttributeAttribute MyAttribute
    ///     {
    ///         get { return GetAttribute&lt;MyAttributeAttribute&gt;(); }
    ///     }
    /// }
    /// 
    /// // Designates that the MyClassBase and all it's ancestors shall use the MyAttributeManager.
    /// [AttributeManager(typeof(MyAttributeManager))]
    /// public class MyClassBase : IMyInterface { ... } 
    /// 
    /// // A descandant with the [MyAttribute] applied.
    /// [MyAttribute]
    /// public class MyClassDescendant : MyClassBase { ... }
    /// 
    /// ...
    /// 
    /// // accessing the [MyAttribute] instance applied to the MyClassDescendant class.
    /// MyAttributeManager.Get&lt;MyClassDescendant&gt;().MyAttribute
    /// ]]></code>
    /// </example>
    /// <seealso cref="AttributeManagerRegistry"/>
    /// <seealso cref="AttributeManagerAttribute"/>
    public class AttributeManager
    {
        /// <summary>
        /// Registers a dynamic attribute as if it was applied to the element.
        /// </summary>
        /// <param name="attr">The dynamic attribute.</param>
        /// <exception cref="ArgumentNullException"><paramref name="attr"/> is a null reference.</exception>
        public void AddAttribute(Attribute attr)
        {
            if (attr == null) throw new ArgumentNullException("attr");
            Type attrType = attr.GetType();
            if (!attributes.ContainsKey(attrType))
            {
                attributes[attrType] = new[] {attr};
            }
            else
            {
                Attribute[] old = attributes[attrType];
                attributes[attrType] = new Attribute[old.Length + 1];
                Array.Copy(old, attributes[attrType], old.Length);
                attributes[attrType][old.Length] = attr;
            }
        }

        /// <summary>
        /// Registers dynamic attributes as if they were applied to the element.
        /// </summary>
        /// <param name="attrs">The dynamic attributes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="attrs"/> is a null reference.</exception>
        public void AddAttributes(IEnumerable<Attribute> attrs)
        {
            if (attrs == null) throw new ArgumentNullException("attrs");
            foreach (Attribute attr in attrs) AddAttribute(attr);
        }

        /// <summary>
        /// Registers dynamic attributes as if they were applied to the element.
        /// </summary>
        /// <param name="attrs">The dynamic attributes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="attrs"/> is a null reference.</exception>
        public void AddAttributes(params Attribute[] attrs)
        {
            if (attrs == null) throw new ArgumentNullException("attrs");
            for (int i = 0; i < attrs.Length; i++) AddAttribute(attrs[i]);
        }

        /// <summary>
        /// Checks if an attribute of type <paramref name="attrType"/> is applied to the element.
        /// </summary>
        /// <param name="attrType">The type of the attribute to be checked for.</param>
        /// <returns><c>true</c> if a matching attribute is applied; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attrType"/> is a null reference.</exception>
        public bool HasAttribute(Type attrType)
        {
            return HasAttribute(attrType, true);
        }

        /// <summary>
        /// Checks if an attribute of type <paramref name="attrType"/> is applied to the element.
        /// </summary>
        /// <param name="attrType">The type of the attribute to be checked for.</param>
        /// <returns><c>true</c> if a matching attribute is applied; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attrType"/> is a null reference.</exception>
        public bool HasAttribute(Type attrType, bool inherit)
        {
            if (attrType == null) throw new ArgumentNullException("attrType");
            return GetFirstAttributeNonInherited(attrType) != null || (!inherit || parent == null ? false : parent.HasAttribute(attrType, inherit));
        }

        /// <summary>
        /// Checks if an attribute of type <typeparamref name="T"/> is applied to the element.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to be checked for.</typeparam>
        /// <returns><c>true</c> if a matching attribute is applied; otherwise, <c>false</c>.</returns>
        public bool HasAttribute<T>() where T : Attribute
        {
            return HasAttribute<T>(true);
        }

        /// <summary>
        /// Checks if an attribute of type <typeparamref name="T"/> is applied to the element.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to be checked for.</typeparam>
        /// <returns><c>true</c> if a matching attribute is applied; otherwise, <c>false</c>.</returns>
        public bool HasAttribute<T>(bool inherit) where T : Attribute
        {
            return GetFirstAttributeNonInherited<T>() != null || (!inherit || parent == null ? false : parent.HasAttribute<T>(inherit));
        }

        /// <summary>
        /// Checks if an attribute of type <paramref name="attrType"/> or any of it's descendants 
        /// is applied to the element.
        /// </summary>
        /// <param name="attrType">The type of the attribute to be checked for.</param>
        /// <returns><c>true</c> if a matching attribute is applied; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attrType"/> is a null reference.</exception>
        public bool HasAttributeOrAncestor(Type attrType)
        {
            return HasAttributeOrAncestor(attrType, true);
        }

        /// <summary>
        /// Checks if an attribute of type <paramref name="attrType"/> or any of it's descendants 
        /// is applied to the element.
        /// </summary>
        /// <param name="attrType">The type of the attribute to be checked for.</param>
        /// <returns><c>true</c> if a matching attribute is applied; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attrType"/> is a null reference.</exception>
        public bool HasAttributeOrAncestor(Type attrType, bool inherit)
        {
            if (attrType == null) throw new ArgumentNullException("attrType");
            AttributeManager mgr = this;
            do
            {
                if (mgr.HasAttributeOrAncestorNonInherited(attrType)) return true;
                mgr = mgr.parent;
            } while (inherit && mgr != null);
            return false;
        }

        /// <summary>
        /// Checks if an attribute of type <typeparamref name="T"/> or any of it's descendants
        /// is applied to the element.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to be checked for.</typeparam>
        /// <returns><c>true</c> if a matching attribute is applied; otherwise, <c>false</c>.</returns>
        public bool HasAttributeOrAncestor<T>() where T : Attribute
        {
            return HasAttributeOrAncestor<T>(true);
        }

        /// <summary>
        /// Checks if an attribute of type <typeparamref name="T"/> or any of it's descendants
        /// is applied to the element.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to be checked for.</typeparam>
        /// <returns><c>true</c> if a matching attribute is applied; otherwise, <c>false</c>.</returns>
        public bool HasAttributeOrAncestor<T>(bool inherit) where T : Attribute
        {
            AttributeManager mgr = this;
            do
            {
                if (mgr.HasAttributeOrAncestorNonInherited<T>()) return true;
                mgr = mgr.parent;
            } while (inherit && mgr != null);
            return false;
        }

        /// <summary>
        /// Retrieves the attribute of type <paramref name="attrType"/> applied to the element.
        /// </summary>
        /// <param name="attrType">The type of the attribute to be retrieved.</param>
        /// <returns>The attribute or a null reference. Note that when multiple matching attributes 
        /// are applied to the element, the first one is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attrType"/> is a null reference.</exception>
        public Attribute GetAttribute(Type attrType)
        {
            return GetAttribute(attrType, true);
        }

        /// <summary>
        /// Retrieves the attribute of type <paramref name="attrType"/> applied to the element.
        /// </summary>
        /// <param name="attrType">The type of the attribute to be retrieved.</param>
        /// <returns>The attribute or a null reference. Note that when multiple matching attributes 
        /// are applied to the element, the first one is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attrType"/> is a null reference.</exception>
        public Attribute GetAttribute(Type attrType, bool inherit)
        {
            return GetFirstAttributeNonInherited(attrType) ?? (!inherit || parent == null ? null : parent.GetAttribute(attrType));
        }

        /// <summary>
        /// Retrieves the attribute of type <typeparamref name="T"/> applied to the element.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to be retrieved.</typeparam>
        /// <returns>The attribute or a null reference. Note that when multiple matching attributes 
        /// are applied to the element, the first one is returned.</returns>
        public T GetAttribute<T>() where T : Attribute
        {
            return GetAttribute<T>(true);
        }

        /// <summary>
        /// Retrieves the attribute of type <typeparamref name="T"/> applied to the element.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to be retrieved.</typeparam>
        /// <returns>The attribute or a null reference. Note that when multiple matching attributes 
        /// are applied to the element, the first one is returned.</returns>
        public T GetAttribute<T>(bool inherit) where T : Attribute
        {
            return GetFirstAttributeNonInherited<T>() ?? (!inherit || parent == null ? null : parent.GetAttribute<T>());
        }

        /// <summary>
        /// Retrieves all attributes of type <paramref name="attrType"/> applied to the element.
        /// </summary>
        /// <param name="attrType">The type of the attribute to be retrieved.</param>
        /// <returns>An array of attributes. Note that when no matching attributes are applied to 
        /// the element, the array returned is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attrType"/> is a null reference.</exception>
        public IEnumerable<Attribute> GetAttributes(Type attrType)
        {
            return GetAttributes(attrType, true);
        }

        /// <summary>
        /// Retrieves all attributes of type <paramref name="attrType"/> applied to the element.
        /// </summary>
        /// <param name="attrType">The type of the attribute to be retrieved.</param>
        /// <returns>An array of attributes. Note that when no matching attributes are applied to 
        /// the element, the array returned is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attrType"/> is a null reference.</exception>
        public IEnumerable<Attribute> GetAttributes(Type attrType, bool inherit)
        {
            if (attrType == null) throw new ArgumentNullException("attrType");
            if (!inherit)
            {
                return GetAttributesNonInherited(attrType);
            }
            else
            {
                var result = new List<Attribute>();
                AttributeManager mgr = this;
                while (mgr != null)
                {
                    result.AddRange(mgr.GetAttributesNonInherited(attrType));
                    mgr = mgr.parent;
                }
                return result;
            }
        }

        /// <summary>
        /// Retrieves all attributes of type <typeparamref name="T"/> applied to the element.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to be retrieved.</typeparam>
        /// <returns>An array of attributes. Note that when no matching attributes are applied to 
        /// the element, the array returned is empty.</returns>
        public IEnumerable<T> GetAttributes<T>() where T : Attribute
        {
            return GetAttributes<T>(true);
        }

        /// <summary>
        /// Retrieves all attributes of type <typeparamref name="T"/> applied to the element.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to be retrieved.</typeparam>
        /// <returns>An array of attributes. Note that when no matching attributes are applied to 
        /// the element, the array returned is empty.</returns>
        public IEnumerable<T> GetAttributes<T>(bool inherit) where T : Attribute
        {
            if (!inherit)
            {
                return GetAttributesNonInherited<T>();
            }
            else
            {
                var result = new List<T>();
                AttributeManager mgr = this;
                while (mgr != null)
                {
                    result.AddRange(mgr.GetAttributesNonInherited<T>());
                    mgr = mgr.parent;
                }
                return result;
            }
        }

        /// <summary>
        /// Retrieves all attributes of type <paramref name="attrType"/> or any of it's descendants 
        /// applied to the element.
        /// </summary>
        /// <param name="attrType">The type of the attribute to be retrieved.</param>
        /// <returns>An array of attributes. Note that when no matching attributes are applied to 
        /// the element, the array returned is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attrType"/> is a null reference.</exception>
        public IList<Attribute> GetAttributesAndDescendants(Type attrType)
        {
            return GetAttributesAndDescendants(attrType, true);
        }

        /// <summary>
        /// Retrieves all attributes of type <paramref name="attrType"/> or any of it's descendants 
        /// applied to the element.
        /// </summary>
        /// <param name="attrType">The type of the attribute to be retrieved.</param>
        /// <returns>An array of attributes. Note that when no matching attributes are applied to 
        /// the element, the array returned is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attrType"/> is a null reference.</exception>
        public IList<Attribute> GetAttributesAndDescendants(Type attrType, bool inherit)
        {
            if (attrType == null) throw new ArgumentNullException("attrType");
            var result = new List<Attribute>();
            AttributeManager mgr = this;
            do
            {
                mgr.GetAttributesAndDescendantsNonInherited(attrType, result);
                mgr = mgr.parent;
            } while (inherit && mgr != null);
            return result;
        }

        /// <summary>
        /// Retrieves all attributes of type <typeparamref name="T"/> or any of it's descendants 
        /// applied to the element.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to be retrieved.</typeparam>
        /// <returns>An array of attributes. Note that when no matching attributes are applied to 
        /// the element, the array returned is empty.</returns>
        public IList<T> GetAttributesAndDescendants<T>() where T : Attribute
        {
            return GetAttributesAndDescendants<T>(true);
        }

        /// <summary>
        /// Retrieves all attributes of type <typeparamref name="T"/> or any of it's descendants 
        /// applied to the element.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to be retrieved.</typeparam>
        /// <returns>An array of attributes. Note that when no matching attributes are applied to 
        /// the element, the array returned is empty.</returns>
        public IList<T> GetAttributesAndDescendants<T>(bool inherit) where T : Attribute
        {
            var result = new List<T>();
            AttributeManager mgr = this;
            do
            {
                mgr.GetAttributesAndDescendantsNonInherited(result);
                mgr = mgr.parent;
            } while (inherit && mgr != null);
            return result;
        }

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// An empty array of <see cref="Attribute"/>s.
        /// </summary>
        private static readonly Attribute[] empty = new Attribute[0];

        /// <summary>
        /// The attributes (including dynamic) applied to the element.
        /// </summary>
        // TODO Synchronize access to this field.
        private readonly Dictionary<Type, Attribute[]> attributes = new Dictionary<Type, Attribute[]>();

        /// <summary>
        /// The element's parent's attribute manager.
        /// </summary>
        private readonly AttributeManager parent;

        /// <summary>
        /// Implements the core logic of the <see cref="HasAttributeOrAncestor(Type,bool)"/> method.
        /// </summary>
        private bool HasAttributeOrAncestorNonInherited(Type attrType)
        {
            foreach (Type key in attributes.Keys)
            {
                if (Types.IsDescendantOrEqual(key, attrType) && attributes[key].Length > 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Implements the core logic of the <see cref="HasAttributeOrAncestor{T}(bool)"/> method.
        /// </summary>
        private bool HasAttributeOrAncestorNonInherited<T>()
        {
            foreach (Type key in attributes.Keys)
            {
                if (Types.IsDescendantOrEqual(key, typeof (T)) && attributes[key].Length > 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Implements the core logic of the <see cref="GetAttribute(Type,bool)"/> method.
        /// </summary>
        private Attribute GetFirstAttributeNonInherited(Type attrType)
        {
            Attribute[] attrs;
            if (attributes.TryGetValue(attrType, out attrs) && attrs.Length > 0)
                return attrs[0];
            else
                return null;
        }

        /// <summary>
        /// Implements the core logic of the <see cref="GetAttribute{T}(bool)<>"/> method.
        /// </summary>
        private T GetFirstAttributeNonInherited<T>() where T : Attribute
        {
            Attribute[] attrs;
            if (attributes.TryGetValue(typeof (T), out attrs) && attrs.Length > 0)
                return (T) attrs[0];
            else
                return null;
        }

        /// <summary>
        /// Implements the core logic of the <see cref="GetAttributes(Type,bool)"/> method.
        /// </summary>
        private IEnumerable<Attribute> GetAttributesNonInherited(Type attrType)
        {
            return attributes.ContainsKey(attrType) ? attributes[attrType] : empty;
        }

        /// <summary>
        /// Implements the core logic of the <see cref="GetAttributes{T}(bool)<>"/> method.
        /// </summary>
        private IEnumerable<T> GetAttributesNonInherited<T>() where T : Attribute
        {
            return (attributes.ContainsKey(typeof (T)) ? attributes[typeof (T)] : empty).Cast<T>();
        }

        /// <summary>
        /// Implements the core logic of the <see cref="GetAttributesAndDescendants(Type,bool)"/> method.
        /// </summary>
        private void GetAttributesAndDescendantsNonInherited(Type attrType, List<Attribute> result)
        {
            foreach (Type key in attributes.Keys)
            {
                if (Types.IsDescendantOrEqual(key, attrType)) result.AddRange(attributes[key]);
            }
        }

        /// <summary>
        /// Implements the core logic of the <see cref="GetAttributesAndDescendants{T}(bool)<>"/> method.
        /// </summary>
        private void GetAttributesAndDescendantsNonInherited<T>(List<T> result)
        {
            foreach (Type key in attributes.Keys)
            {
                if (Types.IsDescendantOrEqual(key, typeof (T))) result.AddRange(attributes[key].Cast<T>());
            }
        }

        #endregion

        #region ------ Constructors ---------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="element">The element whose attributes will be managed by this instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is a null reference.</exception>
        public AttributeManager(Assembly element)
        {
            if (element == null) throw new ArgumentNullException("element");
            parent = null;
            AddAttributes(Attribute.GetCustomAttributes(element, false));
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="element">The element whose attributes will be managed by this instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is a null reference.</exception>
        public AttributeManager(MemberInfo element)
        {
            if (element == null) throw new ArgumentNullException("element");
            // TODO Property inheritance via class inheritance.
            // TODO Method inheritance via class inheritance.
            if (element is Type && ((Type) element).BaseType != null && ((Type) element).BaseType != typeof (object))
                parent = Singleton<AttributeManagerRegistry>.Instance.Get(((Type) element).BaseType);
            else
                parent = null;
            AddAttributes(Attribute.GetCustomAttributes(element, false));
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="element">The element whose attributes will be managed by this instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is a null reference.</exception>
        public AttributeManager(Module element)
        {
            if (element == null) throw new ArgumentNullException("element");
            parent = null;
            AddAttributes(Attribute.GetCustomAttributes(element, false));
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="element">The element whose attributes will be managed by this instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is a null reference.</exception>
        public AttributeManager(ParameterInfo element)
        {
            if (element == null) throw new ArgumentNullException("element");
            // TODO Paremeter inheritance via method inheritance.
            parent = null;
            AddAttributes(Attribute.GetCustomAttributes(element, false));
        }

        #endregion
    }
}