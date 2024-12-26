using AlmazAucceon_3_.menu;
using AlmazAucceon_3_.Model2;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace AlmazAucceon_3_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            RefreshUsers();
        }
        public List<User> RefreshUsers()
        {
            using (var newDbContext = new AuctionContext())
            {
                return newDbContext.Users.AsNoTracking().ToList(); // Возвращает актуальные данные
            }
        }
        private void AutButton_Click(object sender, RoutedEventArgs e)
        {

            string log = loginText.Text;
            string pastext = PasswordTextBox.Text;
            string pass = PasswordBox.Password;
            Personalisation(log, pastext, pass);
        }
        private void Personalisation(string login, string passwordText, string password)
        {
            if (string.IsNullOrWhiteSpace(password) && string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Заполните поля", "Ошибка.");
            }
            else if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Пожалуйста, введите логин.");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(password))
            {

                MessageBox.Show("Пожалуйста, введите пароль.");
                return; // Exit the method to prevent further processing
            }

            else
            {
                ProvetkaTextBox(login, passwordText, password);
            }
        }
        private void ProvetkaTextBox(string login, string passwordText, string password)
        {

            if (!AreAllValidChars1(login))
            {
                MessageBox.Show("Логин содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                loginText.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars1(passwordText))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordTextBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars2(password))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordBox.Clear(); // Очищаем TextBox
                return;
            }
            else
            {
                Authtorization(login, passwordText, password);
            }
        }
        private void Authtorization(string login, string passwordText, string password)
        {
            var satrudneke2 = App.context.Staffes.ToList().Find(u => u.StaffPsswords == password && u.StaffLogin == login);

            User user = App.context.Users.ToList().Find(u => u.Psswords == password && u.Email == login);
            if (satrudneke2 != null)
            {
                if (satrudneke2.IdStaffRole == 1)
                {
                    MessageBox.Show($"Добро пожаловать, {satrudneke2.StaffLastname} {satrudneke2.StaffName} {satrudneke2.StaffPatronymic} !");

                    MenuAdmin menuAdmin = new MenuAdmin(satrudneke2);

                    menuAdmin.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Добро пожаловать, {satrudneke2.StaffLastname} {satrudneke2.StaffName} {satrudneke2.StaffPatronymic} !");

                    MenuM menuAdmin = new MenuM(satrudneke2);

                    menuAdmin.Show();
                    this.Close();
                }
            }


            else if (user != null)
            {
                string NameD = "";
                string LastnameD = "";
                string PatronymicD = "";
                string Dol = "";

                MessageBox.Show($"Рады вас видеть, {user.Lastname} {user.UserName} {user.Patronymic} !");
                Dol = "Участник";
                MenuByer menuByer = new MenuByer(satrudneke2);
                menuByer.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }
        private bool AreAllValidChars1(string text)
        {

            foreach (char c in text)
            {
                if (loginText.Text.Length > 20)
                {

                    return false;

                }
            }
            return true; // Все символы допустимы
        }

        private bool AreAllValidChars2(string text)
        {

            foreach (char c in text)
            {
                if (PasswordBox.Password.Length > 20)
                {

                    return false;

                }
            }
            return true; // Все символы допустимы
        }
        private void ViewPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Показываем пароль в TextBox и скрываем PasswordBox
            PasswordTextBox.Text = PasswordBox.Password;
            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Hidden;
        }
        private void PasswordTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Обновляем пароль в PasswordBox при изменении текста в TextBox
            if (ViewPasswordCheckBox.IsChecked == true)
            {
                PasswordBox.Password = PasswordTextBox.Text;
            }
        }

        private void ViewPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Скрываем TextBox и показываем PasswordBox
            PasswordBox.Password = PasswordTextBox.Text; // Сохраняем введенный текст в PasswordBox
            PasswordTextBox.Visibility = Visibility.Hidden;
            PasswordBox.Visibility = Visibility.Visible;
        }

        private void registrationButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void loginText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void MainTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            var pas = sender as PasswordBox;
            if (e.Key == Key.Space)
            {
                e.Handled = true; // Запретить ввод пробела
                return;
            }
            if (textBox == loginText)
            {
                if (textBox.Text.Length >= 20 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }
            else if (textBox == PasswordTextBox)
            {
                if (textBox.Text.Length >= 20 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }
            else if (pas == PasswordBox)
            {
                if (pas.Password.Length >= 20 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
