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
    public class ExamParameterController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            var examParameters = db.ExamParameters.Include(e => e.ExamTypeParameter).Include(e => e.Subject);
            return View(examParameters.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.TipParametrID = new SelectList(db.ExamTypeParameters, "ID", "Ad");
            ViewBag.FenninID = new SelectList(db.Subjects, "ID", "Fenn");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TipParametrID,Sira,FenninID,Bal,UmumiSualSayi,AciqSualSayi,AciqSuallar,SehvBal,AciqSehvBal")] ExamParameter examParameter)
        {
            if (ModelState.IsValid)
            {
                db.ExamParameters.Add(examParameter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipParametrID = new SelectList(db.ExamTypeParameters, "ID", "Ad", examParameter.TipParametrID);
            ViewBag.FenninID = new SelectList(db.Subjects, "ID", "Fenn", examParameter.FenninID);
            return View(examParameter);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamParameter examParameter = db.ExamParameters.Find(id);
            if (examParameter == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipParametrID = new SelectList(db.ExamTypeParameters, "ID", "Ad", examParameter.TipParametrID);
            ViewBag.FenninID = new SelectList(db.Subjects, "ID", "Fenn", examParameter.FenninID);
            return View(examParameter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TipParametrID,Sira,FenninID,Bal,UmumiSualSayi,AciqSualSayi,AciqSuallar,SehvBal,AciqSehvBal")] ExamParameter examParameter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examParameter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipParametrID = new SelectList(db.ExamTypeParameters, "ID", "Ad", examParameter.TipParametrID);
            ViewBag.FenninID = new SelectList(db.Subjects, "ID", "Fenn", examParameter.FenninID);
            return View(examParameter);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamParameter examParameter = db.ExamParameters.Find(id);
            if (examParameter == null)
            {
                return HttpNotFound();
            }
            return View(examParameter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamParameter examParameter = db.ExamParameters.Find(id);
            db.ExamParameters.Remove(examParameter);
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
