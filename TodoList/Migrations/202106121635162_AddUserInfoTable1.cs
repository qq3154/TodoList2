namespace TodoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserInfoTable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInfoes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserInfoes", new[] { "UserId" });
            DropTable("dbo.UserInfoes");
        }
    }
}
