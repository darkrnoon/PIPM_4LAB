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

namespace PIPM_4LAB
{
    /// <summary>
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : Window
    {
        private ProductsEntities db = ProductsEntities.GetContext(); // Получаем контекст БД

        public User()
        {
            InitializeComponent();
            LoadProducts(); // Загрузка данных продуктов при открытии окна
        }

        // Метод для загрузки данных продуктов в DataGrid
        private void LoadProducts()
        {
            try
            {
                // Получаем все товары из базы данных
                var products = db.Products.ToList();

                // Заполняем DataGrid данными
                UsersDataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки "Выйти"
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем текущее окно и возвращаемся в окно авторизации
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
