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
using Coursework_Client.Managers;

namespace Coursework_Client.XAML.Views
{
    /// <summary>
    /// Interaction logic for MainContentPage.xaml
    /// </summary>
    public partial class MainContentPage : Page
    {
        private NumberInfoPage numberInfoPage;
        private NewsPage newsPage;
        private TariffsPage tariffsPage;
        private PromotionsPage promotionsPage;
        private SettingsPage settingsPage;
        private ServicesPage servicesPage;
        private AdminPanelPage adminPage;


        public MainContentPage()
        {
            InitializeComponent();

            PagesManager.RegisterInnerFrame(InnerFrameView);

            var vm = new MainContentViewModel();
            vm.InnerPageChanged += OnInnerPageChanged;
            DataContext = vm;

            PagesManager.OpenInnerPage(new NumberInfoPage());
        }

        private void OnInnerPageChanged(object sender, EventArgs e)
        {
            switch (((InnerPageChangedEventArgs)e).PageName)
            {
                case "Мой Номер":
                    PagesManager.OpenInnerPage(new NumberInfoPage());
                    //if(numberInfoPage is null)
                    //{
                    //    numberInfoPage = new NumberInfoPage();
                    //    PagesManager.OpenInnerPage(numberInfoPage);
                    //}
                    //else { PagesManager.OpenInnerPage(numberInfoPage); }
                    return;
                case "Новости":
                    PagesManager.OpenInnerPage(new NewsPage());
                    return;
                case "Тарифы":
                    PagesManager.OpenInnerPage(new TariffsPage());
                    return;
                case "Услуги":
                    PagesManager.OpenInnerPage(new ServicesPage());
                    //if (servicesPage is null)
                    //{
                    //    servicesPage = new ServicesPage();
                    //    PagesManager.OpenInnerPage(servicesPage);
                    //}
                    //else { PagesManager.OpenInnerPage(servicesPage); }
                    return;
                case "Акции":
                    PagesManager.OpenInnerPage(new PromotionsPage());
                    return;
                case "Настройки":
                    if (settingsPage is null)
                    {
                        settingsPage = new SettingsPage();
                        PagesManager.OpenInnerPage(settingsPage);
                    }
                    else { PagesManager.OpenInnerPage(settingsPage); }
                    return;
                case "Панель Администратора":
                    PagesManager.OpenInnerPage(new AdminPanelPage());
                    return;
            }
        }


    }
}
