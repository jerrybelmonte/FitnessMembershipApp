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
using System.Windows.Navigation;
using System.Windows.Shapes;

// KEIRA: (AddWindow.xaml Pop-Up) Added namespace.
using GymMembers.ViewModel;

namespace GymMembers.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IClosable // KEIRA: (CloseWindowCommand) MainWindow must also implement the IClosable interface.
    {
        public MainWindow()
        {
            InitializeComponent();
            // KEIRA: (AddWindow.xaml Pop-Up) Bind DataContext of MainWindow to MainViewModel.
            this.DataContext = new MainViewModel();
        }
    }
}
