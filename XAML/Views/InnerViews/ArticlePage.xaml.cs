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
using Coursework_Client.Models;

namespace Coursework_Client.XAML.Views
{
    /// <summary>
    /// Interaction logic for ArticlePage.xaml
    /// </summary>
    public partial class ArticlePage : Page
    {
        public ArticlePage(int id)
        {
            InitializeComponent();
            DataContext = new ArticlePageViewModel(id);
        }
    }
}
