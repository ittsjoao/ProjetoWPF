using System;
using System.Collections.Generic;
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

namespace ProjetoWPF
{
    /// <summary>
    /// Interação lógica para HomePage.xam
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            NotasFrame.Navigate(new NotasPage());
        }

        private void Btn_Terno(object sender, RoutedEventArgs e)
        {
            NotasFrame.Navigate(new TernoControl());
        }

        private void Btn_Vestido(object sender, RoutedEventArgs e)
        {
            NotasFrame.Navigate(new VestidoControl());
        }
    }
}
