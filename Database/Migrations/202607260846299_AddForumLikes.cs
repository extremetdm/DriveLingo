namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForumLikes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForumLikes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ForumPosts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumLikes", "UserId", "dbo.Users");
            DropForeignKey("dbo.ForumLikes", "PostId", "dbo.ForumPosts");
            DropIndex("dbo.ForumLikes", new[] { "PostId" });
            DropIndex("dbo.ForumLikes", new[] { "UserId" });
            DropTable("dbo.ForumLikes");
        }
    }
}
