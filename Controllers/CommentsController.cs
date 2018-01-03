using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wb6.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace wb6.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        /*public AnnouncementViewModel GetAllComments(int? id, AnnouncementViewModel vm)
        {
            AnnouncementViewModel vm2 = vm;
            

            
            return vm2;
        }*/

        // GET: Comments
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Comments.ToList());
        }

        // GET: Comments/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        //public ActionResult Create([Bind(Include = "CommentID,Content,CreatedAt,AnnouncementID")] Comment comment, Announcement announcement)
        
        public ActionResult Create([Bind(Include = "CommentID, Content")]Comment comment, int? id)
        {
            Announcement a = db.Announcements.FirstOrDefault(x => x.AnnouncementID == id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                comment.CreatedAt = DateTime.Now;
                comment.Announcement = a;
                string currentUserID = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
                comment.User = currentUser;

            }
            if (ModelState.IsValid)
            {

                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("../Announcements/Details", new { id = id});
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CommentID,Content,CreatedAt,AnnouncementID")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin, Lecturer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [Authorize(Roles = "Admin, Lecturer")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
