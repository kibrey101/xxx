using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.UI.Data.Lookup;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendRepository _repo;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _dialogService;
        private readonly IProgrammingLanguagesLookupDataService _programmingLanguagesLookupDataService;
        private FriendWrapper _friend;
        private bool _hasChanges;
        private FriendPhoneNumberWrapper _selectedPhoneNumber;
        public FriendDetailViewModel(IFriendRepository repo, 
            IEventAggregator eventAggregator, 
            IMessageDialogService dialogService,
            IProgrammingLanguagesLookupDataService programmingLanguagesLookupDataService)
        {
            _repo = repo;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _programmingLanguagesLookupDataService = programmingLanguagesLookupDataService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);
        }
        private bool OnRemovePhoneNumberCanExecute() => SelectedPhoneNumber != null;
        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            Friend.Model.PhoneNumbers.Remove(SelectedPhoneNumber.Model);
            _repo.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _repo.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = "";
        }

        private void OnDeleteExecute()
        {
            var result = _dialogService.ShowOkCancelDialog("Are you sure you want to delete this friend?", "Delete");
            if (result == MessageDialogresult.Cancel) return;

            _repo.Remove(Friend.Model);            
            _repo.Save();
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
        }

        private void OnSaveExecute()
        {
            _repo.Save();
            HasChanges = _repo.HasChanges();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(new NavigationItemViewModel(Friend.Id, $"{Friend.FirstName} {Friend.LastName}", _eventAggregator));
        }
        private bool OnSaveCanExecute() => Friend != null && !Friend.HasErrors && HasChanges && PhoneNumbers.All(x => !x.HasErrors);
        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue ? await _repo.GetFriendById(friendId.Value) : CreateNewFriend();
            InitializeFriend(friend);
            InitializeFriendPhoneNumbers(friend.PhoneNumbers);
            await LoadProgrammingLanguages();
        }

        private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
                wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged; 
            
            PhoneNumbers.Clear();
            foreach (var number in phoneNumbers)
            {
                var wrapper = new FriendPhoneNumberWrapper(number);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            }
        }
        private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges) HasChanges = _repo.HasChanges();

            if (e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += Friend_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0) Friend.FirstName = "";
        }
        private void Friend_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Friend.HasErrors))
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (!HasChanges) HasChanges = _repo.HasChanges();
        }

        private async Task LoadProgrammingLanguages()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = "-" });
            var languages = await _programmingLanguagesLookupDataService.GetProgrammingLanguagesLookup();
            foreach (var language in languages)
            {
                ProgrammingLanguages.Add(language);
            }
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _repo.Add(friend);
            return friend;
        }

        

        public FriendWrapper Friend
        {
            get => _friend;
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }


        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }   
        
        public FriendPhoneNumberWrapper SelectedPhoneNumber     
        {
            get => _selectedPhoneNumber;
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; set; } = new ObservableCollection<LookupItem>();
        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; set; } = new ObservableCollection<FriendPhoneNumberWrapper>();
        public ICommand SaveCommand { get; set; }   
        public ICommand DeleteCommand { get; set; }
        public ICommand AddPhoneNumberCommand { get; set; }
        public ICommand RemovePhoneNumberCommand { get; set; }  

    }
}
