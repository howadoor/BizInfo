using System;

namespace Perenis.Core.CommandLine
{
    /// <summary>
    /// Marks a property that represents a command-line argument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CmdArgumentAttribute : Attribute, IComparable<CmdArgumentAttribute>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="name">The name of the command line argument.</param>
        public CmdArgumentAttribute(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if ((name != null && String.IsNullOrEmpty(name))) throw new ArgumentException("name");

            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public CmdArgumentAttribute()
        {
        }

        /// <summary>
        /// The name of the command line argument.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// An optional short-name of the command line argument.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Indicates this is an anonymous argument (i.e. both <see cref="Name"/>, and 
        /// <see cref="ShortName"/> are null references).
        /// </summary>
        public bool IsAnonymous
        {
            get { return String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(ShortName); }
        }

        /// <summary>
        /// The suitable display name of this argument (the first non-empty string of <see cref="Name"/>, 
        /// or <see cref="ShortName"/>, or <see cref="Value"/>, or an empty string).
        /// </summary>
        public string DisplayName
        {
            get { return Name ?? ShortName ?? Value ?? String.Empty; }
        }

        /// <summary>
        /// Diaplay name and short name combined for help usage
        /// </summary>
        private string CombinedName
        {
            get { return String.IsNullOrEmpty(ShortName) ? DisplayName : String.Format("{0}|{1}", DisplayName, ShortName); }
        }

        /// <summary>
        /// The name of the argument's value.
        /// </summary>
        // TODO Add suport for localization
        public string Value { get; set; }

        /// <summary>
        /// An optional choice group indication.
        /// </summary>
        public string ChoiceGroup { get; set; }

        /// <summary>
        /// The description of the command line argument.
        /// </summary>
        // TODO Add suport for localization
        public string Description { get; set; }

        /// <summary>
        /// Indicates this is an optinal argument.
        /// </summary>
        public bool Optional { get; set; }

        #region IComparable<CmdArgumentAttribute> Members

        public int CompareTo(CmdArgumentAttribute other)
        {
            int rv = Optional.CompareTo(other.Optional);
            if (rv != 0)
            {
                //order by optional parameter first
                return rv;
            }
            else
            {
//order by diaplay name than
                return DisplayName.CompareTo(other.DisplayName);
            }
        }

        #endregion

        /// <summary>
        /// Argument visualization
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string rv;
            if (IsAnonymous)
            {
                rv = String.Format(Optional ? "[ <{0}> ]" : "  <{0}>  ", Value ?? "value");
            }
            else
            {
                rv = String.Format(Optional ? "[ -{0} <{1}> ]" : "  -{0} <{1}>  ", CombinedName, Value ?? "value");
            }

            return String.Format("{0} {1}", rv.PadRight(22), Description);
        }
    }
}