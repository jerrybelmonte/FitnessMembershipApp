using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

// KEIRA: (ChangeWindow.xaml Pop-Up) Added namespace.
using GymMembers.ViewModel;

namespace GymMembers.View
{
    /// <summary>
    /// Interaction logic for ChangeWindow.xaml
    /// </summary>
    public partial class ChangeWindow : Window, IClosable // KEIRA: (ChangeWindow.xaml Pop-Up) ChangeWindow must also implement the IClosable interface.
    {
        public ChangeWindow()
        {
            InitializeComponent();
            // KEIRA: (ChangeWindow.xaml Pop-Up) Bind DataContext of Changeindow to ChangeViewModel.
            this.DataContext = new ChangeViewModel();
        }
    }
}
