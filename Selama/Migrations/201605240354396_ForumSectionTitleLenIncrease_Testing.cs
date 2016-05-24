namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumSectionTitleLenIncrease_Testing : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ForumSections", "Title", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForumSections", "Title", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
