namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForumPosts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForumPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Title = c.String(maxLength: 100),
                        Content = c.String(nullable: false, maxLength: 256),
                        ReplyingPostId = c.Int(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ForumPosts", t => t.ReplyingPostId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ReplyingPostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumPosts", "UserId", "dbo.Users");
            DropForeignKey("dbo.ForumPosts", "ReplyingPostId", "dbo.ForumPosts");
            DropIndex("dbo.ForumPosts", new[] { "ReplyingPostId" });
            DropIndex("dbo.ForumPosts", new[] { "UserId" });
            DropTable("dbo.ForumPosts");
        }
    }
}
