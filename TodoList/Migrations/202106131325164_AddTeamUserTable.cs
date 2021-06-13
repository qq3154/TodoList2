namespace TodoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeamUserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        TeamId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TeamId, t.UserId })
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TeamUsers", "TeamId", "dbo.Teams");
            DropIndex("dbo.TeamUsers", new[] { "UserId" });
            DropIndex("dbo.TeamUsers", new[] { "TeamId" });
            DropTable("dbo.TeamUsers");
        }
    }
}
