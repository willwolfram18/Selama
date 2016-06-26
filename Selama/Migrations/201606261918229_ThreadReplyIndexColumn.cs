namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThreadReplyIndexColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThreadReplies", "ReplyIndex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ThreadReplies", "ReplyIndex");
        }
    }
}
