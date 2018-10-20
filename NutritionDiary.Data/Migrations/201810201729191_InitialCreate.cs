namespace NutritionDiary.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Secret = c.String(),
                        AppId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        Expiration = c.DateTime(nullable: false),
                        ApiUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiUsers", t => t.ApiUser_Id)
                .Index(t => t.ApiUser_Id);
            
            CreateTable(
                "dbo.Diaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        CurrentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiaryEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Double(nullable: false),
                        DiaryId = c.Int(nullable: false),
                        FoodItem_Id = c.Int(),
                        Measure_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Diaries", t => t.DiaryId, cascadeDelete: true)
                .ForeignKey("dbo.Foods", t => t.FoodItem_Id)
                .ForeignKey("dbo.Measures", t => t.Measure_Id)
                .Index(t => t.DiaryId)
                .Index(t => t.FoodItem_Id)
                .Index(t => t.Measure_Id);
            
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Measures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Calories = c.Double(nullable: false),
                        TotalFat = c.Double(nullable: false),
                        SaturatedFat = c.Double(nullable: false),
                        Protein = c.Double(nullable: false),
                        Carbohydrates = c.Double(nullable: false),
                        Fiber = c.Double(nullable: false),
                        Sugar = c.Double(nullable: false),
                        Sodium = c.Double(nullable: false),
                        Iron = c.Double(nullable: false),
                        Cholestrol = c.Double(nullable: false),
                        FoodId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Foods", t => t.FoodId, cascadeDelete: true)
                .Index(t => t.FoodId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DiaryEntries", "Measure_Id", "dbo.Measures");
            DropForeignKey("dbo.DiaryEntries", "FoodItem_Id", "dbo.Foods");
            DropForeignKey("dbo.Measures", "FoodId", "dbo.Foods");
            DropForeignKey("dbo.DiaryEntries", "DiaryId", "dbo.Diaries");
            DropForeignKey("dbo.AuthTokens", "ApiUser_Id", "dbo.ApiUsers");
            DropIndex("dbo.Measures", new[] { "FoodId" });
            DropIndex("dbo.DiaryEntries", new[] { "Measure_Id" });
            DropIndex("dbo.DiaryEntries", new[] { "FoodItem_Id" });
            DropIndex("dbo.DiaryEntries", new[] { "DiaryId" });
            DropIndex("dbo.AuthTokens", new[] { "ApiUser_Id" });
            DropTable("dbo.Measures");
            DropTable("dbo.Foods");
            DropTable("dbo.DiaryEntries");
            DropTable("dbo.Diaries");
            DropTable("dbo.AuthTokens");
            DropTable("dbo.ApiUsers");
        }
    }
}
