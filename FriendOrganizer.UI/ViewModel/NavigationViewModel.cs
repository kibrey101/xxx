using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.UI.Data.Lookup;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IFriendLookupDataService _lookupDataService;
        private readonly IEventAggregator _eventAggregator;
        public NavigationViewModel(IFriendLookupDataService lookupDataService, IEventAggregator eventAggregator)
        {
            _lookupDataService = lookupDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Subscribe(OnAfterFriendSaved);
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(OnAfterFriendDeleted);
        }

        private void OnAfterFriendDeleted(int friendId)
        {
            var deletedFriend = Friends.FirstOrDefault(f => f.Id == friendId);
            if (deletedFriend == null) return;

            Friends.Remove(deletedFriend);
        }

        private void OnAfterFriendSaved(NavigationItemViewModel obj)
        {

            var friend = Friends.SingleOrDefault(f => f.Id == obj.Id);
            if (friend == null)
                Friends.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator));
            else
                friend.DisplayMember = obj.DisplayMember;
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; set; } = new ObservableCollection<NavigationItemViewModel>();

        public async Task LoadAsync()
        {
            var friends = await _lookupDataService.GetFriendsLookup();

            foreach (var friend in friends)
                Friends.Add(new NavigationItemViewModel(friend.Id, friend.DisplayMember, _eventAggregator));
        }

    }
}
