namespace wb6.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using wb6.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<wb6.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(wb6.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            AddRoles(context);
            AddUsers(context);
            //AddLecturer(context);

            ApplicationUser lecturer = context.Users.FirstOrDefault(x => x.UserName == "Lecturer1@email.com");
            ApplicationUser student4 = context.Users.FirstOrDefault(x => x.UserName == "Student4@email.com");

            //int annID = context.Announcements.FirstOrDefault(x => x.AnnouncementID == )

            Announcement seededAnnouncement = new Announcement
            {
                Subject = "(Seeded) Whiteboard Announcements Now Available!",
                Description = "Hi everyone!",
                User = lecturer,
                CreatedAt = DateTime.Now,
            };
            context.Announcements.AddOrUpdate(seededAnnouncement);

            context.Comments.AddOrUpdate(new Models.Comment
            {
                CommentID = 998,
                Content = "Is there a lab this week?",
                Announcement = seededAnnouncement,
                User = student4,
                CreatedAt = DateTime.Now,
            });

            context.Comments.AddOrUpdate(new Models.Comment
            {
                CommentID = 999,
                Content = "Yes - Thursday at 10am.",
                Announcement = seededAnnouncement,
                User = lecturer,
                CreatedAt = DateTime.Now,
            });
        }

        void AddRoles(wb6.Models.ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole
            {
                Name = "Lecturer"
            });
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole
            {
                Name = "Student"
            });
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole
            {
                Name = "Admin"
            });
        }

        void AddUsers(wb6.Models.ApplicationDbContext context)
        {
            var userLecturer = new ApplicationUser { UserName = "Lecturer1@email.com" };
            var userStudent1 = new ApplicationUser { UserName = "Student1@email.com" };
            var userStudent2 = new ApplicationUser { UserName = "Student2@email.com" };
            var userStudent3 = new ApplicationUser { UserName = "Student3@email.com" };
            var userStudent4 = new ApplicationUser { UserName = "Student4@email.com" };
            var userStudent5 = new ApplicationUser { UserName = "Student5@email.com" };

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            userManager.Create(userLecturer, "password");
            userManager.Create(userStudent1, "password");
            userManager.Create(userStudent2, "password");
            userManager.Create(userStudent3, "password");
            userManager.Create(userStudent4, "password");
            userManager.Create(userStudent5, "password");

            var userManagerRoles = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var lecturer1 = userManagerRoles.FindByName("Lecturer1@email.com");
            var student1 = userManagerRoles.FindByName("Student1@email.com");
            var student2 = userManagerRoles.FindByName("Student2@email.com");
            var student3 = userManagerRoles.FindByName("Student3@email.com");
            var student4 = userManagerRoles.FindByName("Student4@email.com");
            var student5 = userManagerRoles.FindByName("Student5@email.com");

            userManager.AddToRole(lecturer1.Id, "Lecturer");
            userManager.AddToRole(student1.Id, "Student");
            userManager.AddToRole(student2.Id, "Student");
            userManager.AddToRole(student3.Id, "Student");
            userManager.AddToRole(student4.Id, "Student");
            userManager.AddToRole(student5.Id, "Student");



        }

    }
}
