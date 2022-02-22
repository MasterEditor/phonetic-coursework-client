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
    /// Interaction logic for TariffsPage.xaml
    /// </summary>
    public partial class TariffsPage : Page
    {
        public TariffsPage()
        {
            InitializeComponent();
            var vm = new TariffsPageViewModel();

            vm.TariffAdditionRequiered += OnTariffAdditionRequiered;
            vm.TariffsCleaningRequiered += OnTariffsCleaningRequiered;
            
            DataContext = vm;
        }

        private void OnTariffsCleaningRequiered(object sender, EventArgs e)
        {
            SmartphoneTariffsPanel.Children.Clear();
            CallsTariffsPanel.Children.Clear();
            InternetTariffsPanel.Children.Clear();
            SpecialTariffsPanel.Children.Clear();
        }

        private void OnTariffAdditionRequiered(object sender, EventArgs e)
        {
            var args = (OnAddNewTariffEventArgs)e;

            switch (args.Type)
            {
                case Models.TariffType.ForSmartphone:
                    SmartphoneTariffsPanel.Children.Add(new Tariff { TariffItem = args.Tariff });
                    break;
                case Models.TariffType.ForInternet:
                    InternetTariffsPanel.Children.Add(new Tariff { TariffItem = args.Tariff });
                    break;
                case Models.TariffType.ForCalls:
                    CallsTariffsPanel.Children.Add(new Tariff { TariffItem = args.Tariff });
                    break;
                case Models.TariffType.Special:
                    SpecialTariffsPanel.Children.Add(new Tariff { TariffItem = args.Tariff });
                    break;

                default: break;
            }
        }
    }
}
