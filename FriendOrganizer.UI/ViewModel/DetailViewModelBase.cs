using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        protected readonly IEventAggregator _eventAggregator;
        private bool _hasChanges;
        public DetailViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        protected abstract void OnDeleteExecute();
        protected abstract bool OnSaveCanExecute();
        protected abstract void OnSaveExecute();
        public abstract Task LoadAsync(int? id);

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public bool HasChanges
        {
            get => _hasChanges;
            set
            {
                if (_hasChanges == value) return;
                _hasChanges = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        protected virtual void RaiseDetailDeletedEvent(int modelId) => 
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(
                new AfterDetailDeletedEventArgs { Id = modelId, ViewModelName = GetType().Name });

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember) => 
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(
                new AfterDetailSavedEventArgs { Id = modelId, DisplayMember = displayMember, ViewModelName = GetType().Name });
    }
}
