using System;
using System.Collections.Generic;
using System.Reflection;
using Perenis.Core.Pattern;
using Perenis.Core.Reflection;

namespace Perenis.Core.CommandLine
{
    /// <summary>
    /// A command-line processor: parses a given command-line into a series of <see cref="Operations"/>
    /// and executes them.
    /// </summary>
    public class CommandLineProcessor : TypeRegistry<ICmdOperation>, ICommandLineProcessor
    {
        /// <summary>
        /// Storage for OperationTypes property
        /// </summary>
        private readonly Dictionary<string, Type> operationTypes = new Dictionary<string, Type>();

        private IRootCmdOperation rootOperation;

        public CommandLineProcessor()
        {
        }

        public CommandLineProcessor(IList<Type> typesToRegister) : this()
        {
            RegisterTypes(typesToRegister);
        }

        /// <summary>
        /// Indicates if case-sensitive processing of arguments is necessary; by default,
        /// case-insensitive processing is employed.
        /// </summary>
        private bool CaseSensitive { get; set; }

        /// <summary>
        /// The root command-line operation.
        /// </summary>
        /// <remarks>
        /// The value of this property can be set (only once) to a pre-created instance.
        /// </remarks>
        public IRootCmdOperation RootOperation
        {
            get { return rootOperation; }
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (AnonymousOperationType == null) throw new InvalidOperationException();
                if (!value.GetType().Equals(AnonymousOperationType)) throw new ArgumentException(String.Format("Anonymous operation instance of type {0} expected", AnonymousOperationType.FullName));

                rootOperation = value;
                RootInitialization();
            }
        }

        /// <summary>
        /// The type of a possible anonymous command-line operation.
        /// </summary>
        public Type AnonymousOperationType { get; private set; }

        #region ICommandLineProcessor Members

        public bool Parse(string[] args)
        {
            return Parse(new CommandLineParser(args));
        }

        public void ExecuteOperations()
        {
            RootOperation.ExecuteOperations();
        }

        #endregion

        /// <summary>
        /// Initializes the root command object properties
        /// </summary>
        private void RootInitialization()
        {
            // fills the collection of known operations
            foreach (string cmdName in operationTypes.Keys)
            {
                Type operationType = operationTypes[cmdName];
                if (!rootOperation.KnownOperationTypes.ContainsKey(operationType))
                {
                    CmdOperationAttribute attribute = GetCmdOperationAttribute(cmdName);
                    rootOperation.KnownOperationTypes.Add(operationType, attribute);
                }
            }
        }

        /// <summary>
        /// Retrieves the <see cref="CmdOperationAttribute"/> instance associated with the operation 
        /// with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of an operation or a null reference to indicate the anonymous operation.</param>
        /// <returns>The <see cref="CmdOperationAttribute"/> instance associated with the given operation.</returns>
        /// <exception cref="ArgumentException">No such operation has been registered.</exception>
        private CmdOperationAttribute GetCmdOperationAttribute(string name)
        {
            Type operation = GetOperationByName(name);
            var attribute = Singleton<AttributeManagerRegistry>.Instance.Get(operation).GetAttribute<CmdOperationAttribute>();

            // initializes the Arguments collection of command
            PropertyInfo[] allProps = operation.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo prop in allProps)
            {
                var attr = Singleton<AttributeManagerRegistry>.Instance.Get(prop).GetAttribute<CmdArgumentAttribute>();
                if (attr != null) attribute.Arguments.Add(attr.Name, attr);
            }

