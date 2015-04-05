using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using Perenis.Core.Reflection;

namespace Perenis.Core.Resources
{
    /// <summary>
    /// Binds a specific resource to an element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Implementors shall inherit from this class or one of it's predefined descendants to express
    /// actual semantics of the resource being described by the attribute and provide attribute usage 
    /// information (i.e. an <see cref="AttributeUsageAttribute"/> attribute).
    /// </para>
    /// <para>
    /// Note that when a <see cref="ResourceAttribute"/> descendant is applied to an element using
    /// a static attribute declaration, only the <see cref="ResourceAttribute(Type, string)"/> constructor
    /// may be used due to parameter value restrictions. In this case, the actual instance of the
    /// resource manager is determined from the <c>ResourceManager</c> static property of the supplied
    /// type.
    /// </para>
    /// </remarks>
    public abstract class ResourceAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="resourceManager">The resource manager holding the resource.</param>
        /// <param name="resourceID">The ID of the resource.</param>
        /// <exception cref="ArgumentNullException"><paramref name="resourceManager"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resourceID"/> is a null reference.</exception>
        protected ResourceAttribute(ResourceManager resourceManager, string resourceID)
        {
            if (resourceManager == null) throw new ArgumentNullException("resourceManager");
            if (resourceID == null) throw new ArgumentNullException("resourceID");
            ResourceManager = resourceManager;
            ResourceID = resourceID;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="resourceManagerProvider">The type providing a resource manager instance holding the resource.</param>
        /// <param name="resourceID">The ID of the resource.</param>
        /// <remarks>
        /// The <paramref name="resourceManagerProvider"/> must declare a static property <c>ResourceManager</c>
        /// returning an instance of the <see cref="System.Resources.ResourceManager"/> class.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="resourceManagerProvider"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resourceID"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="resourceManagerProvider"/> does not declare the
        /// required <c>ResourceManager</c> property.</exception>
        protected ResourceAttribute(Type resourceManagerProvider, string resourceID)
        {
            if (resourceManagerProvider == null) throw new ArgumentNullException("resourceManagerProvider");
            if (resourceID == null) throw new ArgumentNullException("resourceID");
            PropertyInfo prop = resourceManagerProvider.GetProperty("ResourceManager", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (prop == null) throw new ArgumentException(String.Format("The type {0} does not declare a ResourceManager property of type {1}", resourceManagerProvider.FullName, typeof (ResourceManager).FullName), "resourceManagerProvider");
            ResourceManager = (ResourceManager) prop.GetValueUnbox(null, null);
            ResourceID = resourceID;
            Debug.Assert(ResourceManager.GetObject(ResourceID) != null, String.Format("Resource with ID “{0}” not found in “{1}”", ResourceID, resourceManagerProvider.FullName));
        }

        /// <summary>
        /// The resource manager holding the resource.
        /// </summary>
        public ResourceManager ResourceManager { get; set; }

        /// <summary>
        /// The ID of the resource.
        /// </summary>
        public string ResourceID { get; set; }
    }

    #region ------ Unit tests of the IObjectLogicOperation interface -------------------------------------------

#if NUNIT

    /// <summary>
    /// Common tools for <see cref="ResourceAttribute"/> application unit tests.
    /// </summary>
    public class ResourceAttribute_TestBase
    {
        protected bool CheckResourceAttribute<T>(Type type, string resourceIdSuffix, bool mustHave) where T : ResourceAttribute
        {
            return CheckResourceAttribute<T>(type, resourceIdSuffix, mustHave, null);
        }

        protected bool CheckResourceAttribute<T>(Type type, string resourceIdSuffix, bool mustHave, Func<string, string> typeNameModified) where T : ResourceAttribute
        {
            bool result = false;
            T attr = null;

            // manually traverse the inheritance hierarchy to pair inherited attribute with correct type
            Type typeWithAttr = type;
            while (typeWithAttr != null && typeWithAttr != typeof(object))
            {
                attr = AttributeManagerRegistry.Instance.Get(type).GetAttribute<T>(false);
                if (attr != null) break;
                typeWithAttr = typeWithAttr.BaseType;
            }

            if (attr == null)
            {
                if (mustHave)
                {
                    Console.WriteLine("Missing required attribute {0} on type {1}", typeof(T).FullName, type.FullName);
                    result = true;
                }
            }
            else
            {
                // check value existance
                object value = attr.ResourceManager.GetObject(attr.ResourceID);
                if (value == null)
                {
                    Console.WriteLine("Missing resource for attribute {0} on type {1}", typeof(T).FullName, type.FullName);
                    result = true;
                }

                // check resource name format
                string resourceID = (typeNameModified != null ? typeNameModified(type.Name) : type.Name) + resourceIdSuffix;
                if (!attr.ResourceID.Equals(resourceID))
                {
                    Console.WriteLine("Misspelled ResourceID of attribute {0} on type {1}; found {2}, expected {3}", typeof(T).FullName, type.FullName, attr.ResourceID, resourceID);
                    result = true;
                }
            }
            return result;
        }
    }

#endif

    #endregion
}