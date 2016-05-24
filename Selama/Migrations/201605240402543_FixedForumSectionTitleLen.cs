namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedForumSectionTitleLen : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ForumSections", "Title", c => c.String(nullable: false, maxLength: 35));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForumSections", "Title", c => c.String(nullable: false, maxLength: 150));
        }
    }
}
