using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendGridLib
{
    /// <summary>
    /// IEmailSender扩展
    /// </summary>
    public interface IEmailSenderExtension:IEmailSender
    {
        Task SendMultiEmailAsync(List<string> emails, string subject, string htmlMessage);
    }
    /// <summary>
    /// EmailSender
    /// </summary>
    public class EmailSender : IEmailSenderExtension
    {
        public EmailSender(IOptions<EmailOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public EmailOptions Options { get; }

        /// <summary>
        /// 邮件发送：单个收件人
        /// </summary>
        /// <param name="email">收件人邮箱</param>
        /// <param name="subject">主题</param>
        /// <param name="message">内容</param>
        /// <returns></returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(Options.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = Options.SendGridEmailFrom,
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            var resp = client.SendEmailAsync(msg);
            return resp;
        }

        /// <summary>
        /// 邮件发送：多个收件人
        /// </summary>
        /// <param name="emails">收件人邮箱列表</param>
        /// <param name="subject">主题</param>
        /// <param name="message">内容</param>
        /// <returns></returns>
        public Task SendMultiEmailAsync(List<string> emails, string subject, string message)
        {
            var client = new SendGridClient(Options.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = Options.SendGridEmailFrom,
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTos(emails.Select(email => { return new EmailAddress(email); }).ToList());
            var resp = client.SendEmailAsync(msg);
            return resp;
        }
    }
}
