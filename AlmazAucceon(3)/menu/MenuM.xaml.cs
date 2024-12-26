
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
    /// Логика взаимодействия для MenuM.xaml
    /// </summary>
    /// 

    public partial class MenuM : Window
    {
        private List<string> sortList = new List<string>
        {
            "Имя по возрастанию",
            "Фамилия по возрастанию",
            "Отчество по возрастанию",
            "Номер телефона по возрастанию",
            "Имя по убыванию",
            "Фамилия по убыванию",
            "Отчество по убыванию",
            "Номер телефона по убыванию"

        };
        private Staffe _thisStaffe;
        private AuctionContext dbContext;
        public MenuM(Staffe thisStaffe)
        {
            InitializeComponent();
            _thisStaffe = thisStaffe;
            string Names = _thisStaffe.StaffName;
            string famil = _thisStaffe.StaffLastname;
            string Papatronymics = _thisStaffe.StaffPatronymic;

            DisplayUserInfo(famil, Names, Papatronymics);
            UpdateDataGrid();

            _thisStaffe = thisStaffe;
            sortComboBox.ItemsSource = sortList;
            sortComboBox.SelectedIndex = -1;
        }
        private void DisplayUserInfo(string lastname, string Name, string Patronomick)
        {
            Dog1.Content = lastname;
            Dog2.Content = Name;
            Dog3.Content = Patronomick;
            Dog4.Content = "Менеджер";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dbContext.Dispose();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            UpdateUsersList();
        }
        private void UpdateDataGrid()
        {
            dbContext = new AuctionContext();
            var users = dbContext.Users.Include(r => r.IdRoleNavigation).ToList();

            /* users = ApplyFilter(users);
             users = ApplySort(users);
             users = ApplySearch(users);*/
            dataGridObject.ItemsSource = null;

            dataGridObject.ItemsSource = users;

        }
       
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsersList();
        }
        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (dataGridObject.ItemsSource != null)
                UpdateDataGrid();*/
        }
        private void cartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            addSpisok addspisok = new addSpisok(_thisStaffe);
            addspisok.Show();
            Hide();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow authorization = new MainWindow();

            Close();
            authorization.Show();
        }



        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            sortComboBox.SelectedIndex = -1;
            searchTextBox.Text = string.Empty;

            UpdateUsersList();
        }
        private void UpdateUsersList()
        {
            dbContext = new AuctionContext();
            var staffesList = dbContext.Users.Include(r => r.IdRoleNavigation).ToList();
            staffesList = ApplySort(staffesList);
            staffesList = ApplySearch(staffesList);

            dataGridObject.ItemsSource = null;
            dataGridObject.ItemsSource = staffesList;
        }
        private List<User> ApplySort(List<User> staffesList)
        {
            int sortIndex = sortComboBox.SelectedIndex;

            if (sortComboBox.SelectedIndex != -1)
            {
                switch (sortIndex)
                {
                    case 0:
                        return staffesList.OrderBy(e => e.UserName).ToList();


                    case 1:
                        return staffesList.OrderBy(e => e.Lastname).ToList();

                    case 2:
                        return staffesList.OrderBy(e => e.Patronymic).ToList();

                    case 3:
                        return staffesList.OrderBy(e => e.PhoneNumber).ToList();

                    case 4:
                        return staffesList.OrderByDescending(e => e.UserName).ToList();

                    case 5:
                        return staffesList.OrderByDescending(e => e.Lastname).ToList();

                    case 6:
                        return staffesList.OrderByDescending(e => e.Patronymic).ToList();

                    case 7:
                        return staffesList.OrderByDescending(e => e.PhoneNumber).ToList();


                }
            }

            return staffesList;
        }

        private List<User> ApplySearch(List<User> staffesList)
        {
            string searchText = searchTextBox.Text.ToLower();
            if (searchText != string.Empty)
            {
                staffesList = staffesList.Where(r => r.UserName.ToLower().StartsWith(searchText)
                || r.Lastname.ToLower().StartsWith(searchText)).ToList();
            }
            return staffesList;
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

