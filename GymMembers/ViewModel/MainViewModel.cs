using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GymMembers.Model;
using GymMembers.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
        public MainViewModel() 
        {
            members = new ObservableCollection<Member>();
            database = new MemberDB(members);
            members = database.GetMemberships();

            AddCommand = new RelayCommand<IClosable>(AddMethod);
            ExitCommand = new RelayCommand<IClosable>(ExitMethod);
            ChangeCommand = new RelayCommand<IClosable>(ChangeMethod);

            Messenger.Default.Register<MessageMember>(this, ReceiveMember);
            Messenger.Default.Register<NotificationMessage>(this, ReceiveMessage);
        }

        /// <summary>
        /// The command that triggers adding a new member.
        /// </summary>
        public ICommand AddCommand { get; private set; }

        /// <summary>
        /// The command that triggers closing the main window.
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// The command that triggers changing the membership.
        /// </summary>
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
        public void AddMethod(IClosable window)
        {
            AddWindow add = new AddWindow();
            add.Show();
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="window">The window to close.</param>
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
        public void ChangeMethod(IClosable window) 
        {
            if (SelectedMember != null)
            {
                ChangeWindow change = new ChangeWindow();
                change.Show();
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
            if (m.Message == "Update") // From UpdateCommand in ChangeViewModel.UpdateMethod()
            {
                members.RemoveAt(members.IndexOf(SelectedMember));
                members.Add(new Member(m.FirstName, m.LastName, m.Email));
                database.SaveMemberships();
            }
            else if (m.Message == "Add") // From SaveCommand in AddViewModel.SaveMethod()
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
            if (msg.Notification == "Delete") // From DeleteCommand in ChangeViewModel.DeleteMethod()
            {
                members.Remove(SelectedMember);
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