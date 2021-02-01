using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExamAndQuiz.Models;
using ExamAndQuiz.Tools;

namespace ExamAndQuiz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExamController : Controller
    {

        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            var exams = db.Exams.Include(e => e.Admin).Include(e => e.ExamTypeParameter).Include(e => e.Grade);
            return View(exams.ToList());
        }


        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.Admins, "ID", "Ad");
            ViewBag.TipParametrID = new SelectList(db.ExamTypeParameters, "ID", "Ad");
            ViewBag.SinifID = new SelectList(db.Grades, "ID", "ID");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exam exam, HttpPostedFileBase file, HttpPostedFileBase PDF)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/UploadFoto/Exam/"), Path.GetFileName(DateTime.Now.ToString("ddMMyy") + CodeGenerator.Code() + file.FileName));

                    string[] massiv = path.Split('\\');
                    exam.FotoURL = "~/UploadFoto/Exam/" + massiv[massiv.Length - 1];
                    file.SaveAs(path);
                }
                else
                {
                    exam.FotoURL = "~/UploadFoto/Exam/test.jpg";
                }
                if (PDF != null && PDF.ContentLength > 0)
                {
                    string pathPDF = Path.Combine(Server.MapPath("~/UploadPDF/"), Path.GetFileName(DateTime.Now.ToString("ddMMyy") + CodeGenerator.Code() + PDF.FileName));

                    string[] massivPDf = pathPDF.Split('\\');
                    exam.PDFFileUrl = "~/UploadPDF/" + massivPDf[massivPDf.Length - 1];
                    PDF.SaveAs(pathPDF);
                }

                db.Exams.Add(exam);
                db.SaveChanges();

                /*Imtahan Suallari Start*/
                ExamQuestion questions = new ExamQuestion();
                var fennler = db.ExamParameters.Where(x => x.TipParametrID == exam.TipParametrID).OrderBy(x => x.Sira).ToList();
                int sualsira = 1;
                foreach (var item in fennler)
                {
                    for (int i = 0; i < item.UmumiSualSayi; i++)
                    {
                        questions.ImtahanId = exam.ID;
                        questions.FennId = item.FenninID;
                        questions.TipId = exam.TipParametrID;
                        questions.SualSirasi = sualsira;
                        questions.SualMetni = null;
                        questions.Cavablar = null;
                        questions.YaranmaTarixi = DateTime.Now;
                        if (item.AciqSualSayi != 0)
                        {
                            string[] massiv = item.AciqSuallar.Split(',');
                            if (massiv.Contains(sualsira.ToString()))
                            {
                                var Tipid = db.QuestionTypes.FirstOrDefault(x => x.Tip == "Açıq");
                                questions.SualTipi = Tipid.ID;
                            }
                            else
                            {
                                var Tipid = db.QuestionTypes.FirstOrDefault(x => x.Tip == "Qapalı");
                                questions.SualTipi = Tipid.ID;
                            }
                        }
                        else
                        {
                            var Tipid = db.QuestionTypes.FirstOrDefault(x => x.Tip == "Qapalı");
                            questions.SualTipi = Tipid.ID;
                        }
                        db.ExamQuestions.Add(questions);
                        db.SaveChanges();
                        sualsira++;

                    }
                }
                /*Imtahan Suallari End*/


                return RedirectToAction("Index");
            }

            ViewBag.AdminID = new SelectList(db.Admins, "ID", "Ad", exam.AdminID);
            ViewBag.TipParametrID = new SelectList(db.ExamTypeParameters, "ID", "Ad", exam.TipParametrID);
            ViewBag.SinifID = new SelectList(db.Grades, "ID", "ID", exam.SinifID);
            return View(exam);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.Admins, "ID", "Ad", exam.AdminID);
            ViewBag.TipParametrID = new SelectList(db.ExamTypeParameters, "ID", "Ad", exam.TipParametrID);
            ViewBag.SinifID = new SelectList(db.Grades, "ID", "ID", exam.SinifID);
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Exam exam, HttpPostedFileBase file, HttpPostedFileBase PDF)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/UploadFoto/Exam/"), Path.GetFileName(DateTime.Now.ToString("ddMMyy") + CodeGenerator.Code() + file.FileName));

                    string[] massiv = path.Split('\\');
                    exam.FotoURL = "~/UploadFoto/Exam/" + massiv[massiv.Length - 1];
                    file.SaveAs(path);
                }

                if (PDF != null && PDF.ContentLength > 0)
                {
                    string pathPDF = Path.Combine(Server.MapPath("~/UploadPDF/"), Path.GetFileName(DateTime.Now.ToString("ddMMyy") + CodeGenerator.Code() + PDF.FileName));

                    string[] massivPDf = pathPDF.Split('\\');
                    exam.PDFFileUrl = "~/UploadPDF/" + massivPDf[massivPDf.Length - 1];
                    PDF.SaveAs(pathPDF);
                }

                db.Entry(exam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminID = new SelectList(db.Admins, "ID", "Ad", exam.AdminID);
            ViewBag.TipParametrID = new SelectList(db.ExamTypeParameters, "ID", "Ad", exam.TipParametrID);
            ViewBag.SinifID = new SelectList(db.Grades, "ID", "ID", exam.SinifID);
            return View(exam);
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
