using System;
using System.Collections.Generic;
using System.Reflection;
using Perenis.Core.Pattern;
using Perenis.Core.Reflection;

namespace Perenis.Core.CommandLine
{
    /// <summary>
    /// Processes operations.
    /// </summary>
    internal class OperationProcessor
    {
        /// <summary>
        /// List of all arguments.
        /// </summary>
        private readonly List<ArgumentProcessor> allArgs = new List<ArgumentProcessor>();

        /// <summary>
        /// Anonymous arguments of the operation.
        /// </summary>
        private readonly List<ArgumentProcessor> anonymousArgs = new List<ArgumentProcessor>();

        /// <summary>
        /// Named arguments of the operation.
        /// </summary>
        private readonly Dictionary<string, ArgumentProcessor> namedArgs = new Dictionary<string, ArgumentProcessor>();

        /// <summary>
        /// An index into the <see cref="anonymousArgs"/> list indicating current parsing position.
        /// </summary>
        private int anonymousIndex;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="processor">The command-line processor superior to this operation processor.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operation">A pre-created operation or a null reference.</param>
        public OperationProcessor(CommandLineProcessor processor, Type operationType, ICmdOperation operation)
        {
            OperationType = operationType;
            Processor = processor;

            // use an existing or create a new operation instance
            // TODO Refactor into an operation factory
            Operation = operation ?? TypeFactory<ICmdOperation>.Instance.CreateInstance(operationType);

            // create argument processors
            PropertyInfo[] allProps = Operation.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo prop in allProps)
            {
                var attr = Singleton<AttributeManagerRegistry>.Instance.Get(prop).GetAttribute<CmdArgumentAttribute>();
                if (attr != null)
                {
                    var arg = new ArgumentProcessor(this, prop, attr);
                    allArgs.Add(arg);
                    if (attr.IsAnonymous)
                        anonymousArgs.Add(arg);
                    else
                    {
                        namedArgs.Add(processor.CanonizeName(attr.Name), arg);
                        if (!String.IsNullOrEmpty(attr.ShortName)) namedArgs.Add(processor.CanonizeName(attr.ShortName), arg);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="processor">The command-line processor superior to this operation processor.</param>
        /// <param name="operationType">The type of the operation.</param>
        public OperationProcessor(CommandLineProcessor processor, Type operationType)
            : this(processor, operationType, null)
        {
        }

        /// <summary>
        /// The command-line processor superior to this operation processor.
        /// </summary>
        public CommandLineProcessor Processor { get; private set; }

        /// <summary>
        /// The operation created by this processor instance.
        /// </summary>
        public ICmdOperation Operation { get; private set; }

        /// <summary>
        /// The type of the operation to be created by this processor instance.
        /// </summary>
        public Type OperationType { get; private set; }

        /// <summary>
        /// Parses arguments of the operation.
        /// </summary>
        /// <param name="parser">The parser providing tokens.</param>
        /// <returns><c>true</c> when the parsing succeeded; otherwise, <c>false</c>.</returns>
        public bool Parse(ICommandLineParser parser)
        {
            // parse all arguments
            while (parser.MoreTokens() && !Processor.IsOperation(parser))
            {
                if (parser.IsArgument())
                {
                    // parse named argument
                    string name = Processor.CanonizeName(parser.NextArgument());
                    if (!namedArgs.ContainsKey(name))
                    {
                        Console.WriteLine("Invalid argument: argument {0} not known", parser.LastToken());
                        return false;
                    }
                    ArgumentProcessor arg = namedArgs[name];
                    if (!arg.Parse(parser)) return false;
                }
                else
                {
                    // parse anonymous argument
                    if (anonymousIndex >= anonymousArgs.Count)
                    {
                        Console.WriteLine("Invalid argument: argument {0} not known", parser.LastToken());
                        return false;
                    }
                    ArgumentProcessor arg = anonymousArgs[anonymousIndex++];
                    if (!arg.Parse(parser)) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validates that all required arguments have been specified and performs 
        /// operation-specific validation.
        /// </summary>
        /// <returns><c>true</c> when the resulting operation is valid; otherwise, <c>false</c>.</returns>
        public bool Validate()
        {
            // make sure all required arguments have been parsed
            bool error = false;
            foreach (ArgumentProcessor arg in allArgs)
            {
                if (!arg.IsProcessed && !arg.ArgumentAttr.Optional)
                {
                    Console.WriteLine("Required argument {0} not specified", arg.ArgumentAttr.DisplayName);
                    error = true;
                }
            }
            if (error) return false;
            if (Operation is IValidableCmdOperation)
            {
                // validate the operation
                return (Operation as IValidableCmdOperation).ValidateOperation();
            }
            else return true;
        }
    }
}