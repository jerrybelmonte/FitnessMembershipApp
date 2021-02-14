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

// KEIRA: (ErrorWindow.xaml Pop-Up) Added namespace.
using GymMembers.ViewModel;

namespace GymMembers.View
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
            // KEIRA: (ErrorWindow.xaml Pop-Up) Bind DataContext of AddWindow to AddViewModel.
            this.DataContext = new AddViewModel();
        }
    }
}
