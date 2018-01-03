using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wb6.Models;

namespace wb6.Controllers
{
    public class AnnouncementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Announcements
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IEnumerable<Announcement> GetAnnouncements()
        {
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);

            int announcementsSeen = 0;
            foreach (Seen s in db.MarkedAsRead.Where(x => x.User.Id == currentUserID))
            {
                announcementsSeen++;
            }

            ViewBag.Percent = Math.Round(100f * (float)announcementsSeen / (float)db.Announcements.Count());
            
            return db.Announcements.ToList();
        }

        [Authorize]
        public ActionResult BuildAnnouncementTable()
        {
            return PartialView("_AnnouncementTable", GetAnnouncements());
        }

        [Authorize]
        public ActionResult BuildAnnouncementDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }

            AnnouncementViewModel vm = new AnnouncementViewModel();
            vm.Announcement = announcement;
            vm.Comments = new List<Comment>();
            vm.MarkedAsRead = new List<Seen>();

            foreach (Comment c in db.Comments.Where(x => x.Announcement.AnnouncementID == announcement.AnnouncementID))
            {
                vm.Comments.Add(c);
            }
            foreach (Seen s in db.MarkedAsRead.ToList().Where(x => x.Announcement.AnnouncementID == id))
            {
                vm.MarkedAsRead.Add(s);
            }
            return PartialView("_AnnouncementDetails", vm);
        }

        // GET: Announcements/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }

            AnnouncementViewModel vm = new AnnouncementViewModel();
            vm.Announcement = announcement;

            //CommentsController cc = new CommentsController();
            //cc.GetAllComments(id, vm)
            vm.Comments = new List<Comment>();

            foreach (Comment c in db.Comments.Where(x => x.Announcement.AnnouncementID == announcement.AnnouncementID))
            {
                vm.Comments.Add(c);
            }

            MarkAsSeen(announcement);

            vm.MarkedAsRead = new List<Seen>();
            foreach (Seen s in db.MarkedAsRead.ToList().Where(x => x.Announcement.AnnouncementID == id))
            {
                vm.MarkedAsRead.Add(s);
            }

            return View(vm);
        }

        [Authorize]
        private void MarkAsSeen(Announcement announcement)
        {
            // Mark announcement as 'Seen'
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            Seen view = db.MarkedAsRead.FirstOrDefault(x => x.Announcement.AnnouncementID == announcement.AnnouncementID && x.User.Id == currentUserID);
            if (view == null)
            {
                Seen viewed = new Seen();
                announcement.MarkedAsRead = new List<ApplicationUser>();
                viewed.Announcement = announcement;
                viewed.User = currentUser;
                viewed.HasSeen = true;
                db.MarkedAsRead.Add(viewed);
                announcement.MarkedAsRead.Add(currentUser);
                db.SaveChanges();
            }
        }

        // GET: Announcements/Create
        [Authorize(Roles = "Lecturer, Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer, Admin")]
        public ActionResult Create([Bind(Include = "AnnouncementID,Subject,Description")] Announcement announcement)
        {

            if (ModelState.IsValid)
            {
                announcement.CreatedAt = DateTime.Now;
                string currentUserID = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
                announcement.User = currentUser;

                db.Announcements.Add(announcement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(announcement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer, Admin")]
        public ActionResult AJAXCreate([Bind(Include = "AnnouncementID,Subject,Description,CreatedAt")] Announcement announcement)
        {

            if (ModelState.IsValid)
            {
                announcement.CreatedAt = DateTime.Now;
                string currentUserID = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
                announcement.User = currentUser;

                db.Announcements.Add(announcement);
                db.SaveChanges();
            }

            return PartialView("_AnnouncementTable", GetAnnouncements());
        }

        /*public static void AddComment(Announcement announcement)
        {
            CommentsController c = new CommentsController();
            Comment comment = new Comment();
            c.Create(comment, announcement);

        }*/


        // GET: Announcements/Edit/5
        [Authorize(Roles = "Lecturer, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }

            return View(announcement);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer, Admin")]
        public ActionResult Edit([Bind(Include = "AnnouncementID,Subject,Description,Seen,CreatedAt")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.CreatedAt = DateTime.Now;
                string currentUserID = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
                announcement.User = currentUser;
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(announcement);
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AJAXEdit(int? id, bool seen)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            else
            {
                //announcement.CreatedAt = DateTime.Now;
                announcement.Seen = seen;
                string currentUserID = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
                announcement.User = currentUser;
                Seen marked = new Seen();
                if (seen == true)
                {
                    marked.Announcement = db.Announcements.Find(id);
                    marked.User = currentUser;
                    db.MarkedAsRead.Add(marked);
                    db.Entry(announcement).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return PartialView("_AnnouncementDetails", BuildAnnouncementDetails(id));
        }*/

        // GET: Announcements/Delete/5
        [Authorize(Roles = "Lecturer, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Announcements/Delete/5
        // Deleting an announcement also deletes all comments associated with it
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer, Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            // Find the appropriate announcement in the database
            Announcement announcement = db.Announcements.Find(id);

            // Find and remove each comment associated with the announcement from the database
            foreach (Comment c in db.Comments.Where(x => x.Announcement.AnnouncementID == id))
            {
                db.Comments.Remove(c);
            }

            foreach (Seen s in db.MarkedAsRead.Where(x => x.Announcement.AnnouncementID == id))
            {
                db.MarkedAsRead.Remove(s);
            }

            // Remove announcement from database and save changes
            db.Announcements.Remove(announcement);
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
