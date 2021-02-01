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
    public class ExamResultController : Controller
    {
        //    private OnlineExamEntities db = new OnlineExamEntities();

        //    public ActionResult Index()
        //    {
        //        var examResults = db.ExamResults.ToList();
        //        return View(examResults.ToList());
        //    }

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult IndexResult(int? imtahanID)
        //    {
        //        var examResults = db.ExamResults.Include(e => e.Exam).Include(e => e.Student).Where(x => x.ImtahanID == imtahanID);
        //        return View(examResults);
        //    }

        //    public ActionResult Create()
        //    {
        //        ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad");
        //        ViewBag.StudentID = new SelectList(db.Students, "ID", "Ad");
        //        return View();
        //    }

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Create([Bind(Include = "ID,ImtahanID,StudentID,DuzSayi,SehvSayi,Bal,Yer")] ExamResult examResult)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.ExamResults.Add(examResult);
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }

        //        ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad", examResult.ImtahanID);
        //        ViewBag.StudentID = new SelectList(db.Students, "ID", "Ad", examResult.StudentID);
        //        return View(examResult);
        //    }

        //    public ActionResult Edit(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        ExamResult examResult = db.ExamResults.Find(id);
        //        if (examResult == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad", examResult.ImtahanID);
        //        ViewBag.StudentID = new SelectList(db.Students, "ID", "Ad", examResult.StudentID);
        //        return View(examResult);
        //    }


        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Edit([Bind(Include = "ID,ImtahanID,StudentID,DuzSayi,SehvSayi,Bal,Yer")] ExamResult examResult)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Entry(examResult).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad", examResult.ImtahanID);
        //        ViewBag.StudentID = new SelectList(db.Students, "ID", "Ad", examResult.StudentID);
        //        return View(examResult);
        //    }

        //    public ActionResult Delete(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        ExamResult examResult = db.ExamResults.Find(id);
        //        if (examResult == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(examResult);
        //    }

        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult DeleteConfirmed(int id)
        //    {
        //        ExamResult examResult = db.ExamResults.Find(id);
        //        db.ExamResults.Remove(examResult);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }
    }
}
