﻿using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        private readonly IEventAggregator _eventAggregator;

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Id = id;
            DisplayMember = displayMember;
            OpenFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailViewCommand);
        }

        public int Id { get; }      
        public string DisplayMember
        {
            get => _displayMember;
            set {
                _displayMember = value;
                OnPropertyChanged();
            }
        }   
        public ICommand OpenFriendDetailViewCommand { get; set; }
        private void OnOpenFriendDetailViewCommand()
        {
            _eventAggregator.GetEvent<SelectedFriendChangedEvent>().Publish(Id);
        }
    }
}