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
    public class QuestionTypeController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            return View(db.QuestionTypes.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Tip")] QuestionType questionType)
        {
            if (ModelState.IsValid)
            {
                db.QuestionTypes.Add(questionType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(questionType);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionType questionType = db.QuestionTypes.Find(id);
            if (questionType == null)
            {
                return HttpNotFound();
            }
            return View(questionType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Tip")] QuestionType questionType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(questionType);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionType questionType = db.QuestionTypes.Find(id);
            if (questionType == null)
            {
                return HttpNotFound();
            }
            return View(questionType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionType questionType = db.QuestionTypes.Find(id);
            db.QuestionTypes.Remove(questionType);
            db.SaveChanges();
            return RedirectToAction("Index");
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
