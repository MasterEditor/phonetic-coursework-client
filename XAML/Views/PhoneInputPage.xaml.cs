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

namespace Coursework_Client.XAML.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class PhoneInputPage : Page
    {
        public PhoneInputPage()
        {
            InitializeComponent();
            var vm = new PhoneInputViewModel();
            MaskCompleted += vm.OnMaskCompleted;
            DataContext = vm;

        }

        public event EventHandler MaskCompleted;

        private void MaskedPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MaskedPhoneNumber.IsMaskCompleted)
            {
                MaskCompleted.Invoke(null, new MaskedPhoneNumberEventArgs() { Number = MaskedPhoneNumber.Text});
            }
        }
    }
}
