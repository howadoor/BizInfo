using System;
using System.Collections.Generic;
using Perenis.Core.Collections;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// Parameter handling helper class.
    /// </summary>
    public class ParameterCollection : IndexedList<Parameter, string>
    {
        private static readonly ParameterCollection empty = new ParameterCollection();

        /// <summary>
        /// Set of empty parameters.
        /// </summary>
        public static ParameterCollection Empty
        {
            get { return empty; }
        }

        /// <summary>
        /// Gets the parameter identified by the given <see cref="ParamAttribute"/>.
        /// </summary>
        /// <param name="paramAttr"></param>
        /// <returns></returns>
        public Parameter this[ParamAttribute paramAttr]
        {
            get { return GetParam(paramAttr); }
        }

        /// <summary>
        /// Gets the parameter identified by the given structured parameter name.
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public Parameter this[object paramName]
        {
            get { return GetParam(paramName); }
        }

        /// <summary>
        /// Checks if the collection contains the parameter identified by the given <see cref="ParamAttribute"/>.
        /// </summary>
        /// <param name="paramAttr"></param>
        /// <returns></returns>
        public bool ContainsParam(ParamAttribute paramAttr)
        {
            return itemIndex.ContainsKey(paramAttr.Name);
        }

        /// <summary>
        /// Checks if the collection contains the parameter identified by the given structured parameter name.
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public bool ContainsParam(object paramName)
        {
            return itemIndex.ContainsKey(ParamsBuilder.GetParameterName(paramName));
        }

        /// <summary>
        /// Gets the parameter identified by the given <see cref="ParamAttribute"/>.
        /// </summary>
        /// <param name="paramAttr"></param>
        /// <returns></returns>
        public Parameter GetParam(ParamAttribute paramAttr)
        {
            return itemIndex[paramAttr.Name];
        }

        /// <summary>
        /// Gets the parameter identified by the given structured parameter name.
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public Parameter GetParam(object paramName)
        {
            return itemIndex[ParamsBuilder.GetParameterName(paramName)];
        }

        /// <summary>
        /// Gets the typed value of the parameter identified by the given <see cref="ParamAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the parameter's value.</typeparam>
        /// <param name="paramName">The attribute describing the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        /// <exception cref="InvalidOperationException">When the parameter of the given name wasn't found.</exception>
        /// <exception cref="InvalidOperationException">When the parameter's value is not of type <typeparamref name="T"/>.</exception>
        public T GetParamValue<T>(ParamAttribute paramAttr)
        {
            // TODO Consider using some IncompatibleParamsException instead of InvalidOperationException
            // TODO Type-safety check using the inferred type
            Parameter param;
            if (!TryGetParam(paramAttr, out param)) throw new InvalidOperationException(String.Format("Parameter “{0}” not found", paramAttr.Name));
            if (param.Value != null && !(param.Value is T)) throw new InvalidOperationException(String.Format("Parameter “{0}” has an unexpected type", paramAttr.Name));
            return (T) param.Value;
        }

        /// <summary>
        /// Gets the typed value of the parameter identified by the given structured parameter name.
        /// </summary>
        /// <typeparam name="T">The expected type of the parameter's value.</typeparam>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        /// <exception cref="InvalidOperationException">When the parameter of the given name wasn't found.</exception>
        /// <exception cref="InvalidOperationException">When the parameter's value is not of type <typeparamref name="T"/>.</exception>
        public T GetParamValue<T>(object paramName)
        {
            // TODO Consider using some IncompatibleParamsException instead of InvalidOperationException
            // TODO Type-safety check using the inferred type
            Parameter param;
            if (!TryGetParam(paramName, out param)) throw new InvalidOperationException(String.Format("Parameter {0} not found", ParamsBuilder.GetParameterName(paramName)));
            if (param.Value != null && !(param.Value is T)) throw new InvalidOperationException(String.Format("Parameter {0} has an unexpected type", ParamsBuilder.GetParameterName(paramName)));
            return (T) param.Value;
        }

        /// <summary>
        /// Tries to get the parameter identified by the given <see cref="ParamAttribute"/>.
        /// </summary>
        /// <param name="paramAttr"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool TryGetParam(ParamAttribute paramAttr, out Parameter param)
        {
            return itemIndex.TryGetValue(paramAttr.Name, out param);
        }

        /// <summary>
        /// Tries to get the parameter identified by the given structured parameter name.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool TryGetParam(object paramName, out Parameter param)
        {
            return itemIndex.TryGetValue(ParamsBuilder.GetParameterName(paramName), out param);
        }

        /// <summary>
        /// Adds the given parameter to the collection; replaces existing parameter with the same name.
        /// </summary>
        /// <param name="param"></param>
        public void SetParam(Parameter param)
        {
            Parameter oldParam;
            if (TryGetParam(param.StructuredName, out oldParam))
            {
                int index = IndexOf(oldParam);
                RemoveAt(index);
                Insert(index, param);
            }
            else
            {
                Add(param);
            }
        }

        #region ------ Constructors ---------------------------------------------------------------

        public ParameterCollection()
        {
        }

        public ParameterCollection(IEnumerable<Parameter> items)
            : base(items)
        {
        }

        #endregion

        #region ------ Internals: IndexedList overrides -------------------------------------------

        protected override string GetItemUniqueKey(Parameter item)
        {
            return item.Name;
        }

        #endregion
    }
}