            return attribute;
        }

        /// <summary>
        /// Parses a command-line provided by the given <paramref name="parser"/> and creates a series 
        /// of <see cref="Operations"/> and optionally an <see cref="AnonymousOperation"/>.
        /// </summary>
        /// <param name="parser">The parser providing tokens.</param>
        /// <returns><c>true</c> when the parsing succeeded; otherwise, <c>false</c>.</returns>
        public bool Parse(ICommandLineParser parser)
        {
            OperationProcessor anonymousOperationProcessor = null;

            while (parser.MoreTokens())
            {
                // parse a named operation
                if (IsOperation(parser))
                {
                    if (!ParseOperation(parser)) return false;
                }
                else
                {
                    // make sure an anonymous operation is defined
                    if (AnonymousOperationType == null)
                    {
                        Console.WriteLine("Invalid argument: {0}", parser.NextToken());
                        return false;
                    }
                    if (anonymousOperationProcessor == null)
                    {
                        anonymousOperationProcessor = new OperationProcessor(this, AnonymousOperationType, rootOperation);
                    }

                    // parse as an anonymous operation
                    if (!anonymousOperationProcessor.Parse(parser)) return false;
                }
            }

            // validate the anonymous operation (if any)
            if (anonymousOperationProcessor != null)
            {
                if (!anonymousOperationProcessor.Validate()) return false;
            }

            return true;
        }

        #region ------ Internals: TypeRegistry overrides ------------------------------------------

        protected override bool IsRegisterable(Type type)
        {
            return base.IsRegisterable(type) && Attribute.IsDefined(type, typeof (CmdOperationAttribute));
        }

        protected override void OnTypeRegistered(Type type)
        {
            var attr = (CmdOperationAttribute) Attribute.GetCustomAttribute(type, typeof (CmdOperationAttribute));
            if (attr.IsAnonymous)
            {
                if (AnonymousOperationType != null)
                    throw new InvalidOperationException(String.Format("Multiple anonymous command-line operations found; {0} encountered, while {1} already known", type.FullName, AnonymousOperationType.FullName));
                AnonymousOperationType = type;
            }
            else
            {
                operationTypes.Add(CanonizeName(attr.Name), type);
            }
        }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Indicates if the current token represents a start of an operation.
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal bool IsOperation(ICommandLineParser parser)
        {
            return operationTypes.Count > 0 && parser.IsOperation();
        }

        /// <summary>
        /// Retrieves an operation type by its <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of an operation or a null reference to indicate the anonymous operation.</param>
        /// <returns>The type of the given operation.</returns>
        /// <exception cref="ArgumentException">No such operation has been registered.</exception>
        private Type GetOperationByName(string name)
        {
            if (name != null && !operationTypes.ContainsKey(name)) throw new ArgumentException(String.Format("Unknown operation {0}", name), "name");
            if (name == null && AnonymousOperationType == null) throw new ArgumentException("Anonymous operation not defined", "name");
            return name != null ? operationTypes[name] : AnonymousOperationType;
        }

        /// <summary>
        /// Converts the given <paramref name="name"/> into its canonical representation.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal string CanonizeName(string name)
        {
            return CaseSensitive ? name : name.ToLowerInvariant();
        }

        /// <summary>
        /// Parses an operation and all it's arguments.
        /// </summary>
        /// <param name="parser">The parser providing tokens.</param>
        /// <returns><c>true</c> when the parsing succeeded; otherwise, <c>false</c>.</returns>
        private bool ParseOperation(ICommandLineParser parser)
        {
            // find the operation's registered type
            string name = CanonizeName(parser.NextOperation());
            if (!operationTypes.ContainsKey(name))
            {
                return ParseUnknownOperation(parser, name);
            }

            // create operation instance
            var op = new OperationProcessor(this, operationTypes[name]);

            // adds the operation to the operation list
            RootOperation.InnerOperations.Add(op.Operation);

            // parse the operation
            if (!op.Parse(parser)) return false;

            // validate that all required arguments have been processed
            if (!op.Validate()) return false;

            return true;
        }

        /// <summary>
        /// Try to parse unknown operation. Unknown operation is operation which canonized name is not contained as a key in <see cref="operationTypes"/>.
        /// </summary>
        /// <param name="parser">The parser providing tokens.</param>
        /// <param name="canonizedOperationName">Canonized name of unknown operation.</param>
        /// <returns><c>true</c> when the parsing succeeded; otherwise, <c>false</c>.</returns>
        protected virtual bool ParseUnknownOperation(ICommandLineParser parser, string canonizedOperationName)
        {
            Console.WriteLine("Unknown operation: {0}", parser.LastToken());
            return false;
        }

        #endregion
    }
}