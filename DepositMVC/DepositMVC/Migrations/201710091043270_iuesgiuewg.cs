namespace DepositMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iuesgiuewg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDeposits", "AccuralYet", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDeposits", "AccuralYet");
        }
    }
}
