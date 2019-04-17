# SendGridDemo
AspNet Core 2.2 SendGrid邮件发送(可群发)

开发环境：ASPNet Core 2.2
步骤：
1，前往SendGrid官网，申请账号。有免费账号，30天无限制，30天后仍可使用，做测试足够。
2，新建类库
2，安装包：PM> Install-Package SendGrid
3，实现：
a)，邮件默认配置

    /// <summary>
    /// EmailSender默认配置
    /// </summary>
    public class EmailOptions
    {
        public string SendGridKey { get; set; } = "你申请到的apikey";        
        public EmailAddress SendGridEmailFrom { get; set; } = new EmailAddress("admin@localhost.com", "admin");//邮件默认发送人与显示名称
    }
b)，邮件单发与群发

    /// <summary>
    /// IEmailSender扩展
    /// </summary>
    public interface IEmailSenderExtension:IEmailSender
    {
        Task SendMultiEmailAsync(List<string> emails, string subject, string htmlMessage);
    }
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
4，新建web应用，测试邮件发送
关键点：
Startup中注册

services.AddTransient<SendGridLib.IEmailSenderExtension, SendGridLib.EmailSender>();
5，Home控制器中测试代码：
测试发现 不支持QQ邮箱。
有时收邮件会有延迟。

    public class HomeController : Controller
    {
        private readonly IEmailSenderExtension _emailSender;
        public HomeController(IEmailSenderExtension emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            await _emailSender.SendEmailAsync("xx1@163.com", "主题aaa", "单邮件测试");
            await _emailSender.SendMultiEmailAsync(new List<string>() { "xx1@163.com",  "xx2@gmail.com" }, "主题aaa多人", "多邮件测试");
            return View();
        }
    }
