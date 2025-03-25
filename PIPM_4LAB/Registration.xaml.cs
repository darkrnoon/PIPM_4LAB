using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PIPM_4LAB
{
    public partial class Registration : Window
    {
        private ProductsEntities db = ProductsEntities.GetContext(); // Получаем контекст БД

        public Registration()
        {
            InitializeComponent();
        }

        // Обработчик кнопки "Зарегистрироваться"
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string confirmPassword = ConfirmPasswordBox.Password.Trim();
            string firstName = FirstNameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();

            // Проверка email с использованием регулярного выражения
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Введите корректный email (например, example@gmail.com)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка пароля на длину
            if (password.Length < 8)
            {
                MessageBox.Show("Пароль должен быть не менее 8 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка на совпадение пароля и подтверждения пароля
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка, что email уже существует в базе данных
            if (db.Users.Any(u => u.Email == email))
            {
                MessageBox.Show("Этот email уже зарегистрирован", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка имени и фамилии на корректность (только буквы)
            if (!IsValidName(firstName))
            {
                MessageBox.Show("Имя должно содержать только буквы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidName(lastName))
            {
                MessageBox.Show("Фамилия должна содержать только буквы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Добавление нового пользователя в базу данных
            var newUser = new Users
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                RoleID = 2 // Роль обычного пользователя
            };

            db.Users.Add(newUser);
            db.SaveChanges();
            MessageBox.Show("Регистрация прошла успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            // Перенаправление обратно на окно авторизации
            
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        // Проверка формата email с помощью регулярного выражения
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }

        // Проверка имени или фамилии на наличие только букв
        private bool IsValidName(string name)
        {
            string pattern = @"^[a-zA-Zа-яА-ЯёЁ]+$"; // Разрешает буквы на русском и английском, без цифр и других символов
            return Regex.IsMatch(name, pattern);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
