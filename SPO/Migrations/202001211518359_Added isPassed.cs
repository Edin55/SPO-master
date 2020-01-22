namespace SPO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedisPassed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exams", "isPassed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exams", "isPassed");
        }
    }
}
