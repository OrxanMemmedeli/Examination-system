using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExamAndQuiz.Models;

namespace ExamAndQuiz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExamQuestionController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var examQuestions = db.ExamQuestions.Include(e => e.Exam).Include(e => e.ExamTypeParameter).Include(e => e.Subject).OrderBy(s => s.SualSirasi).Where(x => x.ImtahanId == id);
            ViewBag.ImtahanID = id;
            return View(examQuestions.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion = db.ExamQuestions.Find(id);
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.UmumiSualSayi = db.ExamQuestions.Where(x => x.ImtahanId == examQuestion.ImtahanId).Count();
            return View(examQuestion);
        }

        //public ActionResult Create()
        //{
        //    ViewBag.ImtahanId = new SelectList(db.Exams, "ID", "Ad");
        //    ViewBag.TipId = new SelectList(db.ExamTypeParameters, "ID", "Ad");
        //    ViewBag.FennId = new SelectList(db.Subjects, "ID", "Fenn");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,ImtahanId,FennId,TipId,SualSirasi,SualMetni,Cavablar,YaranmaTarixi")] ExamQuestion examQuestion)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ExamQuestions.Add(examQuestion);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ImtahanId = new SelectList(db.Exams, "ID", "Ad", examQuestion.ImtahanId);
        //    ViewBag.TipId = new SelectList(db.ExamTypeParameters, "ID", "Ad", examQuestion.TipId);
        //    ViewBag.FennId = new SelectList(db.Subjects, "ID", "Fenn", examQuestion.FennId);
        //    return View(examQuestion);
        //}

        // GET: ExamQuestion/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion = db.ExamQuestions.Find(id);
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImtahanId = new SelectList(db.Exams, "ID", "Ad", examQuestion.ImtahanId);
            ViewBag.TipId = new SelectList(db.ExamTypeParameters, "ID", "Ad", examQuestion.TipId);
            ViewBag.FennId = new SelectList(db.Subjects, "ID", "Fenn", examQuestion.FennId);
            return View(examQuestion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExamQuestion examQuestion)
        {
            if (ModelState.IsValid)
            {
                var catchID = db.ExamQuestions.FirstOrDefault(x => x.ID == examQuestion.ID);
                catchID.SualMetni = examQuestion.SualMetni;
                catchID.Cavablar = examQuestion.Cavablar;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = examQuestion.Exam.ID });
            }
            ViewBag.ImtahanId = new SelectList(db.Exams, "ID", "Ad", examQuestion.ImtahanId);
            ViewBag.TipId = new SelectList(db.ExamTypeParameters, "ID", "Ad", examQuestion.TipId);
            ViewBag.FennId = new SelectList(db.Subjects, "ID", "Fenn", examQuestion.FennId);
            return View(examQuestion);
        }

        public ActionResult Clear(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var examQuestion = db.ExamQuestions.FirstOrDefault(x => x.ID == id);
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            examQuestion.SualMetni = null;
            examQuestion.Cavablar = null;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = examQuestion.Exam.ID });
        }

        //public ActionResult AddBank(int? id)
        //{
        //    QuestionBank questionBank = new QuestionBank();
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var examQuestion = db.ExamQuestions.Where(x => x.ImtahanId == id).ToList();
        //    foreach (var item in examQuestion)
        //    {
        //        questionBank.ImtahanID = item.ImtahanId;
        //        questionBank.Sual = item.SualMetni;
        //        questionBank.Cavab = item.Cavablar;
        //        questionBank.Cetinlik = 0;
        //        questionBank.FennID = item.FennId;
        //        questionBank.Status = false;
        //        //questionBank.YaranmaTarixi = item.YaranmaTarixi;
        //        questionBank.TipID = item.TipId;
        //        db.QuestionBanks.Add(questionBank);
        //        db.SaveChanges();
        //    }
        //      return RedirectToAction("Index", new { id = id });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var examQuestion = db.ExamQuestions.Where(x => x.ImtahanId == id);
            db.ExamQuestions.RemoveRange(examQuestion);
            db.SaveChanges();
            var exam = db.Exams.FirstOrDefault(x => x.ID == id);
            db.Exams.Remove(exam);
            db.SaveChanges();
            return RedirectToAction("Index", "Exam");
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
