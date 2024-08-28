namespace Apparent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTBL_APPARENT_SERVICE_PAYMENT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TBL_APPARENT_SERVICE_PAYMENT",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        card_holder = c.String(nullable:true),
                        email = c.String(nullable: true),
                        card_number = c.String(nullable: true),
                        card_type = c.String(nullable: true),
                        decimal_amount = c.Decimal(nullable: true, precision: 18, scale: 2),
                        currency = c.String(nullable: true),
                        reference = c.String(nullable: true),
                        successful = c.Boolean(nullable: true),
                        message = c.String(nullable: true),
                        transaction_id = c.String(),
                        respons_id = c.String(nullable: true),
                        settlement_date = c.String(nullable: true),
                        transaction_date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
           
            
        }
        
        public override void Down()
        {
         
            DropTable("dbo.TBL_APPARENT_SERVICE_PAYMENT");
        }
    }
}
