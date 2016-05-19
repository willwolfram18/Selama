namespace Selama.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumModelsCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Forums",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        SubTitle = c.String(maxLength: 60),
                        IsActive = c.Boolean(nullable: false),
                        ForumSectionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ForumSections", t => t.ForumSectionID, cascadeDelete: true)
                .Index(t => t.ForumSectionID);
            
            CreateTable(
                "dbo.ForumSections",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        DisplayOrder = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Threads",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 60),
                        Content = c.String(nullable: false),
                        PostDate = c.DateTime(nullable: false),
                        AuthorID = c.String(nullable: false, maxLength: 128),
                        ForumID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorID)
                .ForeignKey("dbo.Forums", t => t.ForumID, cascadeDelete: true)
                .Index(t => t.AuthorID)
                .Index(t => t.ForumID);
            
            CreateTable(
                "dbo.ThreadReplies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        PostDate = c.DateTime(nullable: false),
                        AuthorID = c.String(nullable: false, maxLength: 128),
                        ThreadID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Threads", t => t.ThreadID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorID)
                .Index(t => t.AuthorID)
                .Index(t => t.ThreadID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Threads", "Forum_ID1", "dbo.Forums");
            DropForeignKey("dbo.Threads", "Forum_ID", "dbo.Forums");
            DropForeignKey("dbo.Threads", "ForumID", "dbo.Forums");
            DropForeignKey("dbo.Threads", "AuthorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ThreadReplies", "AuthorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ThreadReplies", "ThreadID", "dbo.Threads");
            DropForeignKey("dbo.Forums", "ForumSectionID", "dbo.ForumSections");
            DropIndex("dbo.ThreadReplies", new[] { "ThreadID" });
            DropIndex("dbo.ThreadReplies", new[] { "AuthorID" });
            DropIndex("dbo.Threads", new[] { "Forum_ID1" });
            DropIndex("dbo.Threads", new[] { "Forum_ID" });
            DropIndex("dbo.Threads", new[] { "ForumID" });
            DropIndex("dbo.Threads", new[] { "AuthorID" });
            DropIndex("dbo.Forums", new[] { "ForumSectionID" });
            DropTable("dbo.ThreadReplies");
            DropTable("dbo.Threads");
            DropTable("dbo.ForumSections");
            DropTable("dbo.Forums");
        }
    }
}
