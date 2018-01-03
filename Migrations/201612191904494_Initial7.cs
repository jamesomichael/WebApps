namespace wb6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Announcement_AnnouncementID", c => c.Int());
            CreateIndex("dbo.Comments", "Announcement_AnnouncementID");
            AddForeignKey("dbo.Comments", "Announcement_AnnouncementID", "dbo.Announcements", "AnnouncementID");
            DropColumn("dbo.Comments", "AnnouncementID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "AnnouncementID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Comments", "Announcement_AnnouncementID", "dbo.Announcements");
            DropIndex("dbo.Comments", new[] { "Announcement_AnnouncementID" });
            DropColumn("dbo.Comments", "Announcement_AnnouncementID");
        }
    }
}
