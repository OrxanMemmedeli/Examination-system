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
    public class FreeTicketController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Index()
        {
            return View(db.FreeTickets.OrderBy(x => x.SagirdKodu).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int startCode, int endCode)
        {
            FreeTicket freeTicket = new FreeTicket();

            for (int i = startCode; i <= endCode; i++)
            {
                if (db.FreeTickets.Select(x => x.SagirdKodu).Contains(i) == false)
                {
                    freeTicket.SagirdKodu = i;
                    db.FreeTickets.Add(freeTicket);
                    db.SaveChanges();
                }
                continue;
            }
            return RedirectToAction("Index");

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FreeTicket freeTicket = db.FreeTickets.Find(id);
            if (freeTicket == null)
            {
                return HttpNotFound();
            }
            return View(freeTicket);
        }

        public ActionResult DeleteAll()
        {
            var biletler = db.FreeTickets.ToList();
            db.FreeTickets.RemoveRange(biletler);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FreeTicket freeTicket = db.FreeTickets.Find(id);
            db.FreeTickets.Remove(freeTicket);
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
