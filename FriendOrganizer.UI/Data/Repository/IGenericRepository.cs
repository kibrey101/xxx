using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task Save();
        bool HasChanges();
        void Add(T model);
        void Remove(T model);
    }
}