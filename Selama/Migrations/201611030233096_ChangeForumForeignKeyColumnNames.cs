namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeForumForeignKeyColumnNames : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Forums", new[] { "ForumSectionID" });
            DropIndex("dbo.Threads", new[] { "AuthorID" });
            DropIndex("dbo.Threads", new[] { "ForumID" });
            DropIndex("dbo.ThreadReplies", new[] { "AuthorID" });
            DropIndex("dbo.ThreadReplies", new[] { "ThreadID" });
            CreateIndex("dbo.Forums", "ForumSectionId");
            CreateIndex("dbo.Threads", "AuthorId");
            CreateIndex("dbo.Threads", "ForumId");
            CreateIndex("dbo.ThreadReplies", "AuthorId");
            CreateIndex("dbo.ThreadReplies", "ThreadId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ThreadReplies", new[] { "ThreadId" });
            DropIndex("dbo.ThreadReplies", new[] { "AuthorId" });
            DropIndex("dbo.Threads", new[] { "ForumId" });
            DropIndex("dbo.Threads", new[] { "AuthorId" });
            DropIndex("dbo.Forums", new[] { "ForumSectionId" });
            CreateIndex("dbo.ThreadReplies", "ThreadID");
            CreateIndex("dbo.ThreadReplies", "AuthorID");
            CreateIndex("dbo.Threads", "ForumID");
            CreateIndex("dbo.Threads", "AuthorID");
            CreateIndex("dbo.Forums", "ForumSectionID");
        }
    }
}
