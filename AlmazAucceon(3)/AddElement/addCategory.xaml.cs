using AlmazAucceon_3_.menu;
using AlmazAucceon_3_.Model2;
using AlmazAucceon_3_;
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
using System.Collections;

namespace AlmazAucceon_3_.AddElement
{
    /// <summary>
    /// Логика взаимодействия для addCategory.xaml
    /// </summary>
    public partial class addCategory : Window
    {
        string idSTEx;
        private List<string> sortList = new List<string>
        { 
            "Id по возрастанию",
            "Название по возрастанию",
            "Id по убыванию",
            "Название по убыванию",

        };
        private AuctionContext dbContext;
        private Staffe _thisStaffe;

        private Category сategory;
        public addCategory(Staffe thisStaffe)
        {
            InitializeComponent();
            
            dbContext = new AuctionContext();
            _thisStaffe = thisStaffe;
            string Names = _thisStaffe.StaffName;
            string famil = _thisStaffe.StaffLastname;
            string Papatronymics = _thisStaffe.StaffPatronymic;
            sortComboBox.ItemsSource = sortList;
            sortComboBox.SelectedIndex = 0;
            Disponsi(famil, Names, Papatronymics);
        }
        private void Disponsi(string famil, string Names, string Papatronymics)
        {
            Title = "Меню категорий" + " " + famil + " " + Names + " " + Papatronymics + " Администратор";
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext.Categories.Load();
            dataGridObject.ItemsSource = dbContext.Categories.Local.ToBindingList();
          
            UpdateUsersList();

        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            sortComboBox.SelectedIndex = 0;
            searchTextBox.Text = string.Empty;

            UpdateUsersList();
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsersList();


        }

        private void UpdateUsersList()
        {
            dbContext = new AuctionContext();

            var staffesList = dbContext.Categories.ToList();

            staffesList = ApplySort(staffesList);
            staffesList = ApplySearch(staffesList);
 
            dataGridObject.ItemsSource = null;
            dataGridObject.ItemsSource = staffesList;
            int itemCount = dataGridObject.Items.Count;
            CountLabel.Content = $"{itemCount}";
        }
        private List<Category> ApplySort(List<Category> staffesList)
        {
            int sortIndex = sortComboBox.SelectedIndex;

            if (sortComboBox.SelectedIndex != -1)
            {
                switch (sortIndex)
                {   
                    case 0:
                        return staffesList.OrderBy(e => e.IdCategories).ToList();
                    case 1:
                        return staffesList.OrderBy(p => p.Title).ToList();
                    case 2:
                        return staffesList.OrderByDescending(e => e.IdCategories).ToList();
                    case 3:
                        return staffesList.OrderByDescending(m => m.Title).ToList();


                }
            }

            return staffesList;
        }

        private List<Category> ApplySearch(List<Category> staffesList)
        {
            string searchText = searchTextBox.Text.ToLower();
            if (searchText != string.Empty)
            {
                staffesList = staffesList.Where(r => r.Title.ToLower().StartsWith(searchText)).ToList();
            }
            return staffesList;
        }
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeletProverka();
        }
        private void DeletProverka()
        {
            if (сategory == null)
            {
                MessageBox.Show("Выберите категорию для удаления.");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить данную категорию?", "Подтверждение удаление", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                dbContext.Categories.Remove(сategory);
                dbContext.SaveChanges();

                UpdateUsersList();
                MessageBox.Show("Удаление прошло успешно", "Сообщение");
            }
            else
            {
                return;
            }
         
        }
        private void DeletCategoria()
        {
     
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddProverka();
        }
        private void AddProverka()
        {
           

            if (string.IsNullOrWhiteSpace(NameKategori.Text))
            {
                MessageBox.Show("Пожалуйста, введите Название категории.");
                return; // Exit the method to prevent further processing
            }

            else
            {

                AddUser();
            }

        }


        private void AddUser()
        {
            string Kategoria = NameKategori.Text;


            if (!AreAllValidChars5(Kategoria))
            {
                MessageBox.Show("Наименование категории содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                NameKategori.Clear(); // Очищаем TextBox
                return;
            }
            string Titles = NameKategori.Text.Trim().ToLower();
            Category user1 = App.context.Categories.ToList().Find(u => u.Title.Trim().ToLower() == Titles);
            if (string.IsNullOrWhiteSpace(NameKategori.Text))
            {
                MessageBox.Show("Пожалуйста, введите Название категории.");
                return; // Exit the method to prevent further processing
            }

            else if (user1 != null)
            {
                MessageBox.Show(" Уже есть такая категория");
                return;
            }
            else
            {
                string Kategori2 = NameKategori.Text;
                var newSatrudnik2 = new Category { Title = Kategori2 };

                dbContext.Categories.Add(newSatrudnik2);
                dbContext.SaveChanges();
                NameKategori.Clear();
         
                UpdateUsersList();
                MessageBox.Show("Дабавление просло успешно", "Сообщение");
            }


            

        }

        private bool AreAllValidChars5(string text)
        {

            foreach (char c in text)
            {
                if (!char.IsLetter(c) && c != ' ' || NameKategori.Text.Length > 25)
                {

                    return false;

                }
            }
            return true; // Все символы допустимы

        }




        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

            EditButtonProveka();

        }
        private void EditButtonProveka()
        {
            string Titles = NameKategori.Text.Trim().ToLower();
            Category user1 = App.context.Categories.ToList().Find(u => u.Title.Trim().ToLower() == Titles && u.IdCategories != Convert.ToUInt32(idSTEx));
            if (string.IsNullOrWhiteSpace(NameKategori.Text))
            {
                MessageBox.Show("Пожалуйста, введите Название категории.");
                return; // Exit the method to prevent further processing
            }

            else if (user1 != null)
            {
                MessageBox.Show(" Уже есть такая категория");
                return;
            }


            else
            {

                AddChange();
            }
        }
        private void AddChange()
        {

            string name = NameKategori.Text;


            if (!AreAllValidChars5(name))
            {
                MessageBox.Show("Наименование категории содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                NameKategori.Clear(); // Очищаем TextBox
                return;
            }

            else
            {
                сategory.Title = name;
                dbContext.SaveChanges();
                NameKategori.Clear();
                UpdateUsersList();
                MessageBox.Show("Редактирование прошло успешно", "Cообщение");
                dataGridObject.Items.Refresh();
            }

        }
        private void MainTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            // Получаем ссылку на текстовое поле
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
            }



        }
        private void TextBox_PreviewTextInput2(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }
        private void TextBox_PreviewTextInput(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }
        private void Button_Click(object sender, RoutedEventArgs e)

        {
            string dl = "Администратор";
            MenuAdmin menu = new MenuAdmin(_thisStaffe);
            Close();
            menu.Show();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridObject.SelectedItem != null)
            {
                сategory = (Category)dataGridObject.SelectedItem;
                NameKategori.Text = сategory.Title;
                idSTEx = сategory.IdCategories.ToString();

               
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dbContext.Dispose();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void IdDojnostBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dbContext.Dispose();
            this.Close();
        }
    }
}
