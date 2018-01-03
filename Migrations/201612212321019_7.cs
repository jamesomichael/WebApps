namespace wb6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Announcement_AnnouncementID", "dbo.Announcements");
            AddColumn("dbo.AspNetUsers", "Announcement_AnnouncementID1", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Announcement_AnnouncementID1");
            AddForeignKey("dbo.AspNetUsers", "Announcement_AnnouncementID1", "dbo.Announcements", "AnnouncementID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Announcement_AnnouncementID1", "dbo.Announcements");
            DropIndex("dbo.AspNetUsers", new[] { "Announcement_AnnouncementID1" });
            DropColumn("dbo.AspNetUsers", "Announcement_AnnouncementID1");
            AddForeignKey("dbo.AspNetUsers", "Announcement_AnnouncementID", "dbo.Announcements", "AnnouncementID");
        }
    }
}
