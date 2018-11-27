using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendRepository : IFriendRepository
    {
        private readonly FriendOrganizerDbContext _context;
        public FriendRepository(FriendOrganizerDbContext context) => _context = context;
        public void Add(Friend friend) => _context.Friends.Add(friend);
        public async Task<Friend> GetFriendById(int id) => await _context.Friends.Include(x => x.PhoneNumbers).FirstOrDefaultAsync(x => x.Id == id);
        public async Task<IEnumerable<ProgrammingLanguage>> GetProgrammingLanguagesAsync() => 
            await _context.ProgrammingLanguages.OrderBy(x => x.Name).ToListAsync();
        public bool HasChanges() => _context.ChangeTracker.HasChanges();
        public void Remove(Friend model) => _context.Friends.Remove(model);
        public void RemovePhoneNumber(FriendPhoneNumber model) => _context.FriendPhoneNumbers.Remove(model);
        public void Save() => _context.SaveChanges();
    }
}
