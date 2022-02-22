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
using Coursework_Client.ViewModels;
using Coursework_Client.XAML.Controls;

namespace Coursework_Client.XAML.Views
{
    /// <summary>
    /// Interaction logic for ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ServicesPage()
        {
            InitializeComponent();
            ServicesPageViewModel vm = new ServicesPageViewModel();
            vm.ServicesSourceChanged += OnServicesSourceChanged;
            DataContext = vm;
        }

        MyServicesUserControl MyServicesUserControl = new MyServicesUserControl();

        AllServicesUserControl AllServicesUserControl = new AllServicesUserControl();

        private void OnServicesSourceChanged(object sender, EventArgs e)
        {
            int val = ((IntEventArgs)e).Number;
            

            if(val == 0)
            {
                ServicesSourceControl.Content = MyServicesUserControl;
            }
            else
            {
                ServicesSourceControl.Content = AllServicesUserControl;
            }
        }
    }
}
