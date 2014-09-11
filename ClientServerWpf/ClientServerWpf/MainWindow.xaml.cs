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

namespace ClientServerWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SendReceiveViewModel SendReceiveViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SendReceiveViewModel = new SendReceiveViewModel();
            DataContext = SendReceiveViewModel;
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            ((SendReceiveViewModel)DataContext).Start();
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            ((SendReceiveViewModel)DataContext).Stop();
        }

        private void SendClick(object sender, RoutedEventArgs e)
        {
            ((SendReceiveViewModel)DataContext).Send();
        }
    }
}
