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
    public class ExamTypeParameterController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            var examTypeParameters = db.ExamTypeParameters.Include(e => e.ExamType);
            return View(examTypeParameters.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.TipID = new SelectList(db.ExamTypes, "ID", "Adi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Ad,FennSayi,TipID")] ExamTypeParameter examTypeParameter)
        {
            if (ModelState.IsValid)
            {
                db.ExamTypeParameters.Add(examTypeParameter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipID = new SelectList(db.ExamTypes, "ID", "Adi", examTypeParameter.TipID);
            return View(examTypeParameter);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamTypeParameter examTypeParameter = db.ExamTypeParameters.Find(id);
            if (examTypeParameter == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipID = new SelectList(db.ExamTypes, "ID", "Adi", examTypeParameter.TipID);
            return View(examTypeParameter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ad,FennSayi,TipID")] ExamTypeParameter examTypeParameter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examTypeParameter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipID = new SelectList(db.ExamTypes, "ID", "Adi", examTypeParameter.TipID);
            return View(examTypeParameter);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamTypeParameter examTypeParameter = db.ExamTypeParameters.Find(id);
            if (examTypeParameter == null)
            {
                return HttpNotFound();
            }
            return View(examTypeParameter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamTypeParameter examTypeParameter = db.ExamTypeParameters.Find(id);
            db.ExamTypeParameters.Remove(examTypeParameter);
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
