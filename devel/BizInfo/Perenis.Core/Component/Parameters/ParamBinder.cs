using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Perenis.Core.Pattern;
using Perenis.Core.Reflection;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// Provides dynamic parameter-binding capabilities for objects with parameters idenfitied 
    /// by the <see cref="ParamAttribute"/>.
    /// </summary>
    public static class ParamBinder
    {
        // TODO Add support for containers exposing the IParametersAware interface

        /// <summary>
        /// Reads output parameters from the given object.
        /// </summary>
        /// <param name="instance">The source object.</param>
        /// <returns>The output </returns>
        /// <remarks>
        /// Note that when a property or field is both marked with the <see cref="ParamAttribute"/>
        /// and the <see cref="ParamContainerAttribute"/>, its value is both passed as a parameter
        /// itself and scanned for nested parameters.
        /// </remarks>
        public static ParameterCollection GetParams(object instance)
        {
            var result = new ParameterCollection();
            GetParamsImpl(instance, result);
            return result;
        }

        /// <summary>
        /// Writes input parameters into the given object.
        /// </summary>
        /// <param name="instance">The destination object.</param>
        /// <param name="parameters">The input parameters.</param>
        /// <returns>Collection of all mandatory parameters not covered by the input <paramref name="parameters"/>.</returns>
        /// <exception cref="InputMissingException">the <paramref name="parameters"/> is missing a parameter 
        /// required by <paramref name="instance"/></exception>
        /// <remarks>
        /// <para>
        /// Note that when a property or field is both marked with the <see cref="ParamAttribute"/>
        /// and the <see cref="ParamContainerAttribute"/>, it is first processed as a parameter container
        /// (when not a null reference), and then as a parameter itself. Thus, this may lead to unwanted 
        /// replacing of the pre-set value with a supplied value. On the other hand, when a such 
        /// a property or field is initialized from a parameter, it won't be processed as a parameter
        /// container afterwards. This is slightly different from the behavior of the <see cref="GetParams"/>
        /// method.
        /// </para>
        /// <para>
        /// When the destination type is not compatible with the source value, the method attempts to find 
        /// a suitable <see cref="TypeConverter"/> instance to convert the source value to the destination type.
        /// The method uses <see cref="TypeConverterUtilsTypeEx.GetTypeConverterToConvertFrom(System.Type,System.Type)"/>
        /// to look up the <see cref="TypeConverter"/> instance.
        /// </para>
        /// </remarks>
        /// TODO Let SetParams throw InputMissingException descendant with list of mandatory input parameters
        public static List<ParamAttribute> SetParams(object instance, ParameterCollection parameters)
        {
            var result = new List<ParamAttribute>();
            SetParamsImpl(instance, parameters, result);
            return result;
        }

        /// <summary>
        /// Merges two sets of parameters by overwriting existing parameters in the <paramref name="parameters"/>
        /// collection and adding new parameters from the <paramref name="newParameters"/> collection.
        /// Parameters from the <paramref name="parameters"/> collection not found in the
        /// <paramref name="newParameters"/> are left intact.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="newParameters"></param>
        public static void SetParams(ParameterCollection parameters, ParameterCollection newParameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            if (newParameters == null) throw new ArgumentNullException("newParameters");
            foreach (Parameter param in newParameters)
            {
                parameters.SetParam(param);
            }
        }

        /// <summary>
        /// Merges two sets of parameters into a new set. In case of conflict, the parameters from the
        /// <paramref name="newParameters"/> collection take precedence over those from the
        /// <paramref name="baseParameters"/> collection.
        /// </summary>
        /// <param name="baseParameters"></param>
        /// <param name="newParameters"></param>
        /// <returns></returns>
        public static ParameterCollection MergeParams(ParameterCollection baseParameters, ParameterCollection newParameters)
        {
            if (baseParameters == null) throw new ArgumentNullException("baseParameters");
            if (newParameters == null) throw new ArgumentNullException("newParameters");
            var result = new ParameterCollection(baseParameters);
            SetParams(result, newParameters);
            return result;
        }

        /// <summary>
        /// Indicates if the given object has at least one non-optional parameter.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="parameterName">The name of the first found non-optional parameter.</param>
        /// <returns></returns>
        public static bool HasMandatoryInputParameters(object instance, out string parameterName)
        {
            parameterName = null;
            if (instance == null) return false;

            // process parameter containers first
            IList<Tuple<PropertyInfo, ParamContainerAttribute>> paramContainerMembers = GetParamContainerMembers(instance);
            foreach (var paramContainerMember in paramContainerMembers)
            {
                object memberContainer = paramContainerMember.Item1.GetValueUnbox(instance, null);
                if (memberContainer is IEnumerable)
                {
                    foreach (object item in (IEnumerable) memberContainer)
                    {
                        if (HasMandatoryInputParameters(item, out parameterName)) return true;
                    }
                }
                else
                {
                    if (HasMandatoryInputParameters(memberContainer, out parameterName)) return true;
                }
            }

            // continue processing attributes
            IList<Tuple<MemberInfo, ParamAttribute>> paramMembers = GetParamMembers(instance, ParamMode.In);
            foreach (var param in paramMembers)
            {
                if (!param.Item2.Optional)
                {
                    parameterName = param.Item2.Name;
                    return true;
                }
            }
            return false;
        }

        #region ----- Internals -------------------------------------------------------------------

        /// <summary>
        /// Reads object parameters from a nested parameter container.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="parameters"></param>
        private static void GetContainerParamsImpl(object container, ParameterCollection parameters)
        {
            if (container is IParametersAware)
            {
                SetParams(parameters, ((IParametersAware) container).GetOutputParameters());
            }
            else
            {
                GetParamsImpl(container, parameters);
            }
        }

        /// <summary>
        /// Reads object parameters.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static void GetParamsImpl(object instance, ParameterCollection parameters)
        {
            if (instance == null) return;
            NotifyRecursive(instance, NotifyOnBeforeGetParameters);
            GetParamsRecursive(instance, parameters);
            NotifyRecursive(instance, NotifyOnAfterGetParameters);
        }

        private static void GetParamsRecursive(object instance, ParameterCollection parameters)
        {
            if (instance == null) return;

            // process parameter containers first
            IList<Tuple<PropertyInfo, ParamContainerAttribute>> paramContainerMembers = GetParamContainerMembers(instance);
            foreach (var paramContainerMember in paramContainerMembers)
            {
                object memberContainer = paramContainerMember.Item1.GetValueUnbox(instance, null);
                if (memberContainer is IEnumerable && !(memberContainer is IParametersAware))
                {
                    foreach (object item in (IEnumerable) memberContainer) GetParamsRecursive(item, parameters);
                }
                else
                {
                    GetParamsRecursive(memberContainer, parameters);
                }
            }

            // continue processing attributes
            IList<Tuple<MemberInfo, ParamAttribute>> paramMembers = GetParamMembers(instance, ParamMode.Out);
            foreach (var paramInfo in paramMembers)
            {
                // if no expression is defined in the attribute, take the member's value itself
                object paramValue = paramInfo.Item1 is PropertyInfo
                                        ? ((PropertyInfo) paramInfo.Item1).GetValueUnbox(instance, null)
                                        : ((FieldInfo) paramInfo.Item1).GetValue(instance);

                // in case there's already the same parameter in the output, the values must match
                Parameter param;
                if (!parameters.TryGetParam(paramInfo.Item2, out param))
                {
                    // add parameter to output
                    parameters.Add(paramInfo.Item2.Instantiate(paramValue));
                }
                else
                {
                    // make sure the values match
                    if (!Equals(paramValue, param.Value))
                    {
                        throw new InvalidOperationException(String.Format("Duplicate output parameter '{0}' found in object of type '{1}'.", paramInfo.Item2.Name, instance.GetType().FullName));
                    }
                }
            }
        }

        /// <summary>
        /// Writes object parameters into a nested parameter container.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="parameters"></param>
        /// <param name="uncoveredParams"></param>
        private static void SetContainerParamsImpl(object container, ParameterCollection parameters, List<ParamAttribute> uncoveredParams)
        {
            if (container is IParametersAware)
            {
                // TODO SetInputParameters won't report uncovered parameters; to be solved by replacing uncoveredParams with MissingParamsException
                ((IParametersAware) container).SetInputParameters(parameters);
            }
            else
            {
                SetParamsImpl(container, parameters, uncoveredParams);
            }
        }

        /// <summary>
        /// Writes parameters into an object.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="parameters"></param>
        /// <param name="uncoveredParams"></param>
        /// <exception cref="InputMissingException">the <paramref name="parameters"/> is missing a parameter required by <paramref name="instance"/></exception>
        private static void SetParamsImpl(object instance, ParameterCollection parameters, List<ParamAttribute> uncoveredParams)
        {
            if (instance == null) return;
            NotifyRecursive(instance, NotifyOnBeforeSetParameters);
            SetParamsRecursive(instance, parameters, uncoveredParams);
            NotifyRecursive(instance, NotifyOnAfterSetParameters);
        }

        private static void SetParamsRecursive(object instance, ParameterCollection parameters, List<ParamAttribute> uncoveredParams)
        {
            if (instance == null) return;

            // process parameter containers first
            IList<Tuple<PropertyInfo, ParamContainerAttribute>> paramContainerMembers = GetParamContainerMembers(instance);
            foreach (var paramContainerMember in paramContainerMembers)
            {
                object memberContainer = paramContainerMember.Item1.GetValueUnbox(instance, null);
                if (memberContainer is IEnumerable && !(memberContainer is IParametersAware))
                {
                    foreach (object item in (IEnumerable) memberContainer) SetParamsRecursive(item, parameters, uncoveredParams);
                }
                else
                {
                    SetParamsRecursive(memberContainer, parameters, uncoveredParams);
                }
            }

            // continue processing attributes
            IList<Tuple<MemberInfo, ParamAttribute>> paramMembers = GetParamMembers(instance, ParamMode.In);
            foreach (var paramInfo in paramMembers)
            {
                Parameter param;
                if (!parameters.TryGetParam(paramInfo.Item2, out param))
                {
                    // report eligible for input and not optional
                    if (IsEligible(paramInfo.Item2, paramInfo.Item1, ParamMode.In) && !paramInfo.Item2.Optional)
                    {
                        uncoveredParams.Add(paramInfo.Item2);
                    }
                }
                else
                {
                    Type targetType = paramInfo.Item1 is PropertyInfo
                                          ? ((PropertyInfo) paramInfo.Item1).PropertyType
                                          : ((FieldInfo) paramInfo.Item1).FieldType;
                    Type sourceType = param.Value != null ? param.Value.GetType() : typeof (Nullable);

                    object value = param.Value;

                    // convert the value if it's not type-compatible
                    if (!targetType.IsAssignableFrom(sourceType))
                    {
                        TypeConverter typeConverter = targetType.GetTypeConverterToConvertFrom(sourceType);
                        if (typeConverter != null)
                        {
                            value = typeConverter.ConvertFrom(value);
                        }
                        else
                        {
                            typeConverter = sourceType.GetTypeConverterToConvertTo(targetType);
                            if (typeConverter != null)
                            {
                                value = typeConverter.ConvertTo(value, targetType);
                            }
                        }
                    }

                    // set the property/field value
                    if (paramInfo.Item1 is PropertyInfo)
                    {
                        ((PropertyInfo) paramInfo.Item1).SetValueUnbox(instance, value);
                    }
                    else
                        ((FieldInfo) paramInfo.Item1).SetValue(instance, value);
                }
            }
        }

        /// <summary>
        /// Helper method that retrieves all suitable instance members that are marked with
        /// <see cref="ParamAttribute"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        private static IList<Tuple<MemberInfo, ParamAttribute>> GetParamMembers(object instance, ParamMode paramMode)
        {
            var result = new List<Tuple<MemberInfo, ParamAttribute>>();

            // only public instance members can be parameters
            Type t = instance.GetType();
            GetParamMebersCore(t.GetProperties(BindingFlags.Public | BindingFlags.Instance), paramMode, result);
            GetParamMebersCore(t.GetFields(BindingFlags.Public | BindingFlags.Instance), paramMode, result);

            // sort the parameters by the declared order of setting / getting
            result.Sort(CompareByParamOrder);
            return result;
        }

        /// <summary>
        /// Implements the core functionality of <see cref="GetParamMembers"/>.
        /// </summary>
        /// <param name="members"></param>
        /// <param name="paramMode"></param>
        /// <param name="result"></param>
        private static void GetParamMebersCore(MemberInfo[] members, ParamMode paramMode, List<Tuple<MemberInfo, ParamAttribute>> result)
        {
            foreach (MemberInfo propInfo in members)
            {
                // they must have the ParamAttribute attached and must be usable either as input or output parameters
                var paramAttr = Singleton<AttributeManagerRegistry>.Instance.Get(propInfo).GetAttribute<ParamAttribute>();
                if (paramAttr != null && IsEligible(paramAttr, propInfo, paramMode))
                {
                    result.Add(new Tuple<MemberInfo, ParamAttribute>(propInfo, paramAttr));
                }
            }
        }

        /// <summary>
        /// Finds whether the <paramref name="parameter"/> is eligible for the given <paramref name="paramMode"/>.
        /// </summary>
        private static bool IsEligible(ParamAttribute paramAttr, MemberInfo parameter, ParamMode paramMode)
        {
            if (!paramAttr.SupportsMode(paramMode)) return false;
            switch (paramMode)
            {
                case ParamMode.In:
                    // input parameters need to have a Set method
                    return !(parameter is PropertyInfo) || ((PropertyInfo) parameter).GetSetMethod(false) != null;
                case ParamMode.Out:
                    // output parameters need to have a Get method
                    return !(parameter is PropertyInfo) || ((PropertyInfo) parameter).GetGetMethod(false) != null;
                default:
                    // for the purposes of this method the Default and InOut modes are not supported.
                    throw new ArgumentOutOfRangeException("paramMode");
            }
        }

        /// <summary>
        /// Helper method to sort <see cref="ParamAttribute"/> instance by their declared order of setting / getting.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private static int CompareByParamOrder(Tuple<MemberInfo, ParamAttribute> first, Tuple<MemberInfo, ParamAttribute> second)
        {
            return first.Item2.CompareTo(second.Item2);
        }

        /// <summary>
        /// Helper method that retrieves all suitable instance members that are marked with
        /// <see cref="ParamAttribute"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        private static IList<Tuple<PropertyInfo, ParamContainerAttribute>> GetParamContainerMembers(object instance)
        {
            IList<Tuple<PropertyInfo, ParamContainerAttribute>> result = new List<Tuple<PropertyInfo, ParamContainerAttribute>>();

            foreach (PropertyInfo propInfo in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attr = Singleton<AttributeManagerRegistry>.Instance.Get(propInfo).GetAttribute<ParamContainerAttribute>();
                if (attr != null) result.Add(new Tuple<PropertyInfo, ParamContainerAttribute>(propInfo, attr));
            }

            return result;
        }

        private static void NotifyRecursive(object instance, Action<IParamBindingAware> notificationHandler)
        {
            if (instance == null) return;
            if (instance is IParamBindingAware) notificationHandler((IParamBindingAware) instance);

            // process parameter containers first
            IList<Tuple<PropertyInfo, ParamContainerAttribute>> paramContainerMembers = GetParamContainerMembers(instance);
            foreach (var paramContainerMember in paramContainerMembers)
            {
                object memberContainer = paramContainerMember.Item1.GetValueUnbox(instance, null);
                if (memberContainer is IEnumerable && !(memberContainer is IParametersAware))
                {
                    foreach (object item in (IEnumerable) memberContainer) NotifyRecursive(item, notificationHandler);
                }
                else
                {
                    NotifyRecursive(memberContainer, notificationHandler);
                }
            }
        }

        private static void NotifyOnBeforeSetParameters(IParamBindingAware instance)
        {
            instance.OnBeforeSetParameters();
        }

        private static void NotifyOnAfterSetParameters(IParamBindingAware instance)
        {
            instance.OnAfterSetParameters();
        }

        private static void NotifyOnBeforeGetParameters(IParamBindingAware instance)
        {
            instance.OnBeforeGetParameters();
        }

        private static void NotifyOnAfterGetParameters(IParamBindingAware instance)
        {
            instance.OnAfterGetParameters();
        }

        #endregion
    }
}