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
using System.Xml.Linq;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlmazAucceon_3_.AddElement
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private Staffe _thisStaffe;
        private AuctionContext dbContext;
        private User satrudneke;
        int idDt;
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

        // Приватные поля
        private string _originalUserName;        // Имя пользователя
        private string _originalLastname;        // Фамилия
        private string _originalPatronymic;      // Отчество
        private string _originalEmail;           // Email
        private string _originalPhoneNumber;     // Номер телефона
        private int _originalIdRole;     // Название роли
        private string _originalPassword;        // Пароль
        private string _originalMoney;           // Деньги
        private int _originalIdUser;             // ID пользователя
        private DateTime _originalDataUser;      // Дата пользователя

        public AddUserWindow(Staffe thisStaffe)
        {
            InitializeComponent();
            _thisStaffe = thisStaffe;
            sortComboBox.ItemsSource = sortList;
            sortComboBox.SelectedIndex = -1;
            _thisStaffe = thisStaffe;
            string Names = _thisStaffe.StaffName;
            string famil = _thisStaffe.StaffLastname;
            string Papatronymics = _thisStaffe.StaffPatronymic;
            Disponsi(famil, Names, Papatronymics);
        }
        private void Disponsi(string famil, string Names, string Papatronymics)
        {
            Title = "Меню категорий" + " " + famil + " " + Names + " " + Papatronymics + "Менеджер";
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext = new();
            dbContext.Users.Include(r=> r.IdRoleNavigation).Load();
            dataGridObject.ItemsSource = dbContext.Users.Local.ToBindingList();
            UpdateUsersList();

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


        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsersList();
        }

        #region Датагрид
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridObject.SelectedItem != null)
            {
                satrudneke = (User)dataGridObject.SelectedItem;
                NameBox.Text = satrudneke.UserName;
                LasteNameBox.Text = satrudneke.Lastname;
                PatronymicBox.Text = satrudneke.Patronymic;
                EmailBox.Text = satrudneke.Email;
                PhoneNumberBox.Text = satrudneke.PhoneNumber;
                IdDojnostBox.Text = satrudneke.IdRoleNavigation.Title.ToString();
                PasswordBox.Text = satrudneke.Psswords;
                Text1.Text = satrudneke.Money.ToString();
                idDt = satrudneke.IdUser;
                BirthdayPicker.SelectedDate = new DateTime(satrudneke.DataUser.Year, satrudneke.DataUser.Month, satrudneke.DataUser.Day);

                // Присваиваем значения в приватные поля
                _originalUserName = satrudneke.UserName;
                _originalLastname = satrudneke.Lastname;
                _originalPatronymic = satrudneke.Patronymic;
                _originalEmail = satrudneke.Email;               
                _originalPhoneNumber = satrudneke.PhoneNumber;    
                _originalIdRole = satrudneke.IdRoleNavigation.Id; 
                _originalPassword = satrudneke.Psswords;          
                _originalMoney = satrudneke.Money;                
                _originalIdUser = satrudneke.IdUser;            
                _originalDataUser = new DateTime(satrudneke.DataUser.Year, satrudneke.DataUser.Month, satrudneke.DataUser.Day); // 
            }
        }
        #endregion

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (satrudneke == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления.");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить данного участника?", "Подверждение удаление", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                dbContext.Users.Remove(satrudneke);
                dbContext.SaveChanges();


                dbContext.SaveChanges();
                NameBox.Clear();
                LasteNameBox.Clear();
                PatronymicBox.Clear();
                BirthdayPicker.SelectedDate = null;
                EmailBox.Clear();
                PhoneNumberBox.Clear();
                PasswordBox.Clear();
                Text1.Clear();
                dataGridObject.Items.Refresh();

                satrudneke = null;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ProverkaAddUser();

        }
        private void ProverkaAddUser()
        {
            User user1 = App.context.Users.ToList().Find(u => u.Login == EmailBox.Text);
            User user2 = App.context.Users.ToList().Find(u => u.PhoneNumber == PhoneNumberBox.Text);
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите Имя.");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(LasteNameBox.Text))
            {

                MessageBox.Show("Пожалуйста, введите Фамилию.");
                return; // Exit the method to prevent further processing
            }

            else if (string.IsNullOrWhiteSpace(EmailBox.Text))
            {

                MessageBox.Show("Пожалуйста, введите Логин.");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(PhoneNumberBox.Text))
            {

                MessageBox.Show("Пожалуйста, введите Номер телефона.");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(Text1.Text))
            {

                MessageBox.Show("Пожалуйста, введите Счёт.");
                return; // Exit the method to prevent further processing
            }
            /*else if (PasswordBox.Text.Length != 4)
            {

                MessageBox.Show("В серии паспорта должнобыть 4 символа.");
                return; // Exit the method to prevent further processing
            }*/
            else if (string.IsNullOrWhiteSpace(PasswordBox.Text))
            {

                MessageBox.Show("Пожалуйста, введите Пароль.");
                return; // Exit the method to prevent further processing
            }
            /* else if (Text7.Text.Length != 6)
             {

                 MessageBox.Show("В номере паспорта должнобыть 6 символа.");
                 return; // Exit the method to prevent further processing
             }*/
            else if (string.IsNullOrWhiteSpace(IdDojnostBox.Text))
            {

                MessageBox.Show("Пожалуйста, введите Номер сотрудника.");
                return; // Exit the method to prevent further processing
            }
            /*else if (Text8.Text.Length != 12)
            {

                MessageBox.Show("В номеретелефона должно быть 11 символов");
                return; // Exit the method to prevent further processing
            }*/
            else if (BirthdayPicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату.", "Ошибка");
                return; // Выход из метода, если дата не выбрана
            }
            else if (user1 != null)
            {
                MessageBox.Show(" Уже есть пользователь с таким логином");
                return;
            }
            else if (user2 != null)
            {
                MessageBox.Show(" Уже есть пользователь с похожими номерам телефона.");
                return;
            }

            else if (user1 == null && user2 == null)
            {

                AddUser();
            }
            else
            {
                MessageBox.Show("Неверно введены данные");
                return;
            }

        }
        private bool AreAllValidChars0(string text)
        {

            // Проверяем, что текст содержит только цифры
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || PhoneNumberBox.Text.Length > 11)
                {
                    return false; // Найден недопустимый символ
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars00(string text)
        {

            // Проверяем, что текст содержит только цифры
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || IdDojnostBox.Text.Length > 0)
                {
                    return false; // Найден недопустимый символ
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars000(string text)
        {

            // Проверяем, что текст содержит только цифры
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || Text1.Text.Length > 14)
                {
                    return false; // Найден недопустимый символ
                }
            }
            return true; // Все символы допустимы
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidChars0(e.Text);
        }
        private void TextBox_PreviewTextInput1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidChars00(e.Text);
        }
        private void TextBox_PreviewTextInput2(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidChars000(e.Text);
        }
        private void AddUser()
        {
            string SurnameUser1 = LasteNameBox.Text;
            string NameUser1 = NameBox.Text;
            string PatronymicUser1 = PatronymicBox.Text;
            DateTime BirthdayPicker1 = BirthdayPicker.SelectedDate ?? DateTime.MinValue;
            DateOnly birthday = DateOnly.FromDateTime(BirthdayPicker1);
            string NumberUser1 = PhoneNumberBox.Text;
            string PasswordUser1 = PasswordBox.Text;
            string LoginUser1 = EmailBox.Text;
            string Many = Text1.Text;
            string PassportUser1 = IdDojnostBox.Text;

            ProverkaAddUserText(SurnameUser1, NameUser1, PatronymicUser1, NumberUser1, PasswordUser1, LoginUser1, Many, PassportUser1);


        }
        private void ProverkaAddUserText(string SurnameUser1, string NameUser1, string PatronymicUser1, string NumberUser1, string PasswordUser1, string LoginUser1, string Many, string PassportUser1)
        {
           
             if (!AreAllValidChars5(PasswordUser1))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidCharsPhone(NumberUser1))
            {
                MessageBox.Show("В номере телефона содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                PhoneNumberBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars4(LoginUser1))
            {
                MessageBox.Show("Почта содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                EmailBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars6(SurnameUser1))
            {
                MessageBox.Show("в фамилии содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                LasteNameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars7(NameUser1))
            {
                MessageBox.Show("в имени содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                NameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars8(PatronymicUser1))
            {
                MessageBox.Show("в очестве содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                PatronymicBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars9(Many))
            {
                MessageBox.Show("в счёте содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                Text1.Clear(); // Очищаем TextBox
                return;
            }

            else if (!AreAllValidChars4(EmailBox.Text))
            {
                MessageBox.Show("В почте содержится ошибка(и), попробуйте изменить его,(Максимальная длина 30, минримаььная 10, точка и @ обезательно должны присутствовать", "Ошибка");
                EmailBox.Clear(); // Очищаем TextBox
                return;
            }
            else
            {
                Add(SurnameUser1, NameUser1, PatronymicUser1, NumberUser1, PasswordUser1, LoginUser1, Many, PassportUser1);
            }

        }
        private void Add(string SurnameUser1, string NameUser1, string PatronymicUser1, string NumberUser1, string PasswordUser1, string LoginUser1, string Many, string PassportUser1)
        {
            string name = NameUser1;
            string LasteName = SurnameUser1;
            string patronumic = PatronymicUser1;
            DateTime birthDateTime = BirthdayPicker.SelectedDate ?? DateTime.MinValue;
            string email = LoginUser1;
            string phoneNumber = NumberUser1;
            string password = PasswordUser1;
            string nam = Many;
            int idDolgnost = 3;

            DateOnly birthDate = DateOnly.FromDateTime(birthDateTime);
            var newSatrudnik = new User { UserName = name, Lastname = LasteName, Patronymic = patronumic, DataUser = birthDateTime, Email = email, Psswords = password, PhoneNumber = phoneNumber, IdRole = idDolgnost, Money = nam };

            dbContext.Add(newSatrudnik);
            dbContext.SaveChanges();
            NameBox.Clear();
            LasteNameBox.Clear();
            PatronymicBox.Clear();
            BirthdayPicker.SelectedDate = null;
            EmailBox.Clear();
            PhoneNumberBox.Clear();
            PasswordBox.Clear();
        }
        private bool AreAllValidChars8(string text)
        {

            foreach (char c in text)
            {
                if (!char.IsLetter(c) || PatronymicBox.Text.Length > 20)
                {

                    return false;

                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars9(string text)
        {

            foreach (char c in text)
            {
                if (char.IsLetter(c) || Text1.Text.Length > 20)
                {

                    return false;

                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars7(string text)
        {

            foreach (char c in text)
            {
                if (!char.IsLetter(c) || LasteNameBox.Text.Length > 20)
                {

                    return false;

                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars6(string text)
        {

            foreach (char c in text)
            {
                if (!char.IsLetter(c) || NameBox.Text.Length > 20)
                {

                    return false;

                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars5(string text)
        {


            if (PasswordBox.Text.Length > 25)
            {

                return false;

            }

            else
            {
                return true; // Все символы допустимы
            }

        }

        private bool AreAllValidChars4(string text)
        {


            if (!Regex.IsMatch(text, @"^[a-zA-Z0-9]+\@[a-zA-Z]+\.[a-zA-Z]{2,4}$"))
            {
                return false;
            }
            return true;
        }

        private bool AreAllValidChars3(string text)
        {

            foreach (char c in text)
            {
                if (!char.IsDigit(c) || IdDojnostBox.Text.Length > 1)
                {

                    return false;

                }

            }

            return true; // Все символы допустимы
        }

   
        
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string SurnameUser1 = LasteNameBox.Text;
            string NameUser1 = NameBox.Text;
            string PatronymicUser1 = PatronymicBox.Text;
            DateTime BirthdayPicker1 = BirthdayPicker.SelectedDate ?? DateTime.MinValue;
            DateOnly birthday = DateOnly.FromDateTime(BirthdayPicker1);
            string NumberUser1 = PhoneNumberBox.Text;
            string PasswordUser1 = PasswordBox.Text;
            string LoginUser1 = EmailBox.Text;
            string Many = Text1.Text;
            string PassportUser1 = IdDojnostBox.Text;
            EditButtonProverka(SurnameUser1, NameUser1, PatronymicUser1, NumberUser1, PasswordUser1, LoginUser1, Many, PassportUser1);
        }
        private void EditButtonProverka(string SurnameUser1, string NameUser1, string PatronymicUser1, string NumberUser1, string PasswordUser1, string LoginUser1, string Many, string PassportUser1)
        {
            User user1 = App.context.Users.ToList().Find(u => u.Email == EmailBox.Text && u.IdUser != Convert.ToUInt32(idDt));
            User user2 = App.context.Users.ToList().Find(u => u.PhoneNumber == PhoneNumberBox.Text && u.IdUser != Convert.ToUInt32(idDt));
            Staffe user3 = App.context.Staffes.ToList().Find(u => u.Email == EmailBox.Text);
            Staffe user4 = App.context.Staffes.ToList().Find(u => u.PhoneNumber == PhoneNumberBox.Text);

            if (string.IsNullOrWhiteSpace(NameUser1))
            {
                MessageBox.Show("Пожалуйста, введите Имя.");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(SurnameUser1))
            {

                MessageBox.Show("Пожалуйста, введите Фамилию.");
                return; // Exit the method to prevent further processing
            }

            else if (string.IsNullOrWhiteSpace(LoginUser1))
            {

                MessageBox.Show("Пожалуйста, введите Логин.");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(NumberUser1))
            {

                MessageBox.Show("Пожалуйста, введите Номер телефона.");
                return; // Exit the method to prevent further processing
            }
            /*else if (PasswordBox.Text.Length != 4)
            {

                MessageBox.Show("В серии паспорта должнобыть 4 символа.");
                return; // Exit the method to prevent further processing
            }*/
            else if (string.IsNullOrWhiteSpace(PasswordUser1))
            {

                MessageBox.Show("Пожалуйста, введите Пароль.");
                return; // Exit the method to prevent further processing
            }
            /* else if (Text7.Text.Length != 6)
             {

                 MessageBox.Show("В номере паспорта должнобыть 6 символа.");
                 return; // Exit the method to prevent further processing
             }*/
         
            /*else if (Text8.Text.Length != 12)
            {

                MessageBox.Show("В номеретелефона должно быть 11 символов");
                return; // Exit the method to prevent further processing
            }*/
            else if (BirthdayPicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату.", "Ошибка");
                return; // Выход из метода, если дата не выбрана
            }
            else if (user1 != null)
            {
                MessageBox.Show(" Уже есть пользователь с таким логином");
                return;
            }
            else if (user2 != null)
            {
                MessageBox.Show(" Уже есть пользователь с похожими номерам телефона.");
                return;
            }
            else if (user3 != null)
            {
                MessageBox.Show(" Уже есть пользователь с такой почтой.");
                return;
            }
            else if (user4 != null)
            {
                MessageBox.Show(" Уже есть пользователь с такой же номером телефона.");
                return;
            }
            else if (!AreAllValidChars4(EmailBox.Text))
            {
                MessageBox.Show("В почте содержится ошибка(и), попробуйте изменить его,(Максимальная длина 30, минримаььная 10, точка и @ обезательно должны присутствовать", "Ошибка");
                EmailBox.Clear(); // Очищаем TextBox
                return;
            }

            else
            {
                AddChange(SurnameUser1, NameUser1, PatronymicUser1, NumberUser1, PasswordUser1, LoginUser1, Many, PassportUser1);
            }
        }

        #region Редактирование
        private void AddChange(string SurnameUser1, string NameUser1, string PatronymicUser1, string NumberUser1, string PasswordUser1, string LoginUser1, string Many, string PassportUser1)
        {
            string name = NameUser1;
            string LasteName = SurnameUser1;
            string patronumic = PatronymicUser1;
            DateTime birthDateTime = BirthdayPicker.SelectedDate ?? DateTime.MinValue;
            string email = LoginUser1;
            string phoneNumber = NumberUser1;
            string password = PasswordUser1;
            int idDolgnost = 3;
            int many = int.Parse(Many);

            // Результат проверки на изменения данных о пользователе
            bool hasChanged = HasChanged(name, LasteName, patronumic, birthDateTime, email, phoneNumber, password, idDolgnost, many);

            if (!hasChanged)
            {
                MessageBox.Show("Вы не внесли изменений.", "Ошибка");
                PatronymicBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars5(password))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidCharsPhone(NumberUser1))
            {
                MessageBox.Show("В номере телефона содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                PhoneNumberBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars4(email))
            {
                MessageBox.Show("Почта содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                EmailBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars6(LasteName))
            {
                MessageBox.Show("в фамиоии содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                LasteNameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars7(name))
            {
                MessageBox.Show("в имени содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                NameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars8(patronumic))
            {
                MessageBox.Show("в очестве содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                PatronymicBox.Clear(); // Очищаем TextBox
                return;
            }
            else
            {
                AddFinhs(name, LasteName, patronumic, birthDateTime, email, phoneNumber, password, idDolgnost, many);
            }
        } 
        #endregion

        // Менялись ли значения у выбраного участника
        private bool HasChanged(string name, string LasteName, string patronumic, DateTime birthDateTime, string email, string phoneNumber, string password, int idDolgnost, int many)
        {
            // Сравниваем входные параметры с оригинальными полями
            bool hasChanged =
                name != _originalUserName ||
                LasteName != _originalLastname ||
                patronumic != _originalPatronymic ||
                birthDateTime.Date != _originalDataUser.Date || // Сравниваем только дату
                email != _originalEmail ||
                phoneNumber != _originalPhoneNumber ||
                password != _originalPassword ||
                idDolgnost != _originalIdRole || // Предполагается, что idDolgnost соответствует роли
                many.ToString() != _originalMoney; // Сравнение значений денег

            return hasChanged; // Возвращаем результат сравнения
        }

        private bool AreAllValidCharsPhone(string text)
        {
            foreach (char c in text)
            {
                if (PhoneNumberBox.Text.Length < 12)
                {

                    return false;

                }
            }
            return true; // Все символы допустимы
        }
        private void AddFinhs(string name, string LasteName, string patronumic, DateTime birthDateTime, string email, string phoneNumber, string password, int idDolgnost, int many)
        {
            MessageBox.Show($"Данные пользователя {name} {LasteName} отредактированы!");

            DateOnly birthDate = DateOnly.FromDateTime(birthDateTime);
            satrudneke.UserName = name;
            satrudneke.Lastname = LasteName;
            satrudneke.Patronymic = patronumic;
            satrudneke.DataUser = birthDateTime;
            satrudneke.Email = email;
            satrudneke.Psswords = password;
            satrudneke.PhoneNumber = phoneNumber;
            satrudneke.IdRole = idDolgnost;
            satrudneke.Money = many.ToString();

            dbContext.Update(satrudneke);
            dbContext.SaveChanges();

            NameBox.Clear();
            LasteNameBox.Clear();
            PatronymicBox.Clear();
            BirthdayPicker.SelectedDate = null;
            EmailBox.Clear();
            PhoneNumberBox.Clear();
            PasswordBox.Clear();
            Text1.Clear();
            dataGridObject.Items.Refresh();
        }
        private void MainTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            // Получаем ссылку на текстовое поле
            var textBox = sender as TextBox;
            if (e.Key == Key.Space)
            {
                e.Handled = true; // Запретить ввод пробела
                return;
            }
            // Проверка на пробел
            if (e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Enter)
            {

                return; // Allow these keys
            }

            if (textBox == NameBox || textBox == LasteNameBox || textBox == PatronymicBox) // Фамилия, Имя, Отчество
            {
                

                // Проверка длины для имени, фамилии и отчества (20 символов)
                if (textBox.Text.Length >= 20 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }

            if (textBox == PasswordBox || textBox == EmailBox) // Логин и Пароль
            {
                // Ограничение на 25 символов
                if (textBox.Text.Length > 24 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }

            }

            if (textBox == IdDojnostBox)
            {
                char keyChar2 = (char)KeyInterop.VirtualKeyFromKey(e.Key);
                if (textBox.Text.Length > 0 && e.Key != Key.Back || !char.IsDigit(keyChar2))
                {
                    e.Handled = true;
                }
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)

        {
            string dl = "Администратор";
            MenuM menu = new MenuM(_thisStaffe);
            Close();
            menu.Show();
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dbContext.Dispose();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneNumberBox.Text.Length < 3 || !PhoneNumberBox.Text.StartsWith("+7"))
            {
                PhoneNumberBox.Text = "+7";
                PhoneNumberBox.SelectionStart = PhoneNumberBox.Text.Length; // Place cursor at the end
            }
        }

        private void Text1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LasteNameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^а-яА-ЯёЁ]");
            if (regex.IsMatch(e.Text) || (LasteNameBox.Text.Length + e.Text.Length > 20))
            {
                e.Handled = true;
            }

            if (!string.IsNullOrEmpty(e.Text) && LasteNameBox.Text.Length > 0 && char.IsUpper(e.Text[0]))
            {
                e.Handled = true;
            }

        }
        private void NameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (!Regex.IsMatch(e.Text, @"^[а-яА-ЯёЁ\-]+$") || (NameBox.Text.Length + e.Text.Length > 20))
            {
                e.Handled = true;
            }
            if (!string.IsNullOrEmpty(e.Text) && NameBox.Text.Length > 0 && char.IsUpper(e.Text[0]))
            {
                e.Handled = true;
            }
        }
        private void PatronymicBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-ЯёЁ]");
            if (regex.IsMatch(e.Text) || (PatronymicBox.Text.Length + e.Text.Length > 20))
            {
                e.Handled = true;
            }
            if (e.Text.Length > 0 && PatronymicBox.Text.Length > 0 && char.IsUpper(e.Text[0]))
            {
                e.Handled = true;
            }
        }

        private void NameBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (NameBox.Text.Length > 0)
            {
                string text = NameBox.Text;
                NameBox.Text = char.ToUpper(text[0]) + text.Substring(1);
                NameBox.CaretIndex = NameBox.Text.Length;
            }
        }

        private void LasteNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LasteNameBox.Text.Length > 0)
            {
                string text = LasteNameBox.Text;
                LasteNameBox.Text = char.ToUpper(text[0]) + text.Substring(1);
                LasteNameBox.CaretIndex = LasteNameBox.Text.Length;
            }
        }

        private void PatronymicBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PatronymicBox.Text.Length > 0)
            {
                string text = PatronymicBox.Text;
                PatronymicBox.Text = char.ToUpper(text[0]) + text.Substring(1);
                PatronymicBox.CaretIndex = PatronymicBox.Text.Length;
            }
        }
        private void BirthDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BirthdayPicker.SelectedDate.HasValue)
            {
                DateTime specificDate5 = new DateTime(2006, 10, 22);

                DateTime selectedDate = BirthdayPicker.SelectedDate.Value;
                DateTime today = DateTime.Today;

                // Проверяем, что дата не меньше 18 лет назад и не больше 100 лет назад
                if (selectedDate > today.AddYears(-18))
                {
                    MessageBox.Show("Сотрудник должен быть младше 18 лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    BirthdayPicker.SelectedDate = specificDate5; // Сбрасываем выбор
                    return;
                }
                else if (selectedDate < today.AddYears(-100))
                {
                    MessageBox.Show("Сотрудник не может быть старше 100 лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    BirthdayPicker.SelectedDate = specificDate5; // Сбрасываем выбор
                    return;
                }

            }
        }
        private void IdDojnostBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dbContext.Dispose();
            this.Close();
        }

        private void idS_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}


