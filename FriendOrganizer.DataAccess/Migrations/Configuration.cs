namespace FriendOrganizer.DataAccess.Migrations
{
    using FriendOrganizer.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizer.DataAccess.FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizer.DataAccess.FriendOrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(m => m.FirstName,
                new Friend { FirstName = "Kbreab", LastName = "Hagos", Email = "kibtsegai@gmail.com" },
                new Friend { FirstName = "Mehari", LastName = "Hadgu", Email = "meharihad@gmail.com" },
                new Friend { FirstName = "Yohannes", LastName = "Tekie", Email = "jonitekie@gmail.com" },
                new Friend { FirstName = "Amanuel", LastName = "Abraham", Email = "amitty@gmail.com" },
                new Friend { FirstName = "Filipos", LastName = "Feseha", Email = "philipfeseha@gmail.com" }
                );

            context.ProgrammingLanguages.AddOrUpdate(x => x.Name,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "Java" },
                new ProgrammingLanguage { Name = "Typescript" },
                new ProgrammingLanguage { Name = "Python" },
                new ProgrammingLanguage { Name = "Kotlin" }
                );

            context.SaveChanges();
            context.FriendPhoneNumbers.AddOrUpdate(x => x.Number,
                new FriendPhoneNumber { Number = "+358440315554", FriendId = context.Friends.First().Id });
        }
    }
}
