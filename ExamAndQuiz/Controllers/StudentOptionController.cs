using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExamAndQuiz.Models;
using ExamAndQuiz.Tools;

namespace ExamAndQuiz.Controllers
{
    public class StudentOptionController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var studentOptions = db.StudentOptions.Include(s => s.Exam).Include(s => s.Student);
            return View(studentOptions.ToList());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Exams()
        {
            var exams = db.StudentOptions.ToList();
            return View(exams);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult IndexResult(int id)
        {
            var studentOptions = db.StudentOptions.Where(x => x.Exam.ID == id);
            ViewBag.ExamName = db.Exams.SingleOrDefault(x => x.ID == id).Ad;
            return View(studentOptions);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentOption studentOption = db.StudentOptions.Find(id);
            if (studentOption == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImtahanId = new SelectList(db.Exams, "ID", "Ad", studentOption.ImtahanId);
            ViewBag.SagirdID = new SelectList(db.Students, "ID", "Ad", studentOption.SagirdID);
            return View(studentOption);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,SagirdID,ImtahanId,Bitirmek,Cavablar")] StudentOption studentOption)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentOption).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ImtahanId = new SelectList(db.Exams, "ID", "Ad", studentOption.ImtahanId);
            ViewBag.SagirdID = new SelectList(db.Students, "ID", "Ad", studentOption.SagirdID);
            return View(studentOption);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentOption studentOption = db.StudentOptions.Find(id);
            if (studentOption == null)
            {
                return HttpNotFound();
            }
            return View(studentOption);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentOption studentOption = db.StudentOptions.Find(id);
            db.StudentOptions.Remove(studentOption);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ExamResult(string studentID, string examID)
        {
            int newStudentID = Convert.ToInt32(Encryption.Decrypt(studentID));
            int newExamID = Convert.ToInt32(Encryption.Decrypt(examID));

            ParametrAndS_Option parametrAndS_Option = new ParametrAndS_Option();
            parametrAndS_Option.MyStudentOption = db.StudentOptions.SingleOrDefault(x => x.SagirdID == newStudentID && x.ImtahanId == newExamID);
            var exam = db.Exams.SingleOrDefault(x => x.ID == newExamID);
            parametrAndS_Option.MyExamParameter = db.ExamParameters.Where(k => k.TipParametrID == exam.ExamTypeParameter.ID);

            ViewBag.DogruCavablar = parametrAndS_Option.MyStudentOption.Exam.ExamOptions.FirstOrDefault(x => x.ImtahanID == newExamID).Cavablar.Split('|');
            ViewBag.VerilmisCavablar = parametrAndS_Option.MyStudentOption.Cavablar.Split(';');

            return View(parametrAndS_Option);
        }
        
        [Authorize]
        public ActionResult Print(int studentID, int examID)
        {
            ParametrAndS_Option parametrAndS_Option = new ParametrAndS_Option();
            parametrAndS_Option.MyStudentOption = db.StudentOptions.SingleOrDefault(x => x.SagirdID == studentID && x.ImtahanId == examID);
            var exam = db.Exams.SingleOrDefault(x => x.ID == examID);
            parametrAndS_Option.MyExamParameter = db.ExamParameters.Where(k => k.TipParametrID == exam.ExamTypeParameter.ID);

            ViewBag.DogruCavablar = parametrAndS_Option.MyStudentOption.Exam.ExamOptions.FirstOrDefault(x => x.ImtahanID == examID).Cavablar.Trim().Split('|');
            ViewBag.VerilmisCavablar = parametrAndS_Option.MyStudentOption.Cavablar.Trim().Split(';');

            return View(parametrAndS_Option);
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
