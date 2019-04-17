using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SendGridLib;
using SedGridApp.Models;

namespace SedGridApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSenderExtension _emailSender;
        public HomeController(IEmailSenderExtension emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            await _emailSender.SendEmailAsync("wanglui1990@163.com", "主题aaa", "单邮件测试");
            await _emailSender.SendMultiEmailAsync(new List<string>() { "wanglui1990@163.com", "874655551@qq.com", "wanglui1990@gmail.com" }, "主题aaa多人", "多邮件测试");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
