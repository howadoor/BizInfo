using System;
using System.Collections.Generic;
using System.Reflection;
using Perenis.Core.Pattern;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Maintains a registry of attribute manager instances for various elements.
    /// </summary>
    /// <remarks>
    /// Attribute manager registered by this class are created on a first-accessed basis. Once
    /// an attribute manager instance is created for a specific element, it is guaranteed to be
    /// returned in all subsequent retrievals for the same element.
    /// </remarks>
    public class AttributeManagerRegistry : Singleton<AttributeManagerRegistry>
    {
        /// <summary>
        /// The attribute managers created so far.
        /// </summary>
        // TODO Synchronize access to this field.
        private readonly Dictionary<object, AttributeManager> attributeManagers = new Dictionary<object, AttributeManager>();

        /// <summary>
        /// Synchronization root of this instance.
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// Gets the attribute manager instance for the given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A type.</typeparam>
        /// <returns>The attribute manager for the given type. Multiple calls to this method for the 
        /// same type are guaranteed to return the same attribute manager instance.</returns>
        public AttributeManager Get<T>()
        {
            return Get(typeof (T));
        }

        /// <summary>
        /// Gets the attribute manager instance for the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">An element.</param>
        /// <returns>The attribute manager for the given element. Multiple calls to this method for 
        /// the same element are guaranteed to return the same attribute manager instance.</returns>
        public AttributeManager Get(Assembly element)
        {
            lock (syncRoot)
            {
                AttributeManager attrMgr;
                if (!attributeManagers.TryGetValue(element, out attrMgr))
                {
                    var mgrAttr = (AttributeManagerAttribute) Attribute.GetCustomAttribute(element, typeof (AttributeManagerAttribute), true);
                    Type mgrType = mgrAttr != null ? mgrAttr.AttributeManagerType : typeof (AttributeManager);
                    attrMgr = TypeFactory<AttributeManager>.Instance.CreateInstance(mgrType, element);
                    attributeManagers[element] = attrMgr;
                }
                return attrMgr;
            }
        }

        /// <summary>
        /// Gets the attribute manager instance for the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">An element.</param>
        /// <returns>The attribute manager for the given element. Multiple calls to this method for 
        /// the same element are guaranteed to return the same attribute manager instance.</returns>
        public AttributeManager Get(MemberInfo element)
        {
            lock (syncRoot)
            {
                AttributeManager attrMgr;
                if (!attributeManagers.TryGetValue(element, out attrMgr))
                {
                    var mgrAttr = (AttributeManagerAttribute) Attribute.GetCustomAttribute(element, typeof (AttributeManagerAttribute), true);
                    Type mgrType = mgrAttr != null ? mgrAttr.AttributeManagerType : typeof (AttributeManager);
                    attrMgr = TypeFactory<AttributeManager>.Instance.CreateInstance(mgrType, element);
                    attributeManagers[element] = attrMgr;
                }
                return attrMgr;
            }
        }

        /// <summary>
        /// Gets the attribute manager instance for the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">An element.</param>
        /// <returns>The attribute manager for the given element. Multiple calls to this method for 
        /// the same element are guaranteed to return the same attribute manager instance.</returns>
        public AttributeManager Get(Module element)
        {
            lock (syncRoot)
            {
                AttributeManager attrMgr;
                if (!attributeManagers.TryGetValue(element, out attrMgr))
                {
                    var mgrAttr = (AttributeManagerAttribute) Attribute.GetCustomAttribute(element, typeof (AttributeManagerAttribute), true);
                    Type mgrType = mgrAttr != null ? mgrAttr.AttributeManagerType : typeof (AttributeManager);
                    attrMgr = TypeFactory<AttributeManager>.Instance.CreateInstance(mgrType, element);
                    attributeManagers[element] = attrMgr;
                }
                return attrMgr;
            }
        }

        /// <summary>
        /// Gets the attribute manager instance for the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">An element.</param>
        /// <returns>The attribute manager for the given element. Multiple calls to this method for 
        /// the same element are guaranteed to return the same attribute manager instance.</returns>
        public AttributeManager Get(ParameterInfo element)
        {
            lock (syncRoot)
            {
                AttributeManager attrMgr;
                if (!attributeManagers.TryGetValue(element, out attrMgr))
                {
                    var mgrAttr = (AttributeManagerAttribute) Attribute.GetCustomAttribute(element, typeof (AttributeManagerAttribute), true);
                    Type mgrType = mgrAttr != null ? mgrAttr.AttributeManagerType : typeof (AttributeManager);
                    attrMgr = TypeFactory<AttributeManager>.Instance.CreateInstance(mgrType, element);
                    attributeManagers[element] = attrMgr;
                }
                return attrMgr;
            }
        }
    }
}