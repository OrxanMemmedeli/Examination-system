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
    public class ExamRuleController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            return View(db.ExamRules.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Qayda,Sira,Cap,Elan")] ExamRule examRule)
        {
            if (ModelState.IsValid)
            {
                db.ExamRules.Add(examRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(examRule);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamRule examRule = db.ExamRules.Find(id);
            if (examRule == null)
            {
                return HttpNotFound();
            }
            return View(examRule);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Qayda,Sira,Cap,Elan")] ExamRule examRule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examRule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(examRule);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamRule examRule = db.ExamRules.Find(id);
            if (examRule == null)
            {
                return HttpNotFound();
            }
            return View(examRule);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamRule examRule = db.ExamRules.Find(id);
            db.ExamRules.Remove(examRule);
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
