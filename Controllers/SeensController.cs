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
    public class SeensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Seens
        [Authorize]
        public ActionResult Index(int? id)
        {
            return View(db.MarkedAsRead.ToList().Where(x => x.Announcement.AnnouncementID == id));
        }

        [Authorize]
        public ActionResult NotSeen(int? id)
        {
            Announcement announcement = db.Announcements.Find(id);
            List<ApplicationUser> allUsers = new List<ApplicationUser>();
            List<ApplicationUser> usersSeen = new List<ApplicationUser>();
            IEnumerable<ApplicationUser> usersNotSeen = new List<ApplicationUser>();

            // Add all users to one list
            foreach (ApplicationUser u in db.Users)
            {
                allUsers.Add(u);
            }

            // Add only users who have seen the announcement to another list
            foreach (Seen s in db.MarkedAsRead.Where(x => x.Announcement.AnnouncementID == id))
            {
                usersSeen.Add(s.User);
            }

            // Compare the two lists to find who has not seen the announcement
            // Find the users who do not exist in the 'seen' list
            usersNotSeen = allUsers.Where(x => !usersSeen.Contains(x));

            return View(usersNotSeen.ToList());
        }

        // GET: Seens/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seen seen = db.MarkedAsRead.Find(id);
            if (seen == null)
            {
                return HttpNotFound();
            }
            return View(seen);
        }

        // GET: Seens/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Seens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "SeenID,HasSeen")] Seen seen)
        {
            if (ModelState.IsValid)
            {
                db.MarkedAsRead.Add(seen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(seen);
        }

        // GET: Seens/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seen seen = db.MarkedAsRead.Find(id);
            if (seen == null)
            {
                return HttpNotFound();
            }
            return View(seen);
        }

        // POST: Seens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SeenID,HasSeen")] Seen seen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seen);
        }

        // GET: Seens/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seen seen = db.MarkedAsRead.Find(id);
            if (seen == null)
            {
                return HttpNotFound();
            }
            return View(seen);
        }

        // POST: Seens/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seen seen = db.MarkedAsRead.Find(id);
            db.MarkedAsRead.Remove(seen);
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
