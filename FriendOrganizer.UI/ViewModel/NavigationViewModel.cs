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
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(OnAfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(OnAfterDetailDeleted);
        }

        private void OnAfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    var deletedFriend = Friends.FirstOrDefault(f => f.Id == args.Id);
                    if (deletedFriend == null) return;

                    Friends.Remove(deletedFriend);
                    break;
            }
        }

        private void OnAfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    var friend = Friends.SingleOrDefault(f => f.Id == args.Id);
                    if (friend == null)
                        Friends.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, nameof(FriendDetailViewModel), _eventAggregator));
                    else
                        friend.DisplayMember = args.DisplayMember;
                    break;
            }
            
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; set; } = new ObservableCollection<NavigationItemViewModel>();

        public async Task LoadAsync()
        {
            var friends = await _lookupDataService.GetFriendsLookup();

            foreach (var friend in friends)
                Friends.Add(new NavigationItemViewModel(friend.Id, friend.DisplayMember, nameof(FriendDetailViewModel), _eventAggregator));
        }

    }
}
