namespace Apparent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TBL_APPARENT_SERVICE_PAYMENT", "UserName", c => c.String(nullable:true)) ;
            AddColumn("dbo.TBL_APPARENT_SERVICE_PAYMENT", "card_category", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TBL_APPARENT_SERVICE_PAYMENT", "card_category");
            DropColumn("dbo.TBL_APPARENT_SERVICE_PAYMENT", "UserName");
        }
    }
}
