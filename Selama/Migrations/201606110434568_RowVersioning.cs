namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RowVersioning : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Forums", "Version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.ForumSections", "Version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Threads", "Version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.ThreadReplies", "Version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ThreadReplies", "Version");
            DropColumn("dbo.Threads", "Version");
            DropColumn("dbo.ForumSections", "Version");
            DropColumn("dbo.Forums", "Version");
        }
    }
}
