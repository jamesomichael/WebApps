namespace wb6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Seens", "HasSeen", c => c.Boolean(nullable: false));
            DropColumn("dbo.Announcements", "Seen");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Announcements", "Seen", c => c.Boolean(nullable: false));
            DropColumn("dbo.Seens", "HasSeen");
        }
    }
}
