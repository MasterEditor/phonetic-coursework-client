using Coursework_Client.ViewModels;
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

namespace Coursework_Client.XAML.Controls
{
    /// <summary>
    /// Interaction logic for Tariff.xaml
    /// </summary>
    public partial class Tariff : UserControl
    {
        private static BitmapImage ForSmartphoneImage;
        private static BitmapImage ForInternetImage;
        private static BitmapImage ForCallsImage;
        private static BitmapImage ForSpecialsImage;

        public BitmapImage Image { get; set; }

        static Tariff()
        {
            ForSmartphoneImage = new BitmapImage(new Uri("../../Resources/Images/ForSmartphone.png", UriKind.Relative));
            ForInternetImage = new BitmapImage(new Uri("../../Resources/Images/ForInternet.png", UriKind.Relative));
            ForCallsImage = new BitmapImage(new Uri("../../Resources/Images/ForCalls.png", UriKind.Relative));
            ForSpecialsImage = new BitmapImage(new Uri("../../Resources/Images/ForSpecials.png", UriKind.Relative));
        }

        public Tariff()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
        }

        public static readonly DependencyProperty TariffProperty = DependencyProperty.Register(
            "TariffItem", typeof(Models.Tariff), typeof(Tariff));

        public Models.Tariff TariffItem
        {
            get => (Models.Tariff)GetValue(TariffProperty);
            set
            {
                switch (value.Type)
                {
                    case Models.TariffType.ForSmartphone:
                        Image = ForSmartphoneImage;
                        break;
                    case Models.TariffType.ForInternet:
                        Image = ForInternetImage;
                        break;
                    case Models.TariffType.ForCalls:
                        Image = ForCallsImage;
                        break;
                    case Models.TariffType.Special:
                        Image = ForSpecialsImage;
                        break;
                }

                SetValue(TariffProperty, value);
                if (value.Type == Models.TariffType.ForSmartphone) Image = ForSmartphoneImage;
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as TariffsPageViewModel).GetTariffInfo.Execute(TariffItem);
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as TariffsPageViewModel).OpenChangeTariffDialogCommand.Execute(TariffItem);
        }
    }
}
