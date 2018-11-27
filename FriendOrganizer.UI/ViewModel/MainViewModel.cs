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
        private readonly Func<IMeetingDetailViewModel> _meetingDetailViewModelCreator;
        private readonly IMessageDialogService _dialogService;
        private IDetailViewModel _detailViewModel;
        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            Func<IMeetingDetailViewModel> meetingDetailViewModelCreator,
            IEventAggregator eventAggregator, IMessageDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            NavigationViewModel = navigationViewModel;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _meetingDetailViewModelCreator = meetingDetailViewModelCreator;
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpenDetailView);
            _dialogService = dialogService;
            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(OnAfterDetailDeleted);
        }

        public ICommand CreateNewDetailCommand { get; }
        public INavigationViewModel NavigationViewModel { get; }
        public IDetailViewModel DetailViewModel
        {
            get => _detailViewModel;
            private set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync() => await NavigationViewModel.LoadAsync();

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel?.HasChanges == true)
            {
                var result = _dialogService.ShowOkCancelDialog("You have made changes", "Navigate away?");
                if (result == MessageDialogresult.Cancel) return;
            }
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    DetailViewModel = _friendDetailViewModelCreator();
                    break;
                case nameof(MeetingDetailViewModel):
                    DetailViewModel = _meetingDetailViewModelCreator();
                    break;
                default:
                    throw new Exception($"ViewModel {args.ViewModelName} not mapped");
            }

            await DetailViewModel.LoadAsync(args.Id);
        }
        private void OnCreateNewDetailExecute(Type viewModelType) => OnOpenDetailView(new OpenDetailViewEventArgs {ViewModelName = viewModelType.Name});
        private void OnAfterDetailDeleted(AfterDetailDeletedEventArgs args) => DetailViewModel = null;
    }
}
