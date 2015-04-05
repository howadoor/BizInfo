using System.Collections.Generic;
using BizInfo.Model.Entities;

namespace BizInfo.Harvesting.Services.Messaging
{
    /// <summary>
    /// Sends message to the BizInfo tenant
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Sends message to the <see cref="tenant"/> about <see cref="info"/>
        /// </summary>
        /// <param name="tenant">Recipient of the message</param>
        /// <param name="info">Informations to be send to <see cref="tenant"/></param>
        void SendInfo(Tenant tenant, IEnumerable<Info> info);
    }
}