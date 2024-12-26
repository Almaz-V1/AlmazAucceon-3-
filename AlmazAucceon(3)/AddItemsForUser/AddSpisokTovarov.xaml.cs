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
    /// Логика взаимодействия для AddSpisokTovarov.xaml
    /// </summary>
    public partial class AddSpisokTovarov : Window
    {
        private readonly AuctionContext dbContext;
        private User _selectedUser;
        public AddSpisokTovarov()
        {
            InitializeComponent();
            dbContext = new AuctionContext();

            // Загружаем IdNavigation
            dbContext.Items.Include(r => r.Categotia).Load();

            // Добавляем только те товары и клиентов, которые не имеют клиентов и товаров соответственно
            ItemsCombobox.ItemsSource = dbContext.Items.Where(item => !dbContext.Spisactovars.Any(spisactovar => spisactovar.IdProdyct == item.ItemId)).ToList();
            UserCombobox.ItemsSource = dbContext.Users.Where(user => !dbContext.Spisactovars.Any(spisactovar => spisactovar.IdUser == user.IdUser)).ToList();
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

            // Перебираем выбранные товары и добавляем к пользователю
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
            MessageBox.Show($"Товары для пользователя {_selectedUser.Fullname} успешно добавлены.", "Успешно добавлены товары");
            Close();
        }

        private void UserCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserCombobox != null)
            {
                _selectedUser = UserCombobox.SelectedItem as User;
            }
        }

        private void ItemsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Передаем выбранный товар в ListView
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

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
