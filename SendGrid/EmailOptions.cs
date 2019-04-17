using SendGrid.Helpers.Mail;

namespace SendGridLib
{
    /// <summary>
    /// EmailSender默认配置
    /// </summary>
    public class EmailOptions
    {
		public string SendGridKey { get; set; } = "【你的Sndgrid ApiKey】";        
        public EmailAddress SendGridEmailFrom { get; set; } = new EmailAddress("admin@localhost.com", "admin");//默认发件人与发件人显示名称
    }
}
