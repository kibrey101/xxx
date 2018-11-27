using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDbContext>, IFriendRepository
    {
        public FriendRepository(FriendOrganizerDbContext context) : base(context) { }
        public override async Task<Friend> GetByIdAsync(int id) => await Context.Friends.Include(x => x.PhoneNumbers).FirstOrDefaultAsync(x => x.Id == id);
        public void RemovePhoneNumber(FriendPhoneNumber model) => Context.FriendPhoneNumbers.Remove(model);
    }
}
