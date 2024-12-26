using AlmazAucceon_3_.AddElement;
using AlmazAucceon_3_.Model2;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace AlmazAucceon_3_.menu
{
    /// <summary>
    /// Логика взаимодействия для MenuByer.xaml
    /// </summary>
    public partial class MenuByer : Window
    {
        private Staffe _thisStaffe;
        private AuctionContext dbContext;
        public MenuByer(Staffe thisStaffe)
        {
            InitializeComponent();
            dbContext = new AuctionContext();
            _thisStaffe = thisStaffe;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dbContext.Dispose();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext.Users.Load();
            dataGridObject2.ItemsSource = dbContext.Users.Local.ToList();

        }

        private void cartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow authorization = new MainWindow();

            Close();
            authorization.Show();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PanelRaz_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow adminm = new AddUserWindow(_thisStaffe);
            this.Hide();
            adminm.Show();
        }
    }
}
