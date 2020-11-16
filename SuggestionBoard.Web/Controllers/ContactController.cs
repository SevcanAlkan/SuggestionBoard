using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Data.ViewModel;

namespace SuggestionBoard.Web.Controllers
{
    [AllowAnonymous]
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IConfiguration configuration,
            ILogger<ContactController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactRequestVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("GeneralError", "Invalid form!");
                return View(model);
            }

            try
            {
                IConfigurationSection mailSettings = _configuration.GetSection("MailSettings");
                int.TryParse(mailSettings.GetSection("EMailHostPort").Value, out int hostPort);

                SmtpClient smtpClient = new SmtpClient(mailSettings.GetSection("EMailHost").Value, hostPort);

                smtpClient.Credentials = new System.Net.NetworkCredential(mailSettings.GetSection("EMailAddress").Value, mailSettings.GetSection("EMailPassword").Value);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.From = new MailAddress(mailSettings.GetSection("EMailAddress").Value, "SuggestionBoard - Contact Form");
                mail.To.Add(new MailAddress(mailSettings.GetSection("EMailAddress").Value));
                mail.CC.Add(new MailAddress(model.EMail));
                mail.Subject = $"{model.Name} : { model.Subject}";
                mail.Body = model.Message;

                smtpClient.Send(mail);

                ViewBag.MessageSendSuccess = "Your message succesfully sent!";
            }
            catch (Exception ex)
            {
                _logger.LogError("Contact" ,ex);
            }

            return View(new ContactRequestVM());
        }
    }
}
