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


namespace ExamAndQuiz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExamVideoController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            return View(db.ExamVideos.ToList());
        }


        //public ActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Basliq,Melumat,VideoURL")] ExamVideo examVideo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ExamVideos.Add(examVideo);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(examVideo);
        //}

        // GET: ExamVideo/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamVideo examVideo = db.ExamVideos.FirstOrDefault(x => x.ID == id);
            if (examVideo == null)
            {
                return HttpNotFound();
            }
            return View(examVideo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExamVideo examVideo, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/UploadFoto/" + Path.GetFileName(file.FileName)));
                string[] massiv = path.Split('\\');
                examVideo.VideoURL = "~/UploadFoto/" + massiv[massiv.Length - 1];
                file.SaveAs(path);

            }

            if (ModelState.IsValid)
            {

                //db.Entry(examVideo).State = EntityState.Modified;
                var video = db.ExamVideos.SingleOrDefault(x => x.ID == examVideo.ID);
                video.Basliq = examVideo.Basliq;
                video.Melumat = examVideo.Melumat;
                video.VideoURL = examVideo.VideoURL;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(examVideo);
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
