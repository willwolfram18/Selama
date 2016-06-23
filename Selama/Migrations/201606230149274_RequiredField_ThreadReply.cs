namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredField_ThreadReply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThreadReplies", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ThreadReplies", "IsActive");
        }
    }
}
