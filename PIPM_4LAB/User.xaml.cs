using System;
using System.Linq;
using System.Windows;


namespace PIPM_4LAB
{
    /// <summary>
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : Window
    {
        private ProductsEntities db = ProductsEntities.GetContext();

        public User()
        {
            InitializeComponent();
            LoadProducts(); 
        }

        private void LoadProducts()
        {
            try
            {
                var products = db.Products.ToList();
                UsersDataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
