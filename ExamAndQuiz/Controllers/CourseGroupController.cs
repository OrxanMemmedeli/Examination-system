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
    public class CourseGroupController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            return View(db.CourseGroups.OrderBy(o => o.Qrup).ToList());
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Qrup")] CourseGroup courseGroup)
        {
            if (ModelState.IsValid)
            {
                db.CourseGroups.Add(courseGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(courseGroup);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseGroup courseGroup = db.CourseGroups.Find(id);
            if (courseGroup == null)
            {
                return HttpNotFound();
            }
            return View(courseGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Qrup")] CourseGroup courseGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(courseGroup);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseGroup courseGroup = db.CourseGroups.Find(id);
            if (courseGroup == null)
            {
                return HttpNotFound();
            }
            return View(courseGroup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseGroup courseGroup = db.CourseGroups.Find(id);
            db.CourseGroups.Remove(courseGroup);
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
