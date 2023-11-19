namespace AdventureWorksAppWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.Users");
            DropIndex("dbo.Photos", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropColumn("dbo.Photos", "UserId");
            DropColumn("dbo.Comments", "UserId");
            RenameColumn(table: "dbo.Photos", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Comments", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Photos", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Photos", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Photos", "UserId");
            CreateIndex("dbo.Comments", "UserId");
            AddForeignKey("dbo.Photos", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Photos", "UserId", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Photos", new[] { "UserId" });
            AlterColumn("dbo.Comments", "UserId", c => c.Int());
            AlterColumn("dbo.Comments", "UserId", c => c.String());
            AlterColumn("dbo.Photos", "UserId", c => c.Int());
            AlterColumn("dbo.Photos", "UserId", c => c.String());
            RenameColumn(table: "dbo.Comments", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Photos", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Comments", "UserId", c => c.String());
            AddColumn("dbo.Photos", "UserId", c => c.String());
            CreateIndex("dbo.Comments", "User_Id");
            CreateIndex("dbo.Photos", "User_Id");
            AddForeignKey("dbo.Comments", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Photos", "User_Id", "dbo.Users", "Id");
        }
    }
}
