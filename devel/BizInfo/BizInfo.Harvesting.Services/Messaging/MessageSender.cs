using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using BizInfo.Model.Entities;

namespace BizInfo.Harvesting.Services.Messaging
{
    /// <summary>
    /// Sends a message to the tenant about collection of <see cref="Info"/>
    /// </summary>
    public class MailMessageSender : IMessageSender
    {
        public void SendInfo(Tenant tenant, IEnumerable<Info> info)
        {
            using (var message = CreateMailMessage(tenant, info))
            {
                var client = GetSmtpClient();
                client.Send(message);
            }
        }

        /// <summary>
        /// Returns SMTP client used to send messages
        /// </summary>
        /// <returns></returns>
        private SmtpClient GetSmtpClient()
        {
            var client = new SmtpClient("smtp.gmail.com", 587) {EnableSsl = true, UseDefaultCredentials = false, Credentials = new NetworkCredential("leonis.bizinfo@gmail.com", "bizinfo331")};
            return client;
        }

        /// <summary>
        /// Creates mail message about <see cref="Info"/> for <see cref="tenant"/>
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private MailMessage CreateMailMessage(Tenant tenant, IEnumerable<Info> info)
        {
            var message = new MailMessage(GetMailAddressOfSender(tenant), GetMailAddressOfRecipient(tenant));
            message.Subject = MessageCreator.CreateSubject(tenant, info);
            CreateMailMessageBody(tenant, info, message);
            return message;
        }

        /// <summary>
        /// Creates body of the mail message and sets all appropriate members of it
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="info"></param>
        /// <param name="message"></param>
        private void CreateMailMessageBody(Tenant tenant, IEnumerable<Info> info, MailMessage message)
        {
            message.Body = CreateMessageHtmlBody(tenant, info);
            // all messages are send as HTML now
            message.IsBodyHtml = true;
        }

        /// <summary>
        /// Creates HTML body of the mail message about <see cref="info"/> for <see cref="tenant"/>
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private string CreateMessageHtmlBody(Tenant tenant, IEnumerable<Info> info)
        {
            return MessageCreator.CreateMessage(tenant, info);
        }

        public IMessageCreator MessageCreator
        {
            get; set;
        }

        /// <summary>
        /// Returns a mail address of recipient for <see cref="tenant"/>
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        private MailAddress GetMailAddressOfRecipient(Tenant tenant)
        {
            return new MailAddress(tenant.Mail);
        }

        /// <summary>
        /// Returns sender mail address
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        private MailAddress GetMailAddressOfSender(Tenant tenant)
        {
            // Password is "bizinfo331"
            return new MailAddress("leonis.bizinfo@gmail.com", "Leonis mailer");
        }
    }
}