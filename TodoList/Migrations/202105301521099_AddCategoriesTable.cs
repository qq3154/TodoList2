namespace TodoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Todoes", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Todoes", "CategoryId");
            AddForeignKey("dbo.Todoes", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            DropColumn("dbo.Todoes", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Todoes", "Category", c => c.String(nullable: false));
            DropForeignKey("dbo.Todoes", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Todoes", new[] { "CategoryId" });
            DropColumn("dbo.Todoes", "CategoryId");
            DropTable("dbo.Categories");
        }
    }
}
