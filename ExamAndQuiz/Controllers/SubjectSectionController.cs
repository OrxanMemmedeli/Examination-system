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
    public class SubjectSectionController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            var subjectSections = db.SubjectSections.Include(s => s.Subject);
            return View(subjectSections.ToList());
        }


        public ActionResult Create()
        {
            ViewBag.FennID = new SelectList(db.Subjects, "ID", "Fenn");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Movzu,FennID")] SubjectSection subjectSection)
        {
            if (ModelState.IsValid)
            {
                db.SubjectSections.Add(subjectSection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FennID = new SelectList(db.Subjects, "ID", "Fenn", subjectSection.FennID);
            return View(subjectSection);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectSection subjectSection = db.SubjectSections.Find(id);
            if (subjectSection == null)
            {
                return HttpNotFound();
            }
            ViewBag.FennID = new SelectList(db.Subjects, "ID", "Fenn", subjectSection.FennID);
            return View(subjectSection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Movzu,FennID")] SubjectSection subjectSection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjectSection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FennID = new SelectList(db.Subjects, "ID", "Fenn", subjectSection.FennID);
            return View(subjectSection);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectSection subjectSection = db.SubjectSections.Find(id);
            if (subjectSection == null)
            {
                return HttpNotFound();
            }
            return View(subjectSection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubjectSection subjectSection = db.SubjectSections.Find(id);
            db.SubjectSections.Remove(subjectSection);
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
