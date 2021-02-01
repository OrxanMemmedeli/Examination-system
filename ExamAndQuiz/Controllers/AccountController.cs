using ExamAndQuiz.Models;
using ExamAndQuiz.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ExamAndQuiz.Controllers
{

    public class AccountController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            user.MyMail = SqlInjection.Protect(user.MyMail);
            user.Password = SqlInjection.Protect(user.Password);

            string md5Password = CreateMD5.ConvertMD5("Khan" + CreateMD5.ConvertMD5(user.Password));
            string studentPass = Encryption.PassEncrypt(user.Password);
            var person = db.Students.FirstOrDefault(x => x.Email == user.MyMail && x.Parol == studentPass);
            var admin = db.Admins.FirstOrDefault(x => x.Email == user.MyMail && x.Parol == md5Password);
            if (person != null && admin == null)
            {
                FormsAuthentication.SetAuthCookie(person.Email, false);
                Session["UserName"] = (person.SVNomresi + "-" + person.Ad + " " + person.Soyad).ToString();
                Session["SagirdKodu"] = person.SagirdKodu;
                Session["ID"] = person.ID;
                return RedirectToAction("Index", "Home");
            }
            if (admin != null && person == null)
            {
                FormsAuthentication.SetAuthCookie(admin.Email, false);
                Session["UserName"] = (admin.Ad + " " + admin.Soyad + " - Admin").ToString();
                Session["ID"] = admin.ID;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(user);
            }
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(User user, string seriya)
        {
            seriya = SqlInjection.Protect(seriya);
            user.Password = SqlInjection.Protect(user.Password);
            var student = db.Students.SingleOrDefault(x => x.Email == user.MyMail && x.SVNomresi.Trim() == seriya.Trim());
            if (student != null)
            {
                string studentPass = Encryption.PassDecrypt(student.Parol);

                MailMessage message = new MailMessage();
                message.To.Add(student.Email);
                message.From = new MailAddress("khanexam.tdvhilal@gmail.com", "khanexam.tdvhilal@gmail.com");
                message.IsBodyHtml = true;
                message.Subject = "Unudulmuş Parolun Bərpası";
                message.Body = "<p>Qeyd edilmiş E-mail adresi ilə sistemə giriş cəhti edildi. Əgər parolun bərpası tələbini siz etməmisinizsə, kurs əməktaşları ilə əlaqə saxlayaraq şifrənizi yeniləməyiniz tövsiyyə olunur.&nbsp;<br /><br />Sizin parolunuz : &nbsp;</p>" + studentPass;
                SmtpClient client = new SmtpClient();
                client.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("khanexam.tdvhilal@gmail.com", "1996Orxan1123");
                try
                {
                    client.Send(message);
                    TempData["Message"] = "Məlumat göndərildi";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Bilinməyən bir xəta səbəbi ilə məlumat göndərilmədi. Zəhmət olmasa kurs əməktaşları ilə əlaqə saxlayın.";
                }

                return View();


            }
            else
            {
                ViewBag.Error = "Daxil edilmiş məlumatların doğruluğunu yoxlayın.";
                return View();
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();            
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }


    }
}