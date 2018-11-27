using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.Indexed;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _dialogService;
        private IDetailViewModel _detailViewModel;
        private readonly IIndex<string, IDetailViewModel> _detailViewModelCreator;
        public MainViewModel(INavigationViewModel navigationViewModel,
            IIndex<string, IDetailViewModel> detailViewModelCreator,           
            IEventAggregator eventAggregator, IMessageDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            NavigationViewModel = navigationViewModel;
            _detailViewModelCreator = detailViewModelCreator;
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

            DetailViewModel = _detailViewModelCreator[args.ViewModelName];
            await DetailViewModel.LoadAsync(args.Id);
        }
        private void OnCreateNewDetailExecute(Type viewModelType) => OnOpenDetailView(new OpenDetailViewEventArgs {ViewModelName = viewModelType.Name});
        private void OnAfterDetailDeleted(AfterDetailDeletedEventArgs args) => DetailViewModel = null;
    }
}
