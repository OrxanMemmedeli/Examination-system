using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExamAndQuiz.Models;
using ExamAndQuiz.Tools;

namespace ExamAndQuiz.Controllers
{
    public class StudentActionController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var studentActions = db.StudentActions.Include(s => s.Exam).Include(s => s.Student);
            return View(studentActions.ToList());
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            İnvoice invoice = new İnvoice();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice.MyStudentAction = db.StudentActions.Find(id);
            if (invoice.MyStudentAction == null)
            {
                return HttpNotFound();
            }
            invoice.MyAbout = db.Abouts.SingleOrDefault();
            invoice.MyExamRule = db.ExamRules.Where(x => x.Cap == true).OrderBy(x => x.Sira).ToList();
            return View(invoice);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAction studentAction = db.StudentActions.Find(id);
            if (studentAction == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad", studentAction.ImtahanID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "Ad", studentAction.StudentID);
            return View(studentAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(StudentAction studentAction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentAction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad", studentAction.ImtahanID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "Ad", studentAction.StudentID);
            return View(studentAction);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAction studentAction = db.StudentActions.Find(id);
            if (studentAction == null)
            {
                return HttpNotFound();
            }
            return View(studentAction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentAction studentAction = db.StudentActions.Find(id);
            db.StudentActions.Remove(studentAction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public string Answer(string[] answers, int studentID, int examID)
        {
            StudentOption studentOption = new StudentOption();

            List<string> FennUzreNeticeler = new List<string>();
            var examQuestion = db.ExamQuestions.Where(x => x.ImtahanId == examID);
            var dogruCavablar = db.ExamOptions.FirstOrDefault(x => x.ImtahanID == examID);
            var exam = db.Exams.SingleOrDefault(x => x.ID == examID);
            string[] cavablar = dogruCavablar.Cavablar.Split('|');
            int index = 0; double DogruSayi = 0; double SehvSayi = 0; double BosSayi = 0; double Bal = 0; double YekunDuz = 0;
            if (answers != null)
            {
                foreach (var fenn in examQuestion.Select(x => x.Subject.Fenn).Distinct())
                {
                    BosSayi = 0; SehvSayi = 0; DogruSayi = 0; YekunDuz = 0;

                    for (int i = 0; i < examQuestion.Where(x => x.Subject.Fenn == fenn).Count(); i++)
                    {
                        if (cavablar[index].Trim() == "*")
                        {
                            DogruSayi++;
                        }
                        else
                        {
                            if (answers[index].Trim() == "*")
                            {
                                BosSayi++;
                            }
                            else if (answers[index].Trim() == cavablar[index].Trim())
                            {
                                DogruSayi++;
                            }
                            else if (answers[index].Trim() != cavablar[index].Trim())
                            {
                                if ((examQuestion.FirstOrDefault(o => o.SualSirasi == (index + 1))).QuestionType.Tip == "Qapalı")
                                {
                                    SehvSayi++;
                                }
                            }
                        }

                        index++;
                    }
                    var examParametr = db.ExamParameters.Where(k => k.TipParametrID == exam.ExamTypeParameter.ID);
                    var sehvBal = examParametr.FirstOrDefault(x => x.Subject.Fenn == fenn);
                    if (sehvBal.SehvBal == null || sehvBal.SehvBal == 0)
                    {
                        YekunDuz = DogruSayi - (SehvSayi * (1 / 1));
                    }
                    else
                    {
                        YekunDuz = DogruSayi - (SehvSayi * (1 / (sehvBal.SehvBal.GetValueOrDefault())));
                    }

                    FennUzreNeticeler.Add(fenn + " {Doğru-" + DogruSayi + "; Yanlış-" + SehvSayi + "; Boş-" + BosSayi + "} Bal - " + (YekunDuz <= 0 ? 0 : YekunDuz) + " || "); 
                    Bal += YekunDuz * sehvBal.Bal;
                }
            }
            else
            {
                FennUzreNeticeler.Add("Cavablar daxil edilmədən İMTAHANI BİTİR düyməsi sıxılmışdır.");
            }

            var Option = db.StudentOptions.SingleOrDefault(x => x.SagirdID == studentID && x.ImtahanId == examID);
            if (Option == null)
            {
                studentOption.SagirdID = studentID;
                studentOption.ImtahanId = examID;
                studentOption.Bitirmek = true;
                studentOption.Cavablar = String.Join(";", answers);
                studentOption.Bal = Bal;
                studentOption.FennlerUzreNetice = String.Join("", FennUzreNeticeler);
                studentOption.Tarix = DateTime.Now;
                db.StudentOptions.Add(studentOption);
                db.SaveChanges();
            }
            else
            {
                if (answers != null)
                {
                    Option.Cavablar = String.Join("; ", answers);
                }
                else
                {
                    Option.Cavablar = "Cavablar daxil edilmədən İMTAHANI BİTİR düyməsi sıxılmışdır.";
                }
                Option.Bal = Bal;
                Option.FennlerUzreNetice = String.Join("", FennUzreNeticeler);
                Option.Tarix = DateTime.Now;
                db.SaveChanges();
            }
            string url = Url.Action("ExamResult", "StudentOption", new { studentID = Encryption.Encrypt(studentID.ToString()), examID = Encryption.Encrypt(examID.ToString()) });
            return url;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
