using ExamAndQuiz.Models;
using ExamAndQuiz.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ExamAndQuiz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BackController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad");
            ViewBag.StudentID = db.Students.ToList();
            return View();
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentAction studentAction, string Student)
        {
            string[] massiv = Student.Split('-');
            string student = massiv[0];
            var control = db.Students.SingleOrDefault(x => x.SVNomresi == student);
            if (control != null)
            {
                var participate = db.StudentActions.SingleOrDefault(x => x.ImtahanID == studentAction.ImtahanID && x.StudentID == control.ID);
                if (participate == null)
                {
                    studentAction.StudentID = control.ID;
                    studentAction.GirisKodu = CodeGenerator.Code();
                    studentAction.Tarix = DateTime.Now;
                    studentAction.Status = true;
                    db.StudentActions.Add(studentAction);
                    db.SaveChanges();

                    SendEmail(control, studentAction.ImtahanID, studentAction.GirisKodu);

                    return RedirectToAction("Details", "StudentAction", new { id = studentAction.ID });
                }
                else
                {
                    ViewBag.Error = "Satış ediləcək ŞAGİRD seçilmiş İMTAHANDA iştirak etmişdir. Şagirdin öncəki 'giriş kodu və imtahan nəticəsini' sildikdən sonra yeni bilet satışı etmək mümkün olacaqdır.";
                }
            }
            else
            {
                ViewBag.Error = "Sistemdə belə vəsiqə nömrəsi yoxdur";
            }


            ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad", studentAction.ImtahanID);
            ViewBag.StudentID = db.Students.ToList();
            return View(studentAction);
        }

        private void SendEmail(Student student, int examID, string identifikasiya)
        {
            var exam = db.Exams.SingleOrDefault(x => x.ID == examID);
            MailMessage message = new MailMessage();
            message.To.Add(student.Email);
            message.From = new MailAddress("khanexam.tdvhilal@gmail.com", "khanexam.tdvhilal@gmail.com");
            message.IsBodyHtml = true;
            message.Subject = exam.Ad + " Adlı imtahana Buraxılış məlumatlarınız";
            message.Body = "<p> <b>Ad : </b>" + student.Ad + "<br /><b>Soyad : </b>" + student.Soyad + "<br /><b>Ata Adı : </b>" + student.AtaAdi + "<br /><b>Vəsiqə seriyası : </b>" + student.SVNomresi + "<br /><b>Şagird Kodu : </b>" + student.SagirdKodu + "<br /><b>E-mail : </b>" + student.Email + "<br /><br /><br /><b>İmtahanın Adı : </b>" + exam.Ad + "<br /><b>İmtahanın Sinifi : </b>" + exam.Grade.Sinif + "<br /><b>Keçirilmə Tarixi və Saatı : </b>" + exam.Tarix.ToShortDateString() + " " + exam.Saat + "<br /><b>İmtahanın Müddəti : </b>" + exam.Muddet_deqiqe + " (dəqiqə) " + "<br /><b>İmtahana Giriş (identifikasiya) Kodu : </b>" + identifikasiya + "</p>";
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
        }

        public ActionResult FreeTicket()
        {
            var exams = db.Exams.ToList();
            return View(exams);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FreeTicket(int imtahanID)
        {
            var exams = db.Exams.ToList();
            var exam = db.Exams.SingleOrDefault(x => x.ID == imtahanID);
            var pulsuzlar = db.FreeTickets.ToList();
            var sagirdler = db.Students.Where(x => x.SinifID == exam.SinifID).ToList();
            StudentAction studentAction = new StudentAction();
            if (sagirdler != null)
            {
                foreach (var item in sagirdler)
                {
                    var biletler = pulsuzlar.SingleOrDefault(x => x.SagirdKodu == item.SagirdKodu);
                    if (biletler != null)
                    {
                        var bilet = db.Students.SingleOrDefault(x => x.SagirdKodu == item.SagirdKodu);
                        studentAction.ImtahanID = imtahanID;
                        studentAction.StudentID = bilet.ID;
                        studentAction.GirisKodu = CodeGenerator.Code();
                        studentAction.Tarix = DateTime.Now;
                        studentAction.Status = true;
                        db.StudentActions.Add(studentAction);
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Message = "Seçilmiş imtahan (" + exam.Ad + ") sinifinə uyğun olan şagirdlərin pulsuz bilet siyahısında adlari yoxdur";
                        return View(exams);
                    }
                }
                return RedirectToAction("FreeTicketPrint", "Back", new { id = imtahanID });
            }
            return View(exams);
        }

        public ActionResult FreeTicketPrint(int id)
        {
            InvoicePrint ınvoicePrint = new InvoicePrint();

            ınvoicePrint.MyStudentActions = db.StudentActions.Where(x => x.ImtahanID == id).ToList();
            ınvoicePrint.MyAbout = db.Abouts.SingleOrDefault();
            ınvoicePrint.MyExamRules = db.ExamRules.Where(x => x.Cap == true).OrderBy(x => x.Sira).ToList();
            return View(ınvoicePrint);
        }
    }
}