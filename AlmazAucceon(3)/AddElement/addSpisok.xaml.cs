using AlmazAucceon_3_.Model2;
using AlmazAucceon_3_.menu;
using AlmazAucceon_3_.AddItemsForUser;
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

namespace AlmazAucceon_3_.AddElement
{
    /// <summary>
    /// Логика взаимодействия для addSpisok.xaml
    /// </summary>
    public partial class addSpisok : Window
    {
        private AuctionContext dbContext;
        private Staffe satrudneke;
        private Staffe _thisStaffe;
        private Staffe Lotes;
        private List<string> sortList = new List<string>
        {
            "Имя по возрастанию",
            "Фамилия по возрастанию",
            "Имя по убыванию",
            "Фамилия по убыванию",
            "Отчество по убыванию",
            "Номер телефона по убыванию"

        };

        private Spisactovar? _selectedSpisokTovarov;
        public addSpisok(Staffe thisStaffe)
        {
            InitializeComponent();
            dbContext = new AuctionContext();
            _thisStaffe = thisStaffe;
            string Names = _thisStaffe.StaffName;
            string famil = _thisStaffe.StaffLastname;
            string Papatronymics = _thisStaffe.StaffPatronymic;
            Disponsi(famil, Names, Papatronymics);

            // Инициализируем список всех записей клиент-товар
            var spisactovarsList = dbContext.Spisactovars
                .Include(r => r.IdUserNavigation)
                .Include(r => r.IdProdyctNavigation)
                .ThenInclude(t => t.Categotia)
                .ToList()
                .GroupBy(x => x.IdUser)
                .Select(g => g.First())
                .ToList();
            // Передаем его в датагрид
            dataGridObject.ItemsSource = spisactovarsList;
        }

        private void UpdateDataGridObjects()
        {
            dbContext = new();

            var spisactovarsList = dbContext.Spisactovars
                .Include(r => r.IdUserNavigation)
                .Include(r => r.IdProdyctNavigation)
                .ThenInclude(t => t.Categotia)
                .ToList()
                .GroupBy(x => x.IdUser)
                .Select(g => g.First())
                .ToList();

            // Здесь добавить логику для фильтации
            // Здесь добавить логику для сортировки
            // Здесь добавить логику для поиска

            dataGridObject.ItemsSource = spisactovarsList;
        }

        private void ClearFiels()
        {
            ItemsListView.Items.Clear();
            _selectedSpisokTovarov = null;
        }

        private void Disponsi(string famil, string Names, string Papatronymics)
        {
            Title = "Меню работников" + " " + famil + " " + Names + " " + Papatronymics + " Администратор";
        }
        
        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика сортировки
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика фильтрации
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика сброса фильтров
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Логика поиска
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddSpisokTovarov window = new();
            window.ShowDialog();

            ClearFiels();
            UpdateDataGridObjects();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSpisokTovarov != null)
            {
                var result = MessageBox.Show($"Вы точно хотите удалить товары у пользователя {_selectedSpisokTovarov.IdUserNavigation.Fullname}?",
                    "Удалить товары?",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Инициализируем список всех клиентов-товаров
                    List<Spisactovar> listItemsUsers = dbContext.Spisactovars
                        .Include(r => r.IdProdyctNavigation)
                        .ToList();

                    // Удаляем все товары, прикрепленные к пользователю
                    foreach (var userItem in listItemsUsers)
                    {
                        if (userItem.IdUser == _selectedSpisokTovarov.IdUser)
                        {
                            dbContext.Remove(userItem);
                        }
                    }
                    dbContext.SaveChanges();
                    MessageBox.Show($"Товары пользователя {_selectedSpisokTovarov.IdUserNavigation.Fullname} успешно удалены.", "Успешно удалены товары");

                    ClearFiels();
                    UpdateDataGridObjects();  
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите пользователя.", "Ошибка");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSpisokTovarov != null)
            {
                EditSpisokTovarov window = new(_selectedSpisokTovarov);
                window.ShowDialog();

                ClearFiels();
                UpdateDataGridObjects();
            }
            else
            {
                MessageBox.Show("Сначала выберите пользователя.", "Ошибка");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MenuM menu = new MenuM(_thisStaffe);
            menu.Show();
            Close();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSpisokTovarov = dataGridObject.SelectedItem as Spisactovar;
            if (_selectedSpisokTovarov == null ) { return; } // Если вдруг выбранный объект null

            List<Spisactovar> listItemsUsers = dbContext.Spisactovars // Инициализируем список всех клиентов-товаров
                .Include(r => r.IdProdyctNavigation)
                .ToList();

            ItemsListView.Items.Clear();
            foreach (var itemUser in listItemsUsers)
            {
                if (_selectedSpisokTovarov.IdUser == itemUser.IdUser) // Если совпадает ID пользователей
                {
                    ItemsListView.Items.Add(itemUser.IdProdyctNavigation.ItemNameCostCategory); // Добавляем его услуги в ListView
                }
            }
            UserCombobox.Text = _selectedSpisokTovarov.IdUserNavigation.Fullname;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
