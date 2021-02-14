using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GymMembers.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

// REVIEW: (UpdateCommand) Add namespaces.
using GymMembers.View;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Data;

namespace GymMembers.ViewModel
{
    /// <summary>
    /// The VM for modifying or removing users.
    /// </summary>
    public class ChangeViewModel : ViewModelBase
    {
        /// <summary>
        /// The currently entered first name in the change window.
        /// </summary>
        private string enteredFName;

        /// <summary>
        /// The currently entered last name in the change window.
        /// </summary>
        private string enteredLName;

        /// <summary>
        /// The currently entered email in the change window.
        /// </summary>
        private string enteredEmail;

        /// <summary>
        /// Initializes a new instance of the ChangeViewModel class.
        /// </summary>
        public ChangeViewModel()
        {
            // KEIRA: (UpdateCommand) Attach UpdateCommand to UpdateMethod to act as an event.
            UpdateCommand = new RelayCommand<IClosable>(UpdateMethod);
            // KEIRA: (DeleteCommand) Attach DeleteCommand to DeleteMethod to act as an event.
            DeleteCommand = new RelayCommand<IClosable>(DeleteMethod);
            //Messenger.Default.Register<Member>(this,);
        }

        /// <summary>
        /// The command that triggers saving the filled out member data.
        /// </summary>
        public ICommand UpdateCommand { get; private set; }

        /// <summary>
        /// The command that triggers removing the previously selected user.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        /// <summary>
        /// Sends a valid member to the Main VM to replace at the selected index with, then closes the change window.
        /// </summary>
        /// <param name="window">The window to close.</param>
        public void UpdateMethod(IClosable window)
        {
            try
            {
                // TODO Messenger.Default.Send();
                var changeViewModelMessage = new MessageMember(EnteredFName, EnteredLName, EnteredEmail, "Update");
                Messenger.Default.Send(changeViewModelMessage); // sends "Update" message to MainViewModel.ReceiveMember(MessageMember m)
                window.Close();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Fields must be under 25 characters.", "Entry Error");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Fields cannot be empty.", "Entry Error");
            }
            catch (FormatException)
            {
                MessageBox.Show("Must be a valid e-mail address.", "Entry Error");
            }
        }

        /// <summary>
        /// Sends out a message to the initiate closing the change window.
        /// </summary>
        /// <param name="window">The window to close.</param>
        public void DeleteMethod(IClosable window)
        {
            if (window != null)
            {
                //Messenger.Default.Send(); //TODO
                window.Close();
            }
        }

        /// <summary>
        /// Receives a member from the Main VM to auto-fill the change box with the currently selected member.
        /// </summary>
        /// <param name="m">The member data to fill in.</param>
        public void GetSelected(Member m)
        {
            //TODO
        }

        /// <summary>
        /// The currently entered first name in the change window.
        /// </summary>
        public string EnteredFName
        {
            get { return enteredFName; }
            set
            {
                enteredFName = value;
                RaisePropertyChanged("EnteredFName");
            }
        }

        /// <summary>
        /// The currently entered last name in the change window.
        /// </summary>
        public string EnteredLName
        {
            get { return enteredLName; }
            set
            {
                enteredLName = value;
                RaisePropertyChanged("EnteredLName");
            }
        }

        /// <summary>
        /// The currently entered e-mail in the change window.
        /// </summary>
        public string EnteredEmail
        {
            get { return enteredEmail; }
            set
            {
                enteredEmail = value;
                RaisePropertyChanged("EnteredEmail");
            }
        }
    }
}
