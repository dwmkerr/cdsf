using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CompDataSvcUtil
{
    /// <summary>
    /// Interaction logic for DataServiceView.xaml
    /// </summary>
    public partial class DataServiceView : UserControl
    {
        public DataServiceView()
        {
            InitializeComponent();
        }
        
        private static readonly DependencyProperty DataServiceProperty =
          DependencyProperty.Register("DataService", typeof(DataServiceViewModel), typeof(DataServiceView),
          new PropertyMetadata(null, new PropertyChangedCallback(OnDataServiceChanged)));

        public DataServiceViewModel DataService
        {
            get { return (DataServiceViewModel)GetValue(DataServiceProperty); }
            set { SetValue(DataServiceProperty, value); }
        }

        private static void OnDataServiceChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            DataServiceView me = o as DataServiceView;
        }
                
    }
}
