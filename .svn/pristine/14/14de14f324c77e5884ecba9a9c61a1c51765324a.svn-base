using System;

namespace Perenis.Core.CommandLine
{
    public class CommandLineParser : ICommandLineParser
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string[] args;

        private int index;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public CommandLineParser(params string[] args)
        {
            //replace "" with empty array
            if ((args.Length == 1) && (args[0].Length == 0))
            {
                this.args = new string[0];
            }
            else
            {
                this.args = args;
            }
        }

        public virtual string OperationNamePrefix
        {
            get { return String.Empty; }
        }

        public virtual string ArgumentNamePrefix
        {
            get { return "-"; }
        }

        public virtual string ArgumentShortNamePrefix
        {
            get { return "-"; }
        }

        #region ICommandLineParser Members

        public bool MoreTokens()
        {
            return index < args.Length;
        }

        public bool IsOperation()
        {
            if ((OperationNamePrefix != null && String.IsNullOrEmpty(OperationNamePrefix)))
            {
                return !IsArgument();
            }
            else
            {
                return index < args.Length - 1 && args[index] == OperationNamePrefix;
            }
        }

        public bool IsArgument()
        {
            return index < args.Length && (args[index].StartsWith(ArgumentNamePrefix) || args[index].StartsWith(ArgumentShortNamePrefix));
        }

        public string NextOperation()
        {
            if ((OperationNamePrefix != null && String.IsNullOrEmpty(OperationNamePrefix)))
            {
                return NextToken();
            }
            else
            {
                string prefix = NextToken();
                if (!OperationNamePrefix.Equals(prefix))
                    throw new InvalidOperationException(String.Format("Token '{0}' is not an operation name prefix", prefix));
                return NextToken();
            }
        }

        public string NextArgument()
        {
            string arg = NextToken();
            if (arg.StartsWith(ArgumentNamePrefix))
                return arg.Substring(ArgumentNamePrefix.Length);
            if (arg.StartsWith(ArgumentShortNamePrefix))
                return arg.Substring(ArgumentShortNamePrefix.Length);
            throw new InvalidOperationException(String.Format("Token '{0}' is not a valid argument", arg));
        }

        public string LastToken()
        {
            if (index == 0) return String.Empty;
            return index - 1 < args.Length ? args[index - 1] : String.Empty;
        }

        public string NextToken()
        {
            if (!MoreTokens()) throw new InvalidOperationException("No more command-line tokens");
            return args[index++];
        }

        #endregion
    }
}