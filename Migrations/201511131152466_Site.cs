namespace Analytics.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Site : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Owner = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sites");
        }
    }
}
