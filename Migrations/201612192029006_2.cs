namespace wb6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Announcements", "Subject", c => c.String(nullable: false));
            AlterColumn("dbo.Announcements", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Announcements", "Description", c => c.String());
            AlterColumn("dbo.Announcements", "Subject", c => c.String());
        }
    }
}
