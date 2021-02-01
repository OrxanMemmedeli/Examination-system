using ExamAndQuiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using ExamAndQuiz.Tools;

namespace ExamAndQuiz.Controllers
{
    public class HomeController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();
        
        public ActionResult Index()
        {
            var exams = db.Exams.Where(x => x.Status == true && x.Imtahan == "Sınaq").ToList();
            return View(exams);
        }

        //public ActionResult Quiz()
        //{
        //    var exams = db.Exams.Where(x => x.Status == true && x.Imtahan == "Quiz").ToList();
        //    return View(exams);
        //}
        
        public ActionResult About()
        {
            var result = db.Abouts.SingleOrDefault();
            return View(result);
        }

        public ActionResult Blog(int? page)
        {
            var blogs = db.Blogs.OrderByDescending(x => x.Tarix).ToList().ToPagedList(page ?? 1, 5);
            return View(blogs);
        }

        public ActionResult ExamVideo()
        {
            ExamVideo examVideo = db.ExamVideos.FirstOrDefault();
            return View(examVideo);
        }

        public ActionResult Exam()
        {
            ExamVideo examVideo = db.ExamVideos.FirstOrDefault();
            return PartialView(examVideo);
        }

        [Authorize]
        public ActionResult EnterExam(string id)
        {
            int newID = Convert.ToInt32(Encryption.Decrypt(id));

            Exam exam = db.Exams.SingleOrDefault(x => x.ID == newID);
            return View(exam);
        }

        [Authorize]
        public ActionResult Identification(string id)
        {
            int newID = Convert.ToInt32(Encryption.Decrypt(id));

            Exam exam = db.Exams.SingleOrDefault(x => x.ID == newID);
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Identification(int imtahanID, string vesiqe, string identifikasiyaKodu)
        {
            vesiqe = SqlInjection.Protect(vesiqe);
            identifikasiyaKodu = SqlInjection.Protect(identifikasiyaKodu);

            Exam exam = db.Exams.SingleOrDefault(x => x.ID == imtahanID);

            var sv = db.Students.FirstOrDefault(x => x.SVNomresi.Trim() == vesiqe.Trim());
            if (sv != null)
            {
                List<StudentAction> result = db.StudentActions.Where(x => x.ImtahanID == imtahanID).ToList();
                if (result != null)
                {
                    var resultIdentity = result.FirstOrDefault(x => x.GirisKodu == identifikasiyaKodu);
                    if (resultIdentity != null)
                    {
                        if (resultIdentity.Status == true)
                        {
                            resultIdentity.Status = false;
                            db.SaveChanges();
                            return RedirectToAction("ExamView", "Home", new { id = Encryption.Encrypt(imtahanID.ToString()), studentID = Encryption.Encrypt(sv.ID.ToString()) });
                        }
                        else
                        {
                            ViewBag.Error = "Daxil edilmiş identifikasiya kodu ilə imtahanda iştirak edilmişdir. Yenidən imtahanda iştirak edə bilməniz üçün yeni kod əldə etməniz lazımdır.";
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Daxil ediləm məlumatlarda uyğunsuzluq baş verdi. Buraxılış vərəqəsində qeyd olunmuş məlumatları doğru daxil etdiyinizdən əmin olun. Əgər buraxılış vərəqiniz yoxdursa, Kurs ilə əlaqə saxlamanız və ya indi öndə hissəsindən ödəniş edərək başlaya bilərsiniz.";
                    }
                }
                else
                {
                    ViewBag.Error = "Daxil ediləm məlumatlarda uyğunsuzluq baş verdi. Buraxılış vərəqəsində qeyd olunmuş məlumatları doğru daxil etdiyinizdən əmin olun. Əgər buraxılış vərəqiniz yoxdursa, Kurs ilə əlaqə saxlamanız və ya indi öndə hissəsindən ödəniş edərək başlaya bilərsiniz.";
                }

            }
            else
            {
                ViewBag.Error = "Daxil edilmiş  vəsiqə nömrəsinə uyğun şagirdin sistemdə qeydiyyatı mövcud deyil. İmtahanda iştirak üçün sistemdə qeydiyyat edilməsi vacibdir.";
            }
            return View(exam);
        }
        
        public ActionResult ExamRules(string id)
        {
            int newID = Convert.ToInt32(Encryption.Decrypt(id));
            ExamAndRules examAndRules = new ExamAndRules();
            examAndRules.MyExam = db.Exams.SingleOrDefault(x => x.ID == newID);
            examAndRules.MyExamRule = db.ExamRules.Where(x => x.Elan == true).OrderBy(s => s.Sira).ToList();
            return View(examAndRules);
        }

        [Authorize]
        public ActionResult ExamView(string id, string studentID)
        {
            int newID = Convert.ToInt32(Encryption.Decrypt(id));
            int newStudentID = Convert.ToInt32(Encryption.Decrypt(studentID));

            ExamAndQuestions examAndQuestions = new ExamAndQuestions();
            examAndQuestions.MyExam = db.Exams.SingleOrDefault(x => x.ID == newID);
            examAndQuestions.MyExamQuestion = db.ExamQuestions.Where(x => x.ImtahanId == newID).OrderBy(x => x.SualSirasi);

            ViewBag.UmumiSualSayi = db.ExamQuestions.Where(x => x.ImtahanId == newID).Count();


            var controlOptions = db.StudentOptions.FirstOrDefault(x => x.Student.ID == newStudentID && x.Exam.ID == newID);
            if (controlOptions != null)
            {
                if (controlOptions.Bitirmek == false)
                {
                    controlOptions.Bitirmek = true;
                    db.SaveChanges();
                    //End();
                    return View(examAndQuestions);
           
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                StudentOption studentOption = new StudentOption();
                studentOption.Bitirmek = true;
                studentOption.ImtahanId = newID;
                studentOption.SagirdID = newStudentID;
                db.StudentOptions.Add(studentOption);
                db.SaveChanges();
                return View(examAndQuestions);
            }



        }

        //int start = 0;
        //DateTime EndTime;
        //private void End()
        //{
        //    if (start == 0)
        //    {
        //        EndTime = (DateTime.Now).AddMinutes(/*examAndQuestions.MyExam.Muddet_deqiqe*/ 0.5);
        //        start++;
        //    }
        //    if (EndTime == DateTime.Now)
        //    {
        //        ViewBag.End = "End";
        //    }
        //}

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Timer(int id)
        {
            var exams = db.Exams.SingleOrDefault(x => x.ID == id);
            return PartialView(exams);
        }

        public ActionResult Questions(int id)
        {
            var questions = db.ExamQuestions.Where(x => x.ImtahanId == id).OrderBy(x => x.SualSirasi).ToList();
            ViewBag.UmumiSualSayi = db.ExamQuestions.Where(x => x.ImtahanId == id).Count();

            return PartialView(questions);
        }


    }
}