using System;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repository;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMeetingRepository _meetingRepository;
        private MeetingWrapper _meeting;
        public MeetingDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService, 
            IMeetingRepository meetingRepository) : base(eventAggregator)
        {
            _messageDialogService = messageDialogService;
            _meetingRepository = meetingRepository;
        }
        
        public MeetingWrapper Meeting
        {
            get => _meeting;
            private set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int? id)
        {
            var meeting = id.HasValue ? await _meetingRepository.GetByIdAsync(id.Value) : CreateNewMeeting();
            InitializeMeeting(meeting);
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += Meeting_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void Meeting_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges) HasChanges = _meetingRepository.HasChanges();

            if(e.PropertyName == nameof(Meeting.HasErrors))
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        protected async override void OnDeleteExecute()
        {
            if (_messageDialogService.ShowOkCancelDialog("Are you sure you want to delete this meeting?", "Delete?") ==
                MessageDialogresult.Cancel) return;
            _meetingRepository.Remove(Meeting.Model);
            await _meetingRepository.Save();
            RaiseDetailDeletedEvent(Meeting.Id);
        }

        protected override bool OnSaveCanExecute() => Meeting != null && !Meeting.HasErrors && HasChanges;

        protected async override void OnSaveExecute()
        {
            await _meetingRepository.Save();
            HasChanges = _meetingRepository.HasChanges();
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }
    }
}
