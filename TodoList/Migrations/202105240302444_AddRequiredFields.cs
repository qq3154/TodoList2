namespace TodoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Todoes", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Todoes", "Category", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Todoes", "Category", c => c.String());
            AlterColumn("dbo.Todoes", "Description", c => c.String());
        }
    }
}
