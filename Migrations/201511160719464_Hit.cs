namespace Analytics.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Hit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Site = c.String(),
                        Date = c.DateTime(nullable: false),
                        Page = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Hits");
        }
    }
}
