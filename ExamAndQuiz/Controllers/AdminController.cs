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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Admin admin = db.Admins.Find(id);
        //    if (admin == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(admin);
        //}

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin)
        {
            var mailControl = db.Admins.FirstOrDefault(o => o.Email == admin.Email);
            var mailControl2 = db.Students.FirstOrDefault(o => o.Email == admin.Email);

            if (mailControl == null && mailControl2 == null)
            {
                if (admin.Parol == admin.TekrarParol)
                {
                    admin.Parol = CreateMD5.ConvertMD5("Khan" + CreateMD5.ConvertMD5(admin.Parol));
                    admin.QeydiyyatTarixi = DateTime.Now;

                    if (ModelState.IsValid)
                    {
                        db.Admins.Add(admin);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.Error = "Daxil edilmiş məlumatlarda uyğunsuzluq aşkar edildi.";
                    return View(admin);
                }
                else
                {
                    ViewBag.Tekrar = "Daxil edilmiş parollar arasında uyğunsuzluq var.";
                    admin.Parol = null;
                    admin.TekrarParol = null;
                }
            }
            else
            {
                ViewBag.Email = "Daxil edilmiş E-mail ünvanı mövcuddur. Zəhmət olmasa fərqli adress daxil edin.";
                admin.Email = null;
                admin.Parol = null;
                admin.TekrarParol = null;
            }

            return View(admin);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin admin)
        {
            var mailControl = db.Admins.FirstOrDefault(o => o.Email == admin.Email);
            var mailControl2 = db.Students.FirstOrDefault(o => o.Email == admin.Email);
            if (mailControl == null && mailControl2 == null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (mailControl.ID == admin.ID)
                {
                    if (ModelState.IsValid)
                    {
                        mailControl.Ad = admin.Ad;
                        mailControl.Soyad = admin.Soyad;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.Email = "Daxil edilmiş E-mail ünvanı mövcuddur. Zəhmət olmasa fərqli adress daxil edin.";
                    admin.Email = null;
                    admin.Parol = null;
                    admin.TekrarParol = null;
                }

            }


            return View(admin);
        }

        public ActionResult UpdatePassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            admin.Parol = null;
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(Admin admin)
        {
            if (admin.Parol == admin.TekrarParol)
            {
                admin.Parol = CreateMD5.ConvertMD5("Khan" + CreateMD5.ConvertMD5(admin.Parol));

                if (ModelState.IsValid)
                {
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.Tekrar = "Daxil edilmiş parollar arasında uyğunsuzluq var.";
                admin.Parol = null;
                admin.TekrarParol = null;
            }

            return View(admin);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
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
