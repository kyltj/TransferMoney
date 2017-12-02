namespace DepositMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaavbn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDeposits", "DateEnd", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDeposits", "DateEnd");
        }
    }
}
