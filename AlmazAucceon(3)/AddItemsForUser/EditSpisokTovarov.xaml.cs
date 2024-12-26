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

namespace AlmazAucceon_3_.AddItemsForUser
{
    /// <summary>
    /// Логика взаимодействия для EditSpisokTovarov.xaml
    /// </summary>
    public partial class EditSpisokTovarov : Window
    {
        private AuctionContext dbContext;
        private User _selectedUser;
        public EditSpisokTovarov(Spisactovar spisactovar)
        {
            InitializeComponent();
            dbContext = new AuctionContext();

            // Загружаем IdNavigation
            dbContext.Items.Include(r => r.Categotia).Load();
            dbContext.Users.Include(r => r.IdRoleNavigation).Load();

            // Находим выбранного пользователя
            var user = dbContext.Users.Single(u => u.IdUser == spisactovar.IdUser);
            _selectedUser = user;

            // Добавляем только те товары, которые не имеют клиентов или привязаны к этому клиенту
            ItemsCombobox.ItemsSource = dbContext.Items
                .Where(item => !dbContext.Spisactovars
                .Any(spisactovar => spisactovar.IdProdyct == item.ItemId && spisactovar.IdUser != _selectedUser.IdUser))
                .ToList();

            // Инициализируем список всех клиентов-товаров
            List<Spisactovar> ListItemsUsers = dbContext.Spisactovars
                .Include(r => r.IdUserNavigation)
                .Include(r => r.IdProdyctNavigation)
                .ThenInclude(t => t.Categotia)
                .ToList();

            // Заполняем ListView
            foreach (var itemUser in ListItemsUsers)
            {
                if (_selectedUser.IdUser == itemUser.IdUser) // Если совпадает ID пользователей
                {
                    var item = dbContext.Items.Single(r => r.ItemId ==  itemUser.IdProdyct);
                    ItemsListView.Items.Add(item); // Добавляем его услуги в ListView
                }
            }

            // Передаем фио пользователя в текстбокс
            UserTextBox.Text = _selectedUser.Fullname;
        }

        private void ItemsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Перебираем выбранный товар в ListView
            if (ItemsCombobox.SelectedIndex != -1)
            {
                var item = ItemsCombobox.SelectedItem as Item;
                if (!ItemsListView.Items.Contains(item)) // Если такого товара не добавлено к клиенту, то добавляем
                {
                    ItemsListView.Items.Add(item);
                }
                ItemsCombobox.SelectedIndex = -1;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Очищаем список
            ItemsListView.Items.Clear();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Добавить ограничения на то, чтобы обязательно был выбран клиент
            // Добавить ограничения на то, чтобы обязательно был выбран ХОТЯ БЫ 1 товар

            // Инициализируем список всех клиентов-товаров
            List<Spisactovar> listItemsUsers = dbContext.Spisactovars
                .Include(r => r.IdProdyctNavigation)
                .ToList();

            // Удаляем все товары, прикрепленные к пользователю,
            // чтобы потом прикрепить заново 
            foreach (var userItem in listItemsUsers)
            {
                if (userItem.IdUser == _selectedUser.IdUser)
                {
                    dbContext.Remove(userItem);
                }
            }

            // Добавляем новые товары к пользователю
            foreach (var item in ItemsListView.Items)
            {
                var selectItem = item as Item;
                Spisactovar spisactovar = new Spisactovar()
                {
                    IdUser = _selectedUser.IdUser,
                    IdProdyct = selectItem.ItemId
                };
                dbContext.Add(spisactovar);
            }
            dbContext.SaveChanges();
            MessageBox.Show($"Товары для пользователя {_selectedUser.Fullname} успешно изменены.", "Успешно изменены товары");
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
