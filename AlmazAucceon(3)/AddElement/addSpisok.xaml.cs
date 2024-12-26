using AlmazAucceon_3_.Model2;
using AlmazAucceon_3_.menu;
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
                .ToList();
            // Передаем его в датагрид
            dataGridObject.ItemsSource = spisactovarsList;

            // Получаем список ID товаров из Spisactovars для проверки
            var usedProductIds = spisactovarsList.Select(s => s.IdProdyct).ToList();

            // Фильтруем товары, исключая те, которые уже используются в Spisactovars
            var availableItems = dbContext.Items
                .Include(r => r.Categotia)
                .Where(item => !usedProductIds.Contains(item.ItemId)) // Проверка на наличие ID товара в списке
                .ToList();

            // Получаем список ID клиентов из Spisactovars для проверки
            var usedUsersIds = spisactovarsList.Select(s => s.IdUser).ToList();

            // Фильтруем пользователей, исключая тех, которые уже используются в Spisactovars
            var availableUsers = dbContext.Users
                .Include(r => r.IdRoleNavigation)
                .Where(item => !usedUsersIds.Contains(item.IdUser)) // Проверка на наличие ID клиента в списке
                .ToList();

            TavarItemsComboBox.ItemsSource = availableItems;
            UserCombobox.ItemsSource = availableUsers;
        }
        private void Disponsi(string famil, string Names, string Papatronymics)
        {
            Title = "Меню работников" + " " + famil + " " + Names + " " + Papatronymics + " Администратор";
        }
        
        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void IdDojnostBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MenuM menu = new MenuM(_thisStaffe);
            menu.Show();
            Close();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSpisokTovarov = dataGridObject.SelectedItem as Spisactovar;

            UserCombobox.Text = selectedSpisokTovarov.IdUserNavigation.Fullname;
        }

        private void MainTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

           /* // Получаем ссылку на текстовое поле
            var textBox = sender as TextBox;


            if (textBox == NameKategori) // Фамилия, Имя, Отчество
            {
                char inputChar = (char)KeyInterop.VirtualKeyFromKey(e.Key);
                if (!char.IsLetter(inputChar) && inputChar != ' ' && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод неалфавитных символов
                }

                // Проверка длины для имени, фамилии и отчества (20 символов)
                if (textBox.Text.Length >= 25 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }*/
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void TavarItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item item = TavarItemsComboBox.SelectedItem as Item;

            Kategori.Text = item.Categotia.Title;
            CostTextBox.Text = $"{item.CurrentPrice} руб.";
        }

        private void UserTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
