namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThreadLocking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Threads", "IsLocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Threads", "IsLocked");
        }
    }
}
