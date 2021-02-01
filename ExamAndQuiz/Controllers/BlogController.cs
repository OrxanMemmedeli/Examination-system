using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExamAndQuiz.Models;
using ExamAndQuiz.Tools;


namespace ExamAndQuiz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            return View(db.Blogs.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Basliq,Melumat,FotoURL,Tarix")] Blog blog, HttpPostedFileBase file)
        {
            blog.Tarix = DateTime.Now;
            if (file != null && file.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/UploadFoto/Blog/"), Path.GetFileName(DateTime.Now.ToString("ddMMyy") + CodeGenerator.Code() + file.FileName));

                string[] massiv = path.Split('\\');
                blog.FotoURL = "~/UploadFoto/Blog/" + massiv[massiv.Length - 1];
                file.SaveAs(path);
            }

            if (ModelState.IsValid)
            {
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Basliq,Melumat,FotoURL,Tarix")] Blog blog, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/UploadFoto/Blog/"), Path.GetFileName(DateTime.Now.ToString("ddMMyy") + CodeGenerator.Code() + file.FileName));

                string[] massiv = path.Split('\\');
                blog.FotoURL = "~/UploadFoto/Blog/" + massiv[massiv.Length - 1];
                file.SaveAs(path);
            }

            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
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
