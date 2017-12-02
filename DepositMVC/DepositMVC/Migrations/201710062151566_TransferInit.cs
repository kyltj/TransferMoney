namespace DepositMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransferInit : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TransferMoneyUsers", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransferMoneyUsers", "Status", c => c.String(nullable: false));
        }
    }
}
