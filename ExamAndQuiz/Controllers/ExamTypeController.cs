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
    public class ExamTypeController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            return View(db.ExamTypes.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Adi")] ExamType examType)
        {
            if (ModelState.IsValid)
            {
                db.ExamTypes.Add(examType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(examType);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamType examType = db.ExamTypes.Find(id);
            if (examType == null)
            {
                return HttpNotFound();
            }
            return View(examType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Adi")] ExamType examType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(examType);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamType examType = db.ExamTypes.Find(id);
            if (examType == null)
            {
                return HttpNotFound();
            }
            return View(examType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamType examType = db.ExamTypes.Find(id);
            db.ExamTypes.Remove(examType);
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
