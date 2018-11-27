using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Lookup
{
    public class LookupDataService : IFriendLookupDataService, IProgrammingLanguagesLookupDataService, IMeetingLookupDataService
    {
        private readonly Func<FriendOrganizerDbContext> _contextCreator;
        public LookupDataService(Func<FriendOrganizerDbContext> contextCreator) => _contextCreator = contextCreator;
        public async Task<IEnumerable<LookupItem>> GetFriendsLookup()
        {
            using (var ctx = _contextCreator())
                return await ctx.Friends.Select(x => new LookupItem{Id = x.Id, DisplayMember = x.FirstName + " " + x.LastName}).ToListAsync();            
        }

        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguagesLookup()
        {
            using (var ctx = _contextCreator())
                return await ctx.ProgrammingLanguages.Select(x => new LookupItem { Id = x.Id, DisplayMember = x.Name }).ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> GetMeetingsLookup()
        {
            using (var ctx = _contextCreator())
                return await ctx.Meetings.Select(x => new LookupItem { Id = x.Id, DisplayMember = x.Title }).ToListAsync();
        }
    }
}
