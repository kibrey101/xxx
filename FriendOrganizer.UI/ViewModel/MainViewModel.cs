using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private readonly IMessageDialogService _dialogService;
        private IFriendDetailViewModel _friendDetailViewModel;
        public MainViewModel(INavigationViewModel navigationViewModel, 
            Func<IFriendDetailViewModel> friendDetailViewModelCreator, 
            IEventAggregator eventAggregator, IMessageDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            NavigationViewModel = navigationViewModel;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _eventAggregator.GetEvent<SelectedFriendChangedEvent>().Subscribe(OnOpenFriendDetailView);
            _dialogService = dialogService;
            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(OnAfterFriendDeleted);
        }

        public ICommand CreateNewFriendCommand { get; }
        public INavigationViewModel NavigationViewModel { get; }
        public IFriendDetailViewModel FriendDetailViewModel
        {
            get => _friendDetailViewModel;
            private set {
                _friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync() => await NavigationViewModel.LoadAsync();

        private async void OnOpenFriendDetailView(int? friendId)
        {
            if(FriendDetailViewModel?.HasChanges == true)
            {
                var result = _dialogService.ShowOkCancelDialog("You have made changes", "Navigate away?");
                if (result == MessageDialogresult.Cancel) return;
            }

            FriendDetailViewModel = _friendDetailViewModelCreator();
            await FriendDetailViewModel.LoadAsync(friendId);
        }
        private void OnCreateNewFriendExecute() => OnOpenFriendDetailView(null);
        private void OnAfterFriendDeleted(int friendId) => FriendDetailViewModel = null;
    }
}
