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
    public class StudentController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.City).Include(s => s.CourseGroup).Include(s => s.Grade);
            return View(students.ToList());
        }

        [Authorize]
        public ActionResult Details(string id)
        {
            int newID = Convert.ToInt32(Encryption.Decrypt(id));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profil profil = new Profil(); List<Exam> myExams = new List<Exam>();
            profil.student = db.Students.Find(newID);

            profil.studentActionsBeAttended = db.StudentActions.Where(x => x.StudentID == newID && x.Status == true);
            profil.studentOption = db.StudentOptions.Where(x => x.SagirdID == newID);
            var exams = db.Exams.Where(x => x.SinifID == profil.student.SinifID);
            foreach (var allExam in exams)
            {
                if (allExam.StudentActions.SingleOrDefault(x => x.StudentID == profil.student.ID) == null)
                {
                    myExams.Add(allExam);
                }
            }
            profil.exams = myExams;
            return View(profil);
        }

        public ActionResult Create()
        {
            ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
            ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
            ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student, HttpPostedFileBase file)
        {
            student.Ad = SqlInjection.Protect(student.Ad);
            student.Soyad = SqlInjection.Protect(student.Soyad);
            student.AtaAdi = SqlInjection.Protect(student.AtaAdi);
            student.Email = SqlInjection.Protect(student.Email);
            student.Parol = SqlInjection.Protect(student.Email);

            var mailControl = db.Students.FirstOrDefault(o => o.Email == student.Email);
            var mailControl2 = db.Admins.FirstOrDefault(o => o.Email == student.Email);
            var svControl = db.Students.FirstOrDefault(o => o.SVNomresi == student.SVNomresi);

            if (svControl == null)
            {
                if (mailControl == null && mailControl2 == null)
                {
                    if (student.Parol == student.TekrarParol)
                    {
                        student.QeydiyyatTarixi = DateTime.Now;

                        if (ModelState.IsValid)
                        {
                            student.Parol = Encryption.PassEncrypt(student.Parol);
                            if (student.Parol.Length <= 250)
                            {
                                if (file != null && file.ContentLength > 0)
                                {
                                    string path = Path.Combine(Server.MapPath("~/UploadFoto/Student/"), Path.GetFileName(DateTime.Now.ToString("ddMMyy") + CodeGenerator.Code() + file.FileName));

                                    string[] massiv = path.Split('\\');
                                    student.FotoURL = "~/UploadFoto/Student/" + massiv[massiv.Length - 1];
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    student.FotoURL = "~/UploadFoto/Student/Studentfoto.jpg";
                                }
                                /* Upload İmage END*/
                                if (student.Cins == null)
                                {
                                    student.Cins = " ";
                                }
                                db.Students.Add(student);
                                db.SaveChanges();
                                ViewBag.Success = "Qeydiyyat Uğurla Tamamlandı! Sistemə daxil ola bilərsiniz.";
                                return RedirectToAction("Login", "Account");
                            }
                            else{
                                ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
                                ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
                                ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
                                ViewBag.Error = "Sifrə üçün qoyulmuş simvol sayı aşıldı";
                                student.Parol = null;
                                student.TekrarParol = null;
                                return View(student);
                            }
                        }

                        ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
                        ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
                        ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
                        ViewBag.Error = "Daxil edilmiş məlumatlarda uyğunsuzluq aşkar edildi.";
                        return View(student);
                    }
                    else
                    {

                        ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
                        ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
                        ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
                        ViewBag.Parol = "Daxil edilmiş parollar arasında uyğunsuzluq var.";
                        student.Parol = null;
                        student.TekrarParol = null;
                        return View(student);
                    }
                }
                else
                {
                    ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
                    ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
                    ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
                    ViewBag.Email = "Daxil edilmiş E-mail ünvanı mövcuddur. Zəhmət olmasa fərqli adress daxil edin.";
                    student.Email = null;
                    student.Parol = null;
                    student.TekrarParol = null;
                    return View(student);
                }
            }
            else
            {
                if (svControl.Ad == student.Ad && svControl.Soyad == student.Soyad && svControl.AtaAdi == student.AtaAdi)
                {
                    ViewBag.SV = "Sistemdə Seria nömrəsi " + student.SVNomresi + " olan " + student.Soyad + " " + student.Ad + " " + student.AtaAdi + " adlı şagird mövcuddur. Zəhmət olmasa məlumatların doğruluğunu yoxlayasanız.";
                }
                else
                {
                    ViewBag.SV = "Daxil edilmiş Seria nömrəsi mövcuddur. Zəhmət olmasa seria nömrəsinin doğruluğunu yoxlayın.";
                    student.SVNomresi = null;
                    student.Parol = null;
                    student.TekrarParol = null;
                }

            }
            ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
            ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
            ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
            return View(student);

        }

        [Authorize]
        public ActionResult EditStudent(string id)
        {
            int newID = Convert.ToInt32(Encryption.Decrypt(id));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(newID);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
            ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
            ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditStudent(Student student, HttpPostedFileBase file)
        {
            student.Ad = SqlInjection.Protect(student.Ad);
            student.Soyad = SqlInjection.Protect(student.Soyad);
            student.AtaAdi = SqlInjection.Protect(student.AtaAdi);
            student.Email = SqlInjection.Protect(student.Email);
            student.Parol = SqlInjection.Protect(student.Email);

            var mailControl = db.Students.FirstOrDefault(o => o.Email == student.Email);
            var mailControl2 = db.Admins.FirstOrDefault(o => o.Email == student.Email);
            var svControl = db.Students.FirstOrDefault(o => o.SVNomresi == student.SVNomresi);
            var id = db.Students.FirstOrDefault(o => o.ID == student.ID);

            if (id.Parol == Encryption.PassEncrypt(student.TekrarParol))
            {
                if (svControl == null)
                {
                    if (mailControl == null && mailControl2 == null)
                    {
                        if (ModelState.IsValid)
                        {
                            db.Entry(student).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
                        ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
                        ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
                        ViewBag.Email = "Daxil edilmiş E-mail ünvanı mövcuddur. Zəhmət olmasa fərqli adress daxil edin.";
                        return View(student);
                    }
                }
                else
                {
                    if (svControl.ID == student.ID)
                    {
                        if (mailControl == null)
                        {
                            if (ModelState.IsValid)
                            {
                                if (file != null && file.ContentLength > 0)
                                {
                                    string path = Path.Combine(Server.MapPath("~/UploadFoto/Student/"), Path.GetFileName(DateTime.Now.ToString("ddMMyy") + CodeGenerator.Code() + file.FileName));
                                    string[] massiv = path.Split('\\');
                                    student.FotoURL = "~/UploadFoto/Student/" + massiv[massiv.Length - 1];
                                    file.SaveAs(path);
                                }

                                var update = db.Students.FirstOrDefault(x => x.ID == student.ID);
                                update.Ad = student.Ad;
                                update.Soyad = student.Soyad;
                                update.AtaAdi = student.AtaAdi;
                                update.SVNomresi = student.SVNomresi;
                                update.DogumTarixi = student.DogumTarixi;
                                update.SinifID = student.SinifID;
                                update.SeherID = student.SeherID;
                                update.QrupID = student.QrupID;
                                update.Cins = student.Cins;
                                update.Email = student.Email;
                                update.WhatsApp = student.WhatsApp;
                                update.MobilNomre = student.MobilNomre;
                                update.FotoURL = student.FotoURL;
                                db.SaveChanges();
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            if (mailControl.ID == student.ID)
                            {
                                if (ModelState.IsValid)
                                {
                                    if (file != null && file.ContentLength > 0)
                                    {
                                        string path = Path.Combine(Server.MapPath("~/UploadFoto/Student/"), Path.GetFileName(DateTime.Now.ToString("ddMMyy") + CodeGenerator.Code() + file.FileName));

                                        string[] massiv = path.Split('\\');
                                        student.FotoURL = "~/UploadFoto/Student/" + massiv[massiv.Length - 1];
                                        file.SaveAs(path);
                                    }

                                    var update = db.Students.FirstOrDefault(x => x.ID == student.ID);
                                    update.Ad = student.Ad;
                                    update.Soyad = student.Soyad;
                                    update.AtaAdi = student.AtaAdi;
                                    update.SVNomresi = student.SVNomresi;
                                    update.DogumTarixi = student.DogumTarixi;
                                    update.SinifID = student.SinifID;
                                    update.SeherID = student.SeherID;
                                    update.QrupID = student.QrupID;
                                    update.Cins = student.Cins;
                                    update.Email = student.Email;
                                    update.WhatsApp = student.WhatsApp;
                                    update.MobilNomre = student.MobilNomre;
                                    update.FotoURL = student.FotoURL;

                                    db.SaveChanges();
                                    return RedirectToAction("Index");
                                }
                            }
                            else
                            {
                                ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
                                ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
                                ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
                                ViewBag.Email = "Daxil edilmiş E-mail ünvanı mövcuddur. Zəhmət olmasa fərqli adress daxil edin.";
                                return View(student);
                            }
                        }

                    }
                    else
                    {
                        if (svControl.Ad == student.Ad && svControl.Soyad == student.Soyad && svControl.AtaAdi == student.AtaAdi)
                        {
                            ViewBag.SV = "Sistemdə Seria nömrəsi " + student.SVNomresi + " olan " + student.Soyad + " " + student.Ad + " " + student.AtaAdi + " adlı şagird mövcuddur. Zəhmət olmasa məlumatların doğruluğunu yoxlayasanız.";
                        }
                        else
                        {
                            ViewBag.SV = "Daxil edilmiş Seria nömrəsi mövcuddur. Zəhmət olmasa seria nömrəsinin doğruluğunu yoxlayın.";
                        }
                    }
                }
                ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
                ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
                ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
                return View(student);
            }
            else
            {
                ViewBag.Parol = "Sifrə doğru daxil edilməyib.";
                ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
                ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
                ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");

                return View(student);
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.SeherID = new SelectList(db.Cities.OrderBy(o => o.Seher), "ID", "Seher");
            ViewBag.SinifID = new SelectList(db.Grades.OrderBy(o => o.Sinif), "ID", "Sinif");
            ViewBag.QrupID = new SelectList(db.CourseGroups.OrderBy(o => o.Qrup), "ID", "Qrup");
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Student student)
        {
            var codeKontrol = db.Students.SingleOrDefault(x => x.SagirdKodu == student.SagirdKodu);

            if (codeKontrol == null)
            {
                if (ModelState.IsValid)
                {
                    student.Parol = Encryption.PassEncrypt(student.Parol);
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (student.ID == codeKontrol.ID)
                {
                    if (ModelState.IsValid)
                    {
                        student.Parol = Encryption.PassEncrypt(student.Parol);
                        codeKontrol.SagirdKodu = student.SagirdKodu;
                        codeKontrol.Parol = student.Parol;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.Error = "Şagird Kodu mövcuddur.";
                }
            }

            return View(student);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
