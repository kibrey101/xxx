using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Lookup
{
    public interface IFriendLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetFriendsLookup();
    }
}