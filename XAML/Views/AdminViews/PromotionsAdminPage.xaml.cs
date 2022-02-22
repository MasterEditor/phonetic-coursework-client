﻿using Coursework_Client.ViewModels;
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

namespace Coursework_Client.XAML.Views
{
    /// <summary>
    /// Interaction logic for PromotionsAdminPage.xaml
    /// </summary>
    public partial class PromotionsAdminPage : Page
    {
        public PromotionsAdminPage()
        {
            InitializeComponent();
            DataContext = new PromotionsAdminViewModel();
        }
    }
}
