namespace wb6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Announcement_AnnouncementID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Announcement_AnnouncementID");
            AddForeignKey("dbo.AspNetUsers", "Announcement_AnnouncementID", "dbo.Announcements", "AnnouncementID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Announcement_AnnouncementID", "dbo.Announcements");
            DropIndex("dbo.AspNetUsers", new[] { "Announcement_AnnouncementID" });
            DropColumn("dbo.AspNetUsers", "Announcement_AnnouncementID");
        }
    }
}
