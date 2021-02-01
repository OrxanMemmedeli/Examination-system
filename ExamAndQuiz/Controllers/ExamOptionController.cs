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
    public class ExamOptionController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            var examOptions = db.ExamOptions.Include(e => e.BookletOption).Include(e => e.Exam);
            return View(examOptions.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamOption examOption = db.ExamOptions.Find(id);
            if (examOption == null)
            {
                return HttpNotFound();
            }
            return View(examOption);
        }

        public ActionResult Create()
        {
            ViewBag.VariantID = new SelectList(db.BookletOptions, "ID", "Variant");
            ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExamOption examOption)
        {
            var option = db.ExamOptions.FirstOrDefault(x => x.ImtahanID == examOption.ImtahanID && x.VariantID == examOption.VariantID);
            examOption.VariantID = 1;
            if (option == null)
            {
                examOption.Cavablar = "Cavablar Daxil Edilməyib";
                if (ModelState.IsValid)
                {
                    db.ExamOptions.Add(examOption);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.Error = "Daxil edilmiş  imtahan adı və varianta uyğun cavablar mövcuddur. Yenidən cavab daxil edə bilməzsiniz.";
            }


            ViewBag.VariantID = new SelectList(db.BookletOptions, "ID", "Variant", examOption.VariantID);
            ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad", examOption.ImtahanID);
            return View(examOption);
        }

        public ActionResult Edit(int? id)
        {
            OptionAndParameter optionAndParameter = new OptionAndParameter();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamOption examOption = db.ExamOptions.Find(id);
            if (examOption == null)
            {
                return HttpNotFound();
            }
            ViewBag.VariantID = new SelectList(db.BookletOptions, "ID", "Variant", examOption.VariantID);
            ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad", examOption.ImtahanID);
            optionAndParameter.MyExamOption = examOption;
            optionAndParameter.MyExamParameter = db.ExamParameters.Where(x => x.TipParametrID == examOption.Exam.TipParametrID).OrderBy(x => x.Sira).ToList();
            return View(optionAndParameter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OptionAndParameter examOption)
        {

            string[] massiv = examOption.InputCavablar;
            string cavab = "";
            for (int i = 0; i < massiv.Count(); i++)
            {
                cavab += massiv[i] + " | ";
            }
            //string[] parts1 = input.Split(new string[] { "][" }, StringSplitOptions.None);


            if (ModelState.IsValid)
            {
                var result = db.ExamOptions.Find(examOption.MyExamOption.ID);
                result.ImtahanID = examOption.MyExamOption.ImtahanID;
                result.VariantID = examOption.MyExamOption.VariantID;
                result.Cavablar = cavab.ToUpper();
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VariantID = new SelectList(db.BookletOptions, "ID", "Variant", examOption.MyExamOption.VariantID);
            ViewBag.ImtahanID = new SelectList(db.Exams, "ID", "Ad", examOption.MyExamOption.ImtahanID);
            return View(examOption);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamOption examOption = db.ExamOptions.Find(id);
            if (examOption == null)
            {
                return HttpNotFound();
            }
            return View(examOption);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamOption examOption = db.ExamOptions.Find(id);
            db.ExamOptions.Remove(examOption);
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
