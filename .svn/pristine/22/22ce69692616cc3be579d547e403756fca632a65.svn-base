using System;
using System.Collections.Generic;

namespace Perenis.Core.CommandLine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CmdOperationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="name">The name of the command line argument.</param>
        public CmdOperationAttribute(string name) : this()
        {
            if (name == null) throw new ArgumentNullException("name");
            if ((name != null && String.IsNullOrEmpty(name))) throw new ArgumentException("name");

            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public CmdOperationAttribute()
        {
            Arguments = new Dictionary<string, CmdArgumentAttribute>();
        }

        /// <summary>
        /// The name of the command line operation.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The description of the command line argument.
        /// </summary>
        // TODO Add suport for localization
        public string Description { get; set; }

        /// <summary>
        /// Message that is shown when the operation exedcution started
        /// </summary>
        public string StartMessage { get; set; }

        /// <summary>
        /// List of assotiated command arguments
        /// </summary>
        public Dictionary<string, CmdArgumentAttribute> Arguments { get; private set; }

        /// <summary>
        /// Indicates this is an anonymous operation (i.e. <see cref="Name"/> is a null reference).
        /// </summary>
        public bool IsAnonymous
        {
            get { return Name == null; }
        }

        /// <summary>
        /// Operation visualization
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("  {0} {1}", Name.PadRight(18), Description);
        }
    }
}