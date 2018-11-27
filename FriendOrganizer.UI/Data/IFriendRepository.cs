using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Wrapper;

namespace FriendOrganizer.UI.Data
{
    public interface IFriendRepository
    {
        Task<Friend> GetFriendById(int id);
        void Save();
        bool HasChanges();
        void Add(Friend friend);
        void Remove(Friend model);
        Task<IEnumerable<ProgrammingLanguage>> GetProgrammingLanguagesAsync();
        void RemovePhoneNumber(FriendPhoneNumber model);
    }
}