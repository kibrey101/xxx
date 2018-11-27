using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Lookup
{
    public interface IProgrammingLanguagesLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetProgrammingLanguagesLookup();
    }
}