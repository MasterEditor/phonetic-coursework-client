using Coursework_Client.Models;
using HandyControl.Controls;
using HandyControl.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coursework_Client.XAML.Controls
{
    /// <summary>
    /// Interaction logic for FlexList.xaml
    /// </summary>
    public partial class FlexList : UserControl
    {
        public FlexList()
        {
            InitializeComponent();
            var items = new ObservableCollection<object>();
            items.CollectionChanged += (s, e) =>
            {
                OnItemsChanged(s, e);
            };
            Items = items;
        }

        public Collection<object> Items { get; }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(FlexList),
            new PropertyMetadata(default(IEnumerable), OnItemsSourceChanged));


        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FlexList)d).OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }

        protected virtual void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue == null) return;
            foreach(Tariff t in newValue)
            {
                Grid grid = new Grid();
                grid.Width = 210;
                grid.Height = 250;
                grid.Margin = new Thickness(10);

                Border shadow = new Border();
                shadow.Background = new SolidColorBrush(Colors.White);
                shadow.CornerRadius = new CornerRadius(5);
                shadow.Width = 210;
                shadow.Height = 250;
                shadow.Effect = new DropShadowEffect
                {
                    BlurRadius = 12,
                    ShadowDepth = 0,
                    Color = Colors.Black,
                    Opacity = 0.15
                };

                StackPanel container = new StackPanel();

                TextBlock header = new TextBlock();
                header.Text = t.Name;
                header.HorizontalAlignment = HorizontalAlignment.Center;
                header.FontFamily = ResourceHelper.GetResource<FontFamily>("Roboto-Regular");
                header.FontSize = 19;
                header.Margin = new Thickness(0, 10, 0, 0);
                header.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));

                Image image = new Image() { Source = new BitmapImage(new Uri("pack://application:,,,/Coursework_Client;component/Resources/Images/Shape 1.png")) };
                image.Width = 130;
                image.Height = 170;

                StackPanel buttons = new StackPanel();
                buttons.Orientation = Orientation.Horizontal;
                buttons.HorizontalAlignment = HorizontalAlignment.Center;

                Button confirm = new Button();
                confirm.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4E7E7"));
                confirm.Content = new TextBlock() { Text = "Подключить" };
                confirm.BorderThickness = new Thickness(0);
                confirm.FontFamily = ResourceHelper.GetResource<FontFamily>("Roboto-Regular");
                confirm.Width = 125;
                confirm.Margin = new Thickness(0, 0, 3, 0);

                Button info = new Button();
                info.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F3F3F3"));
                info.Content = new TextBlock() { Text = "?" };
                info.BorderThickness = new Thickness(0);
                info.Margin = new Thickness(3, 0, 0, 0);
                info.Width = 40;

                buttons.Children.Add(confirm);
                buttons.Children.Add(info);

                container.Children.Add(header);
                container.Children.Add(image);
                container.Children.Add(buttons);

                grid.Children.Add(shadow);
                grid.Children.Add(container);



                FlexPanel.Children.Add(grid);
            }
        }

        protected virtual void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Refresh();
            UpdateItems();
        }

        protected void Refresh()
        {
            
        }

        protected void UpdateItems()
        {

        }

        //private static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = sender as FlexList;
        //    if (control != null)
        //        control.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        //}

        //private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        //{
        //    // Remove handler for oldValue.CollectionChanged
        //    var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

        //    if (null != oldValueINotifyCollectionChanged)
        //    {
        //        oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
        //    }
        //    // Add handler for newValue.CollectionChanged (if possible)
        //    var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
        //    if (null != newValueINotifyCollectionChanged)
        //    {
        //        newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
        //    }

        //}

        //void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    MessageBox.Show("хуй");
        //}



    }
}
