using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace PIPM_4LAB
{
    public partial class MainWindow : Window
    {
        private ProductsEntities db = ProductsEntities.GetContext(); // Получаем контекст БД

        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик кнопки "Войти"
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = EmailTextBox.Text.Trim();
                string password = PasswordBox.Password.Trim();

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Введите email и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка, если пользователь — администратор (RoleID == 1), то пропускаем валидацию email и пароля
                var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

                if (user != null)
                {
                    if (user.RoleID != 1) // Если это не администратор, проверяем email и пароль
                    {
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
                    }

                    MessageBox.Show($"Добро пожаловать, {user.FirstName} {user.LastName}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (user.RoleID == 1) // Администратор
                    {
                        Admin adminWindow = new Admin();
                        adminWindow.Show();
                    }
                    else // Обычный пользователь
                    {
                        User userWindow = new User();
                        userWindow.Show();
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неправильный email или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Проверка формата email с использованием регулярного выражения
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }

        // Обработчик кнопки "Регистрация"
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new Registration();
            registrationWindow.Show();
            this.Close();
        }
    }
}
