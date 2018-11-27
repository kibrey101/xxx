using System.Collections.Generic;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Wrapper;

namespace FriendOrganizer.UI.Data
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {        
        void RemovePhoneNumber(FriendPhoneNumber model);
    }
}