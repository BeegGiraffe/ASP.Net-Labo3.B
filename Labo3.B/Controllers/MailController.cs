using Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoviesDBManager.Models;

namespace MoviesDBManager.Controllers
{
    public class MailController : Controller
    {
        // GET: Accounts/GroupEmail
        public ActionResult GroupEmail()
        {
            return View();
        }

        // POST: Accounts/GroupEmail
        [HttpPost]
        public ActionResult GroupEmail(MailFormView mail)
        {
            string toName = mail.ToName;
            string toEmail = mail.ToEmail;
            string subject = mail.Subject;
            string body = mail.Body;
            SMTP.SendEmail(toName, toEmail, subject, body);

            return RedirectToAction("GroupEmail", new { status = "Message envoyé aux usagers sélectionnés avec succès" });
        }
    }
}