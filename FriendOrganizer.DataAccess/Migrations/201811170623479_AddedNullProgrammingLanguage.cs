namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNullProgrammingLanguage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProgrammingLanguage", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProgrammingLanguage", "Discriminator");
        }
    }
}
