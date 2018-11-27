namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedProgrammingLangaugesEntity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProgrammingLanguage", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProgrammingLanguage", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
