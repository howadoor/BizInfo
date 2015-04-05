using System.Collections.Generic;
using BizInfo.Model.Entities;

namespace BizInfo.Harvesting.Services.Messaging
{
    /// <summary>
    /// Creates a HTML message body of the message informing <see cref="Tenant"/> about collection of new <see cref="Info"/>
    /// </summary>
    public interface IMessageCreator
    {
        /// <summary>
        /// Creates a subject of the message 
        /// </summary>
        /// <param name="tenant">Recipient of the message</param>
        /// <param name="info">Content of the message to be send to <see cref="tenant"/></param>
        /// <returns>String expressing subject of the message</returns>
        string CreateSubject(Tenant tenant, IEnumerable<Info> info);

        /// <summary>
        /// Creates a HTML message body of the message informing <see cref="Tenant"/> about collection of new <see cref="Info"/>
        /// </summary>
        /// <param name="tenant">Recipient of the message</param>
        /// <param name="info">Content of the message to be send to <see cref="tenant"/></param>
        /// <returns>HTML content of the message</returns>
        string CreateMessage(Tenant tenant, IEnumerable<Info> info);
    }
}