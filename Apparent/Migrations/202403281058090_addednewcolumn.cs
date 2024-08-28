namespace Apparent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addednewcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TBL_APPARENT_SERVICE_PAYMENT", "Notes", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TBL_APPARENT_SERVICE_PAYMENT", "Notes");
        }
    }
}
