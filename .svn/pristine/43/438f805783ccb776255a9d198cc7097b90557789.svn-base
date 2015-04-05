using System;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Provides schemes 
    /// </summary>
    public interface ISchemeProvider
    {
        /// <summary>
        /// Gets a scheme for particular <see cref="Type"/> and <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        IWritingScheme GetScheme(Type type, ISerializer serializer);
    }
}