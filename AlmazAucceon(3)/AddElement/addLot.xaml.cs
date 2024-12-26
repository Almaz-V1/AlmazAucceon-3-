using AlmazAucceon_3_.Model2;
using AlmazAucceon_3_.menu;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AlmazAucceon_3_.AddElement
{
    public class ByteArrayToImageConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, является ли значение массивом байтов
            if (value is byte[] byteArray && byteArray.Length > 0)
            {
                BitmapImage image = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                }
                return image;
            }

            // Возвращаем заглушку при null или пустом массиве
            return new BitmapImage(new Uri(@"pack://application:,,,/Image/notImage.png", UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class addLot : Window
    {
        public List<Category> GetProfessions()
        {
            using (var context = new AuctionContext())
            {
                return context.Categories.ToList(); // Получаем все профессии
            }
        }
        string idSTEx;
        private Item _holpes;
        private Item Lotes;
        private List<string> sortList = new List<string>
        {
            "Id по возрастанию",
            "Название по возрвстанию",
            "Цена по возрвстанию",
            "Категория по возрвстанию",
            "Id по убыванию",
            "Название по убыванию",
            "Цена по убыванию",
            "Категория по убыванию"
        };
        private AuctionContext dbContext;
        private Staffe _thisStaffe;


        public addLot(Staffe thisStaffe)
        {
            InitializeComponent();
            _thisStaffe = thisStaffe;

            dbContext = new AuctionContext();
            dbContext.Items.Include(c => c.Categotia).Load();

            sortComboBox.ItemsSource = sortList;

            List<string> categories = new List<string>() { "Все" };
            foreach (var categoria in App.context.Categories)
            {
                categories.Add(categoria.Title);
            }
            filterComboBox.ItemsSource = categories;

            UpdateUsersList();

            _thisStaffe = thisStaffe;
            string Names = _thisStaffe.StaffName;
            string famil = _thisStaffe.StaffLastname;
            string Papatronymics = _thisStaffe.StaffPatronymic;
            Disponsi(famil, Names, Papatronymics);
        }
        private void UpdateUsersList()
        {
            dbContext = new AuctionContext();

            var itemsFromDb = dbContext.Items.Include(c => c.Categotia).ToList(); // Предположим, что у вас есть DbSet<Item>
            ObservableCollection<Item> items = new ObservableCollection<Item>();

            foreach (var item in itemsFromDb)
            {
                if (item.ImageItems == null || item.ImageItems.Length == 0)
                {
                    // Загружаем заглушку в качестве массива байтов
                    string file = @"C:\Users\Acer\source\repos\AlmazAucceon(3)\AlmazAucceon(3)\Image\notImage.png";

                    ImageSource image = new BitmapImage(new Uri(file, UriKind.Absolute));
                    item.ImageItems = ImageSourceToBytes(image);
                }

                items.Add(new Item()
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    CurrentPrice = item.CurrentPrice,
                    CategotiaId = item.CategotiaId,
                    Categotia = item.Categotia,
                    ImageItems = item.ImageItems  // Если ImageItems равно null, используем заглушку
                });
            }

            items = ApplySort(items);
            items = ApplySearch(items);
            items = ApplyFilter(items);

            dataGridObject.ItemsSource = null;
            dataGridObject.ItemsSource = items;

            int itemCount = dataGridObject.Items.Count;
            CountLabel.Content = $"{itemCount}";
        }
        public static byte[]? ImageSourceToBytes(ImageSource? imageSource)
        {
            byte[]? bytes = null;

            var bitmapSource = imageSource as BitmapSource;
            if (bitmapSource != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }
            return bytes;
        }
        private void Disponsi(string famil, string Names, string Papatronymics)
        {
            Title = "Меню лотов" + " " + famil + " " + Names + " " + Papatronymics + " Администратор";
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var professions = GetProfessions(); // Получаем профессии из базы
            if (professions != null && professions.Count > 0)
            {
                comboBoxProfessions.ItemsSource = professions; // Устанавливаем источник данных для комбобокса
            }
            else
            {
                MessageBox.Show("Нет доступных категооорий.", "Ошибка");
            }
            dbContext.Items.Include(r => r.Categotia).Load();

            UpdateUsersList();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dbContext.Dispose();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridObject.SelectedItem != null)
            {
                Lotes = (Item)dataGridObject.SelectedItem;

                // Передача значений в текстовые поля
                NameBox.Text = Lotes.ItemName;
                comboBoxProfessions.SelectedValue = Lotes.CategotiaId;
                CurrentPriceBox.Text = Lotes.CurrentPrice.ToString(); // Форматирование цены
                idSTEx = Lotes.ItemId.ToString();
                // Проверка наличия изображения и его установка
                if (Lotes.ImageItems != null)
                {
                    BitmapImage imageBitmap = new BitmapImage();
                    using (MemoryStream stream = new MemoryStream(Lotes.ImageItems))
                    {
                        imageBitmap.BeginInit();
                        imageBitmap.StreamSource = stream;
                        imageBitmap.CacheOption = BitmapCacheOption.OnLoad;
                        imageBitmap.EndInit();
                    }
                    SelectedImage.Source = imageBitmap; // Установка изображения в элемент Image
                }
               
            }
        }



        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            sortComboBox.SelectedIndex = 0;
            filterComboBox.SelectedIndex = 0;
            searchTextBox.Text = string.Empty;

            UpdateUsersList();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private ObservableCollection<Item> ApplySort(ObservableCollection<Item> staffes)
        {
            int sortIndex = sortComboBox.SelectedIndex;
            List<Item> staffesList = new List<Item>(staffes.ToList());

            if (sortComboBox.SelectedIndex != -1)
            {
                switch (sortIndex)
                {
                    case 0:
                        return new ObservableCollection<Item>(staffesList.OrderBy(e => e.ItemId).ToList());
                    case 1:
                        return new ObservableCollection<Item>(staffesList.OrderBy(e => e.ItemName).ToList());
                    case 2:
                        return new ObservableCollection<Item>(staffesList.OrderBy(e => e.CurrentPrice).ToList());
                    case 3:
                        return new ObservableCollection<Item>(staffesList.OrderBy(e => e.CategotiaId).ToList());
                    case 4:
                        return new ObservableCollection<Item>(staffesList.OrderByDescending(e => e.ItemId).ToList());
                    case 5:
                        return new ObservableCollection<Item>(staffesList.OrderByDescending(e => e.ItemName).ToList());
                    case 6:
                        return new ObservableCollection<Item>(staffesList.OrderByDescending(e => e.CurrentPrice).ToList());
                    case 7:
                        return new ObservableCollection<Item>(staffesList.OrderByDescending(e => e.CategotiaId).ToList());

                }
            }

            return new ObservableCollection<Item>(staffesList.ToList());
        }

        private ObservableCollection<Item> ApplySearch(ObservableCollection<Item> staffesList)
        {
            string searchText = searchTextBox.Text.ToLower();
            if (searchText != string.Empty)
            {
                staffesList = new ObservableCollection<Item>(staffesList.Where(r => r.ItemName.ToLower().StartsWith(searchText)).ToList());
            }
            return new ObservableCollection<Item>(staffesList.ToList());
        }

        private ObservableCollection<Item> ApplyFilter(ObservableCollection<Item> staffesList)
        {
            if (filterComboBox.SelectedIndex != 0 && filterComboBox.SelectedIndex != -1 && filterComboBox.SelectedItem != null)
            {
                var selecedCategoria = filterComboBox.SelectedItem.ToString();
                return staffesList = new ObservableCollection<Item>(staffesList.Where(r => r.Categotia.Title == selecedCategoria).ToList());
            }
            return new ObservableCollection<Item>(staffesList.ToList());        
        }
        private void CategoriId_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void CurrentPriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidCharsser(e.Text);
        }
        private bool AreAllValidCharsser(string text)
        {
            // Проверяем, что текст содержит только цифры
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || CurrentPriceBox.Text.Length > 10)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
     

            string Titles = NameBox.Text.Trim().ToLower();
            Item user1 = App.context.Items.ToList().Find(u => u.ItemName.Trim().ToLower() == Titles);
            string SurnameUser1 = NameBox.Text;
            string PatronymicUser1 = CurrentPriceBox.Text;

            
            string NameBoxText = NameBox.Text;
            string CurrentPriceBoxText = CurrentPriceBox.Text;

            if (!AreAllValidChars6(NameBoxText))
            {
                MessageBox.Show("в название лота содержатся ошибка(и), попробуйте изменить его.", "Ошибка");
                NameBox.Clear(); // Очищаем TextBox
                return;
            }

            else if (user1 != null)
            {
                MessageBox.Show(" Уже есть такой лот");
                return;
            }
            else if (!AreAllValidChars8(CurrentPriceBoxText))
            {
                MessageBox.Show("в конечной цене содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                CurrentPriceBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (string.IsNullOrWhiteSpace(SurnameUser1))
            {
                MessageBox.Show("Пожалуйста, введите название.", "Ошибка");
                return; // Exit the method to prevent further processing
            }

            else if (string.IsNullOrWhiteSpace(PatronymicUser1))
            {
                MessageBox.Show("Пожалуйста, введите итоговую цену.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (comboBoxProfessions.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, укажите категорию товара.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (Convert.ToInt32(CurrentPriceBox.Text) <= 0)
            {
                MessageBox.Show("Цена не может быть равна 0 или быть отрицательной,", "Ошибка");
                return;
            }
            else
            {
                int selectedProfessionId = (int)comboBoxProfessions.SelectedValue;
                byte[] imageBytes = ImageSourceToByteArray((BitmapImage)SelectedImage.Source);

                addButtonLot(NameBoxText, selectedProfessionId, CurrentPriceBoxText, imageBytes);

            }

        }
       
        private bool AreAllValidChars6(string text)
        {

            foreach (char c in text)
            {
                if (NameBox.Text.Length > 20)
                {

                    return false;

                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars8(string text)
        {

            // Проверяем, что текст содержит только цифры
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || CurrentPriceBox.Text.Length > 20)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }


        private void addButtonLot(string NameBoxText, int CategoriIdText, string CurrentPriceBoxText, byte[] imageBytes)
        {
            
            var newLot = new Item
            {
                ItemName = NameBoxText,
                CategotiaId = Convert.ToInt32(CategoriIdText),
                CurrentPrice = Convert.ToInt32(CurrentPriceBoxText),
                ImageItems = imageBytes
            };

            dbContext.Add(newLot);
            dbContext.SaveChanges();
            NameBox.Clear();
            comboBoxProfessions.SelectedIndex = -1;
            CurrentPriceBox.Clear();
            SelectedImage.Source = null;
            
        }


        private byte[] ImageSourceToByteArray(BitmapImage imageSource)
        {
            if (imageSource == null) return null;

            using (var memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder(); // Выберите нужный формат
                encoder.Frames.Add(BitmapFrame.Create(imageSource));
                encoder.Save(memoryStream);
                return memoryStream.ToArray();
            }
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string Titles = NameBox.Text.Trim().ToLower();

            Item user1 = App.context.Items.ToList().Find(u => u.ItemName.Trim().ToLower() == Titles && u.ItemId != Convert.ToUInt32(idSTEx));
            string SurnameUser1 = NameBox.Text;
         
            string PatronymicUser1 = CurrentPriceBox.Text;


            string NameBoxText = NameBox.Text;
            
            string CurrentPriceBoxText = CurrentPriceBox.Text;

            if (!AreAllValidChars6(NameBoxText))
            {
                MessageBox.Show("в название лота содержатся ошибка(и), попробуйте изменить его.", "Ошибка");
                NameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (user1 != null)
            {
                MessageBox.Show(" Уже есть такой лот");
                return;
            }
            else if (!AreAllValidChars8(CurrentPriceBoxText))
            {
                MessageBox.Show("в конечной цене содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                CurrentPriceBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (string.IsNullOrWhiteSpace(SurnameUser1))
            {
                MessageBox.Show("Пожалуйста, введите название.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(PatronymicUser1))
            {
                MessageBox.Show("Пожалуйста, введите итоговую цену.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
           

            
            else if (comboBoxProfessions.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, укажите категорию товара.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (Convert.ToInt32(CurrentPriceBox.Text) <= 0)
            {
                MessageBox.Show("Цена не может быть равна 0 или быть отрицательной,", "Ошибка");
                return;
            }
            else
            {
                byte[] imageBytes = ImageSourceToByteArray((BitmapImage)SelectedImage.Source);
                int selectedProfessionId = (int)comboBoxProfessions.SelectedValue;
                IsetIs(NameBoxText, selectedProfessionId, CurrentPriceBoxText, imageBytes);
            }
        }
        private void IsetIs(string NameBoxText, int CategoriIdText, string CurrentPriceBoxText, byte[] imageBytes)
        {
            _holpes = dbContext.Items.First(r => r.ItemId == Convert.ToInt32(idSTEx));

            _holpes.ItemName = NameBoxText;
            _holpes.CategotiaId = Convert.ToInt32(CategoriIdText);
            _holpes.CurrentPrice = Convert.ToInt32(CurrentPriceBoxText);
            _holpes.ImageItems = imageBytes;
            dbContext.SaveChanges();
            dataGridObject.Items.Refresh();
            UpdateUsersList();
            comboBoxProfessions.SelectedIndex = -1;
            MessageBox.Show("Редактирование прошло успешно", "Сообщение");
        }
        private void MainTextBox_PreviewKeyDown1(object sender, KeyEventArgs e)
        {

            // Получаем ссылку на текстовое поле
            var textBox = sender as TextBox;


            if (textBox == NameBox) // 
            {


                // Проверка длины для имени, фамилии и отчества (20 символов)
                if (textBox.Text.Length >= 25 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }
        }
        private void MainTextBox_PreviewKeyDown2(object sender, KeyEventArgs e)
        {

            // Получаем ссылку на текстовое поле
            var textBox = sender as TextBox;


            if (textBox == NameBox) // 
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
        private void MainTextBox_PreviewKeyDown3(object sender, KeyEventArgs e)
        {

            // Получаем ссылку на текстовое поле
            var textBox = sender as TextBox;


            if (textBox == NameBox) // 
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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MenuAdmin menuAadmin = new MenuAdmin(_thisStaffe);
            menuAadmin.Show();
            Close();
        }

        private void AddButtonImadge_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                // Загружаем изображение
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));

                // Отображаем загруженное изображение в элементе управления Image
                SelectedImage.Source = bitmap;
            }
        }

        private void DelitButtonImadge_Click(object sender, RoutedEventArgs e)
        {
            SelectedImage.Source = null;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            if (_holpes == null)
            {
                MessageBox.Show("Выберите лот для удаления.", "Ошибка");
                return;
            }
            else
            {
                Item holpes = App.context.Items.First(r => r.ItemId == _holpes.ItemId);
                holpes.IsDeleted = 1;
                App.context.Update(holpes);
                App.context.SaveChanges();
                _holpes = null;
                comboBoxProfessions.SelectedIndex = -1;
                UpdateUsersList();
                MessageBox.Show("Удаление прошло успешно", "Сообщение");
            }

        }

        private void comboBoxProfessions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CurrentPriceBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dbContext.Dispose();
             this.Close();
        }
    }
}
