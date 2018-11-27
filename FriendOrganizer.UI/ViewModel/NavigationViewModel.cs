using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.UI.Data.Lookup;
using System;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IFriendLookupDataService _friendLookupDataService;
        private readonly IMeetingLookupDataService _meetingLookupDataService;
        private readonly IEventAggregator _eventAggregator;
        public NavigationViewModel(IFriendLookupDataService friendLookupDataService,
            IMeetingLookupDataService meetingLookupDataService, IEventAggregator eventAggregator)
        {
            _friendLookupDataService = friendLookupDataService;
            _meetingLookupDataService = meetingLookupDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(OnAfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(OnAfterDetailDeleted);
        }

        private void OnAfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailDeleted(Friends, args);
                    break;

                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                    break;
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var deletedItem = items.FirstOrDefault(f => f.Id == args.Id);
            if (deletedItem == null) return;

            items.Remove(deletedItem);
        }

        private void OnAfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailSaved(Friends, args);
                    break;

                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, args);
                    break;
            }
            
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item == null)
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, args.ViewModelName, _eventAggregator));
            else
                item.DisplayMember = args.DisplayMember;
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; set; } = new ObservableCollection<NavigationItemViewModel>();
        public ObservableCollection<NavigationItemViewModel> Meetings { get; set; } = new ObservableCollection<NavigationItemViewModel>();
        public async Task LoadAsync()
        {
            var friends = await _friendLookupDataService.GetFriendsLookup();
            Friends.Clear();

            foreach (var friend in friends)
                Friends.Add(new NavigationItemViewModel(friend.Id, friend.DisplayMember, nameof(FriendDetailViewModel), _eventAggregator));

            var meetings = await _meetingLookupDataService.GetMeetingsLookup();
            Meetings.Clear();

            foreach (var meeting in meetings)
                Meetings.Add(new NavigationItemViewModel(meeting.Id, meeting.DisplayMember, nameof(MeetingDetailViewModel), _eventAggregator));
        }

    }
}
