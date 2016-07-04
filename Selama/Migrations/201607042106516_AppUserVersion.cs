namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppUserVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Version");
        }
    }
}
