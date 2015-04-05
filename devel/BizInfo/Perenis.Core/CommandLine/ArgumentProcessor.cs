using System;
using System.ComponentModel;
using System.Reflection;
using Perenis.Core.Reflection;

namespace Perenis.Core.CommandLine
{
    /// <summary>
    /// Processes arguments
    /// </summary>
    internal class ArgumentProcessor
    {
        // TODO Add support for collections.

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ArgumentProcessor(OperationProcessor processor, PropertyInfo property, CmdArgumentAttribute argumentAttr)
        {
            Processor = processor;
            Property = property;
            ArgumentAttr = argumentAttr;
        }

        /// <summary>
        /// The operation processor superior to this argument processor.
        /// </summary>
        public OperationProcessor Processor { get; private set; }

        /// <summary>
        /// Information about the underlying property.
        /// </summary>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// The defining attribute.
        /// </summary>
        public CmdArgumentAttribute ArgumentAttr { get; private set; }

        /// <summary>
        /// Indicates that this argument has been processed.
        /// </summary>
        public bool IsProcessed { get; private set; }

        /// <summary>
        /// Indicates that this argument can be parsed multiple-times.
        /// </summary>
        private bool IsRepeatable
        {
            get { return Property.PropertyType.IsArray; }
        }

        /// <summary>
        /// Parses an argument.
        /// </summary>
        /// <param name="parser">The parser providing tokens.</param>
        /// <returns><c>true</c> when the parsing succeeded; otherwise, <c>false</c>.</returns>
        public bool Parse(ICommandLineParser parser)
        {
            // get the type of the argument's value 
            Type valueType = Property.PropertyType.IsArray
                                 ? Property.PropertyType.GetElementType()
                                 : Property.PropertyType;

            // parse the value
            object value;
            if (valueType == typeof (bool) && !ArgumentAttr.IsAnonymous)
            {
                value = true;
            }
            else
            {
                if (!parser.MoreTokens())
                {
                    Console.WriteLine(string.Format("Missing <{0}> value for argument -{1}", ArgumentAttr.Value ?? "value", ArgumentAttr.Name));
                    return false;
                }

                try
                {
                    value = TypeDescriptor.GetConverter(valueType).ConvertFromString(parser.NextToken());
                }
                catch
                {
                    Console.WriteLine(string.Format("Value '{0}' of argument -{1} is not in a supported format", parser.LastToken(), ArgumentAttr.Name));
                    return false;
                }
            }


            // set the value of the operation's actual property
            Property.SetValueUnbox(Processor.Operation, value, null);

            IsProcessed = true;
            return true;
        }
    }
}