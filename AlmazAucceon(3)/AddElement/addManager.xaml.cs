using AlmazAucceon_3_.Model2;
using AlmazAucceon_3_.menu;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlmazAucceon_3_.AddElement
{
    /// <summary>
    /// Логика взаимодействия для addManager.xaml
    /// </summary>
    public partial class addManager : Window
    {
        string IdEmploers;
        public List<Role> GetProfessions()
        {
            using (var context = new AuctionContext())
            {
                return context.Roles.ToList();
            }
        }

        private AuctionContext dbContext;
        private Staffe satrudneke;
        private Staffe _thisStaffe;
        private List<string> sortList = new List<string>
        {
            "Имя по возрастанию",
            "Фамилия по возрастанию",
            "Имя по убыванию",
            "Фамилия по убыванию",
            "Отчество по убыванию",
            "Номер телефона по убыванию"
        };
        public addManager(Staffe thisStaffe)
        {
            InitializeComponent();
            _thisStaffe = thisStaffe;
            dbContext = new AuctionContext();
            dbContext.Staffes.Include(c => c.IdStaffRoleNavigation).Load();
            string Names = _thisStaffe.StaffName;
            string famil = _thisStaffe.StaffLastname;
            string Papatronymics = _thisStaffe.StaffPatronymic;
            Disponsi(famil, Names, Papatronymics);
            filterComboBox.ItemsSource = dbContext.Roles.Select(e => e.Title).ToList();
            sortComboBox.ItemsSource = sortList;

            filterComboBox.SelectedIndex = -1;
            sortComboBox.SelectedIndex = 8;
        }

        private void Disponsi(string famil, string Names, string Papatronymics)
        {
            Title = "Меню работников" + " " + famil + " " + Names + " " + Papatronymics + " Администратор";
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
                MessageBox.Show("Нет доступных ролей.", "Ошибка");
            }
            dbContext.Staffes.Include(r => r.IdStaffRoleNavigation).Load();
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
        private void UpdateUsersList()
        {
            dbContext = new AuctionContext();

            var itemsFromDb = dbContext.Staffes.Include(c => c.IdStaffRoleNavigation).ToList(); // Предположим, что у вас есть DbSet<Item>
            ObservableCollection<Staffe> items = new ObservableCollection<Staffe>();

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


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridObject.SelectedItem != null)
            {
                satrudneke = (Staffe)dataGridObject.SelectedItem;
                string g = satrudneke.IdStaffes.ToString();

                PochtaBox.Text = satrudneke.Email;
                NameBox.Text = satrudneke.StaffName;
                LasteNameBox.Text = satrudneke.StaffLastname;
                PatronymicBox.Text = satrudneke.StaffPatronymic;
                EmailBox.Text = satrudneke.StaffLogin;
                PhoneNumberBox.Text = satrudneke.PhoneNumber;
                PasswordBox.Text = satrudneke.StaffPsswords;
                IdEmploers = satrudneke.IdStaffes.ToString();
                comboBoxProfessions.SelectedValue = satrudneke.IdStaffRole;
                BirthdayPicker.SelectedDate = new DateTime(satrudneke.Data.Year, satrudneke.Data.Month, satrudneke.Data.Day);
            }
        }

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
            if (satrudneke.IdStaffes == _thisStaffe.IdStaffes)
            {
                MessageBox.Show("Вы не можете удалть себя!.");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить данного сотрудника?", "Подверждение удаление", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes && _thisStaffe.IdStaffes != satrudneke.IdStaffes)
            {
                dbContext.Staffes.Remove(satrudneke);
                dbContext.SaveChanges();



                NameBox.Clear();
                LasteNameBox.Clear();
                PatronymicBox.Clear();
                BirthdayPicker.SelectedDate = null;
                EmailBox.Clear();
                PhoneNumberBox.Clear();
                comboBoxProfessions.SelectedIndex = -1;
                PochtaBox.Clear();
                PasswordBox.Clear();
                dataGridObject.Items.Refresh();

                satrudneke = null;
                UpdateUsersList();
                MessageBox.Show("Удаление прошло успешно", "Сообщение");
            }

        }

        private void MainTextBox_PreviewKeyDown(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[a-zA-Z]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true; // Запретить ввод английских букв
            }
        }

        private void NameBox_PreviewTextInput(object sender, RoutedEventArgs e)
        {

        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserProverka();
        }
        private void AddUserProverka()
        {
            Staffe user1 = App.context.Staffes.ToList().Find(u => u.StaffLogin == EmailBox.Text);
            Staffe user2 = App.context.Staffes.ToList().Find(u => u.PhoneNumber == PhoneNumberBox.Text);
            Staffe user3 = App.context.Staffes.ToList().Find(u => u.Email == PochtaBox.Text);
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
            else if (comboBoxProfessions.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, укажите категорию товара.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            /*else if (Text8.Text.Length != 12)
            {

                MessageBox.Show("В номеретелефона должно быть 11 символов");
                return; // Exit the method to prevent further processing
            }*/

            else if (string.IsNullOrWhiteSpace(PochtaBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите почту.", "Ошибка");
                return; // Выход из метода, если дата не выбрана
            }
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
                MessageBox.Show(" Уже есть пользователь с похожей почтой.");
                return;
            }

            else if (user1 == null && user2 == null && user3 == null)
            {

                AddUser();
            }
            else
            {
                MessageBox.Show("Данные существуют у другого ползователя");
                return;
            }

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
            string Pochta = PochtaBox.Text;

            if (!AreAllValidChars5(PasswordUser1))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordBox.Clear(); // Очищаем TextBox
                return;
            }

            else if (!AreAllValidChars6(SurnameUser1))
            {
                MessageBox.Show("в фамиоии содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                LasteNameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars7(NameUser1))
            {
                MessageBox.Show("в имени содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                NameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!NameFormat(NameUser1))
            {
                MessageBox.Show("в имени формата 'имя-имя' не должно быть больше одного знака '-', попробуйте изменить его.", "Ошибка");
                NameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars8(PatronymicUser1))
            {
                MessageBox.Show("в очестве содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                PatronymicBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidCharsPhone(NumberUser1))
            {
                MessageBox.Show("В номере телефона содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                PhoneNumberBox.Clear(); // Очищаем TextBox
                return;
            }

            else if (!EmailFormat(Pochta))
            {
                MessageBox.Show("В почте содержится ошибка(и), попробуйте изменить его,(Максимальная длина 30, минримаььная 10, точка и @ обезательно должны присутствовать", "Ошибка");
                PochtaBox.Clear(); // Очищаем TextBox
                return;
            }
            else
            {
                string name = NameBox.Text;
                string LasteName = LasteNameBox.Text;
                string patronumic = PatronymicBox.Text;
                DateTime birthDateTime = BirthdayPicker.SelectedDate ?? DateTime.MinValue;
                string email = EmailBox.Text;
                string phoneNumber = PhoneNumberBox.Text;
                string password = PasswordBox.Text;
                string Pochta1 = PochtaBox.Text;
                int selectedProfessionId = (int)comboBoxProfessions.SelectedValue;

                DateOnly birthDate = DateOnly.FromDateTime(birthDateTime);
                var newSatrudnik = new Staffe
                {
                    StaffName = name,
                    StaffLastname = LasteName,
                    StaffPatronymic = patronumic,
                    Data = birthDateTime,
                    StaffLogin = email,
                    StaffPsswords = password,
                    PhoneNumber = phoneNumber,
                    IdStaffRole = selectedProfessionId,
                    Email = Pochta1
                };

                dbContext.Staffes.Add(newSatrudnik);
                dbContext.SaveChanges();
                NameBox.Clear();
                LasteNameBox.Clear();
                PatronymicBox.Clear();
                BirthdayPicker.SelectedDate = null;
                EmailBox.Clear();
                PhoneNumberBox.Clear();
                comboBoxProfessions.SelectedIndex = -1;

                PochtaBox.Clear();
                PasswordBox.Clear();
                dataGridObject.Items.Refresh();

                satrudneke = null;
                UpdateUsersList();
                MessageBox.Show("Дабавление просло успешно", "Сообщение");
            }
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
        private bool AreAllValidCharsPochta(string text)
        {

            foreach (char c in text)
            {
                if (PochtaBox.Text.Length > 25 || PochtaBox.Text.Length < 10)
                {
                    return false;
                }
            }

            if (!PochtaBox.Text.Contains('@') || !PochtaBox.Text.Contains('.')) return false;

            return true; // Все символы допустимы
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
        private bool AreAllValidChars7(string text)
        {

            foreach (char c in text)
            {
                if (/*!char.IsLetter(c) || */LasteNameBox.Text.Length > 20)
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



        private bool AreAllValidChars(string text)
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

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ProverkaTextEditClik();
        }
        private void ProverkaTextEditClik()
        {
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
            else if (comboBoxProfessions.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, укажите категорию товара.", "Ошибка");
                return; // Exit the method to prevent further processing
            }

            else if (!EmailFormat(PochtaBox.Text))
            {
                MessageBox.Show("В почте содержится ошибка(и), попробуйте изменить его,(Максимальная длина 30, минримаььная 10, точка и @ обезательно должны присутствовать", "Ошибка");
                PochtaBox.Clear(); // Очищаем TextBox
                return;
            }
            /*else if (Text8.Text.Length != 12)
            {

                MessageBox.Show("В номеретелефона должно быть 11 символов");
                return; // Exit the method to prevent further processing
            }*/


            else
            {
                ProverkaAddChange();

            }

        }
        private void ProverkaAddChange()
        {

            Staffe user1 = App.context.Staffes.ToList().Find(u => u.StaffLogin == EmailBox.Text && u.IdStaffes != Convert.ToUInt32(IdEmploers));
            Staffe user2 = App.context.Staffes.ToList().Find(u => u.PhoneNumber == PhoneNumberBox.Text && u.IdStaffes != Convert.ToUInt32(IdEmploers));
            Staffe user3 = App.context.Staffes.ToList().Find(u => u.Email == PochtaBox.Text && u.IdStaffes != Convert.ToUInt32(IdEmploers));
            if (BirthdayPicker.SelectedDate == null)
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
                MessageBox.Show(" Уже есть пользователь с похожей почтой.");
                return;
            }

            else if (user1 == null && user2 == null && user3 == null)
            {

                AddChange();
            }
            else
            {
                MessageBox.Show("Данные существуют у другого ползователя");
                return;
            }
        }
        private void AddChange()
        {

            string name = NameBox.Text;
            string LasteName = LasteNameBox.Text;
            string patronumic = PatronymicBox.Text;

            string email = EmailBox.Text;
            string phoneNumber = PhoneNumberBox.Text;
            string password = PasswordBox.Text;
            string pochta = PochtaBox.Text;

            ProverkaCtrlV(name, LasteName, patronumic, email, phoneNumber, password, pochta);



        }
        private void ProverkaCtrlV(string name, string LasteName, string patronumic, string email,
            string phoneNumber, string password, string Pochta)
        {
            DateTime birthDateTime = BirthdayPicker.SelectedDate ?? DateTime.MinValue;
            if (!AreAllValidChars5(password))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidCharsPochta(Pochta))
            {
                MessageBox.Show("В почте содержится ошибка(и), попробуйте изменить его,(Максимальная длина 30, минримаььная 10, точка и @ обезательно должны присутствовать", "Ошибка");
                PochtaBox.Clear(); // Очищаем TextBox
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
                Add(name, LasteName, patronumic, email, phoneNumber, password, birthDateTime, Pochta);

            }
        }
        private bool NameFormat(string name)
        {
            if (name.Contains('-'))
            {
                if (!Regex.IsMatch(name, @"^[а-яА-Я]+-[а-яА-Я]+$"))
                {
                    return false;
                }
            }
            return true;
        }

        private bool EmailFormat(string email)
        {
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9]+\@[a-zA-Z]+\.[a-zA-Z]{2,4}$"))
            {
                return false;
            }
            return true;
        }

        private void Add(string name, string LasteName, string patronumic, string email, string phoneNumber,
            string password, DateTime birthDateTime, string Pochta)
        {
            int selectedProfessionId = (int)comboBoxProfessions.SelectedValue;
            satrudneke.StaffName = name;
            satrudneke.StaffLastname = LasteName;
            satrudneke.StaffPatronymic = patronumic;
            satrudneke.Data = birthDateTime;
            satrudneke.StaffLogin = email;
            satrudneke.StaffPsswords = password;
            satrudneke.PhoneNumber = phoneNumber;
            satrudneke.Email = Pochta;
            satrudneke.IdStaffRole = selectedProfessionId;

            dbContext.SaveChanges();

            NameBox.Clear();
            LasteNameBox.Clear();
            PatronymicBox.Clear();
            BirthdayPicker.SelectedDate = null;
            EmailBox.Clear();
            PhoneNumberBox.Clear();
            comboBoxProfessions.SelectedIndex = -1;
            PochtaBox.Clear();
            PasswordBox.Clear();
            dataGridObject.Items.Refresh();
            sortComboBox.SelectedIndex = 8;
            filterComboBox.SelectedIndex = -1;
            satrudneke = null;
            UpdateUsersList();
            MessageBox.Show("Редактирование прошло успешно", "Cообщение");
            dataGridObject.Items.Refresh();
        }
        private void MainTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Получаем ссылку на текстовое поле
            var textBox = sender as TextBox;

            // Запретить ввод пробела
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }

            // Разрешаем клавиши Back, Tab и Enter
            if (e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Enter)
            {
                return;
            }

            // Проверяем текстовые поля для имени, фамилии и отчества
            if (textBox == NameBox || textBox == LasteNameBox || textBox == PatronymicBox)
            {


                // Проверка длины для имени, фамилии и отчества (20 символов)
                if (textBox.Text.Length >= 20 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }

            // Проверяем текстовые поля для логина и пароля
            if (textBox == PasswordBox || textBox == EmailBox || textBox == PochtaBox)
            {
                // Ограничение на 25 символов
                if (textBox.Text.Length >= 25 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }
        }
        private void TextBox_PreviewTextInput2(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidChars(e.Text);
        }


        private void Button_Click(object sender, RoutedEventArgs e)

        {
            string dl = "Администратор";
            MenuAdmin menu = new MenuAdmin(_thisStaffe);
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

        private void IdDojnostBox_TextChanged(object sender, TextChangedEventArgs e)
        {

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
        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
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

        private void comboBoxProfessions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EmailBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dbContext.Dispose();
            this.Close();
        }
    }
}