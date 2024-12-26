using AlmazAucceon_3_.AddElement;
using AlmazAucceon_3_.Model2;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace AlmazAucceon_3_.menu
{
    /// <summary>
    /// Логика взаимодействия для MenuAdmin.xaml
    /// </summary>
    public partial class MenuAdmin : Window
    {
        private Staffe _thisStaffe;
        private Staffe _Eplo;
        private AuctionContext dbContext;

        private List<string> sortList = new List<string>
        { 
            "Имя по возрастанию",
            "Фамилия по возрастанию", 
            "Отчество по возрастанию",
            "Номер телефона по возрастанию",
            "Имя по убыванию",
            "Фамилия по убыванию",
            "Отчество по убыванию",
            "Номер телефона по убыванию",
            "id по возрастанию"

        };

        public MenuAdmin(Staffe thisStaffe)
        {
            _Eplo = new Staffe();
            InitializeComponent();
            _thisStaffe = thisStaffe;

            string Names = _thisStaffe.StaffName;
            string famil = _thisStaffe.StaffLastname;
            string Papatronymics = _thisStaffe.StaffPatronymic;

            dbContext = new AuctionContext();

            filterComboBox.ItemsSource = dbContext.Roles.Select(e => e.Title).ToList();
            sortComboBox.ItemsSource = sortList;

            filterComboBox.SelectedIndex = -1;
            sortComboBox.SelectedIndex = 8;
            DisplayUserInfo(famil, Names, Papatronymics);
            MicrSoft();
        }
        private void MicrSoft()
        {

        }
        private void DisplayUserInfo(string lastname, string Name, string Patronomick)
        {
            Dog1.Content = lastname;
            Dog2.Content = Name;
            Dog3.Content = Patronomick;
            Dog4.Content = "Администрторо";
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dbContext.Dispose();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext.Staffes.Include(r => r.IdStaffRoleNavigation).Load();
            UpdateUsersList();
        }

        private void cartButton_Click(object sender, RoutedEventArgs e)
        {
            addCategory addCategory = new addCategory(_thisStaffe);
            addCategory.Show();
            Hide();
        }


        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow authorization = new MainWindow();

            Close();
            authorization.Show();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            filterComboBox.SelectedIndex = -1;
            sortComboBox.SelectedIndex = 8;
            searchTextBox.Text = string.Empty;

            UpdateUsersList();
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PanelRaz_Click(object sender, RoutedEventArgs e)
        {
            addManager adminm = new addManager(_thisStaffe);
            this.Hide();
            adminm.Show();
        }

        private void LotRegkick(object sender, RoutedEventArgs e)
        {
            addLot addlot = new addLot(_thisStaffe);
            addlot.Show();
            Hide();
        }

        private void UpdateUsersList()
        {
            dbContext = new AuctionContext();

            var staffesList = dbContext.Staffes.Include(e => e.IdStaffRoleNavigation).ToList();

            staffesList = ApplyFilter(staffesList);
            staffesList = ApplySort(staffesList);
            staffesList = ApplySearch(staffesList);

            dataGridObject.ItemsSource = null;
            dataGridObject.ItemsSource = staffesList;
        }

        private List<Staffe> ApplyFilter(List<Staffe> staffesList)
        {
            if (filterComboBox.SelectedIndex != -1)
            {
                string nameRole = filterComboBox.SelectedValue.ToString();
                staffesList = staffesList.Where(e => e.IdStaffRoleNavigation.Title == nameRole).ToList();
            }

            return staffesList;
        }

        private List<Staffe> ApplySort(List<Staffe> staffesList)
        {
            int sortIndex = sortComboBox.SelectedIndex;

            if (sortComboBox.SelectedIndex != -1)
            {
                switch (sortIndex)
                {
                    case 0:
                        return staffesList.OrderBy(e => e.StaffName).ToList();
                    case 1:
                        return staffesList.OrderBy(e => e.StaffLastname).ToList();

                    case 2:
                        return staffesList.OrderBy(e => e.StaffPatronymic).ToList();

                    case 3:
                        return staffesList.OrderBy(e => e.PhoneNumber).ToList();

                    case 4:
                        return staffesList.OrderByDescending(e => e.StaffName).ToList();

                    case 5:
                        return staffesList.OrderByDescending(e => e.StaffLastname).ToList();

                    case 6:
                        return staffesList.OrderByDescending(e => e.StaffPatronymic).ToList();

                    case 7:
                        return staffesList.OrderByDescending(e => e.PhoneNumber).ToList();
                    case 8:
                        return staffesList.OrderBy(e => e.IdStaffes).ToList();


                }
            }

            return staffesList;
        }

        private List<Staffe> ApplySearch(List<Staffe> staffesList)
        {
            string searchText = searchTextBox.Text.ToLower();
            if (searchText != string.Empty)
            {
                staffesList = staffesList.Where(r => r.StaffName.ToLower().StartsWith(searchText)
                || r.StaffLastname.ToLower().StartsWith(searchText)).ToList();
            }
            return staffesList;
        }

    }
}



