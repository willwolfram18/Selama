namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuildNewsFeed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GuildNewsFeed",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GuildNewsFeed");
        }
    }
}
