namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumSubTitleLenChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Forums", "SubTitle", c => c.String(maxLength: 85));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Forums", "SubTitle", c => c.String(maxLength: 60));
        }
    }
}
