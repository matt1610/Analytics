namespace Analytics.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedHitDatetostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Hits", "Date", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Hits", "Date", c => c.DateTime(nullable: false));
        }
    }
}
