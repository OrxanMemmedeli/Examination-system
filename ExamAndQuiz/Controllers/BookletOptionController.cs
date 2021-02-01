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
    public class BookletOptionController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            return View(db.BookletOptions.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Variant")] BookletOption bookletOption)
        {
            if (ModelState.IsValid)
            {
                db.BookletOptions.Add(bookletOption);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookletOption);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookletOption bookletOption = db.BookletOptions.Find(id);
            if (bookletOption == null)
            {
                return HttpNotFound();
            }
            return View(bookletOption);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Variant")] BookletOption bookletOption)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookletOption).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookletOption);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookletOption bookletOption = db.BookletOptions.Find(id);
            if (bookletOption == null)
            {
                return HttpNotFound();
            }
            return View(bookletOption);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookletOption bookletOption = db.BookletOptions.Find(id);
            db.BookletOptions.Remove(bookletOption);
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
