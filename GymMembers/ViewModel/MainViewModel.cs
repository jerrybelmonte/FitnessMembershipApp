using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GymMembers.Model;
using GymMembers.View;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

// KEIRA: (AddWindow.xaml Pop-Up) Add namespaces.
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Data;

namespace GymMembers.ViewModel
{
    /// <summary>
    /// The VM for the main screen that shows the member list.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The list of registered members.
        /// </summary>
        private ObservableCollection<Member> members;

        /// <summary>
        /// The currently selected member.
        /// </summary>
        private Member selectedMember;

        /// <summary>
        /// The database that keeps track of saving and reading the registered members.
        /// </summary>
        private MemberDB database;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel() //TODO: MainViewModel() constructor
        {
            members = new ObservableCollection<Member>();
            database = new MemberDB(members); // dependency injection of the members list into the database instance
            members = database.GetMemberships();

            // KEIRA: (AddWindow.xaml Pop-Up) Attach AddCommand to AddMethod to act as an event.
            AddCommand = new RelayCommand<IClosable>(AddMethod);
            // KEIRA: (ExitCommand) Attach ExitCommand to ExitMethod() to act as an event. 
            this.ExitCommand = new RelayCommand<IClosable>(this.ExitMethod);
            // KEIRA: (ChangeWindow Pop-Up) Attach ChangeCommand to ChangeMethod to act as an event.
            ChangeCommand = new RelayCommand<IClosable>(ChangeMethod);

            Messenger.Default.Register<MessageMember>(this, ReceiveMember);
            Messenger.Default.Register<NotificationMessage>(this, ReceiveMessage);
        }

        /// <summary>
        /// The command that triggers adding a new member.
        /// </summary>
        /// 
        // KEIRA: (AddWindow.xaml) Add AddCommand.
        public ICommand AddCommand { get; private set; }

        /// <summary>
        /// The command that triggers closing the main window.
        /// </summary>
        /// 
        // KEIRA: (ExitCommand) Add ExitCommand.
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// The command that triggers changing the membership.
        /// </summary>
        /// 
        // KEIRA: (ChangeCommand) Add ChangeCommand.
        public ICommand ChangeCommand { get; private set; }

        /// <summary>
        /// The currently selected member in the list box.
        /// </summary>
        public Member SelectedMember 
        {
            get { return selectedMember; }
            set
            {
                selectedMember = value;
                RaisePropertyChanged("SelectedMember");
            }
        }

        /// <summary>
        /// Shows a new add screen.
        /// </summary>
        ///
        // KEIRA: (AddWindow.xaml Pop-Up) Add AddMethod(). 
        public void AddMethod(IClosable window) // KEIRA: (Needs IClosable as a parameter to match RelayCommand/delegate signature.)
        {
            AddWindow add = new AddWindow();
            add.Show();
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="window">The window to close.</param>
        ///
        // KEIRA: (ExitCommand) Add ExitMethod().
        public void ExitMethod(IClosable window) 
        {
            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Opens the change window.
        /// </summary>
        /// 
        // KEIRA: (ChangeWindow Pop-Up) Add ChangeMethod().
        public void ChangeMethod(IClosable window) // KEIRA: (Needs IClosable as a parameter to match RelayCommand/delegate signature.)
        {
            if (SelectedMember != null)
            {
                ChangeWindow change = new ChangeWindow();
                change.Show();
                // TODO Send selectedMember to ChangeViewModel; ChangeViewModel must receive the SelectedMember from MainViewModel in order to auto-fill input boxes.
                Messenger.Default.Send(SelectedMember);
            }
        }
        
        /// <summary>
        /// Gets a new member for the list.
        /// </summary>
        /// <param name="m">The member to add. The message denotes how it is added.
        /// "Update" replaces at the specified index, "Add" adds it to the list.</param>
        public void ReceiveMember(MessageMember m)
        {
            if (m.Message == "Update")
            {
                // KEIRA: Replace Member at index of selectedMember.
                members[members.IndexOf(selectedMember)] = m;
                // KEIRA: Update Membership database.
                database.SaveMemberships();
            }
            else if (m.Message == "Add")
            {
                members.Add(new Member(m.FirstName, m.LastName, m.Email));
                database.SaveMemberships();
            }
        }
        
        /// <summary>
        /// Gets text messages.
        /// </summary>
        /// <param name="msg">The received message. "Delete" means the currently selected member is deleted.</param>
        public void ReceiveMessage(NotificationMessage msg) 
        { 
            if (msg.Notification == "Delete")
            {
                // KEIRA "Delete" the currently selectedMember from the list of Members ("members")
                members.RemoveAt(members.IndexOf(selectedMember));
                database.SaveMemberships();
            }
        }

        /// <summary>
        /// The list of registered members.
        /// </summary>
        public ObservableCollection<Member> MemberList 
        { 
            get { return members; }
        }
    }
}