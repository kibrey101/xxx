using System.Data.Entity;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.DataAccess;

namespace FriendOrganizer.UI.Data.Repository
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDbContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDbContext context) : base(context)
        {
        }
        public override async Task<Meeting> GetByIdAsync(int id) => 
            await Context.Meetings.Include(m => m.Friends).SingleAsync(m => m.Id == id);
    }
}
