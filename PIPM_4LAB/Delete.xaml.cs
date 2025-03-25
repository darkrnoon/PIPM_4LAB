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
    /// Логика взаимодействия для Delete.xaml
    /// </summary>
    public partial class Delete : Window
    {
        private ProductsEntities db = ProductsEntities.GetContext(); // Получаем контекст БД

        public Delete()
        {
            InitializeComponent();
        }

        // Обработчик кнопки "Удалить товар"
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string productName = ProductNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(productName))
            {
                MessageBox.Show("Введите название товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var product = db.Products.FirstOrDefault(p => p.Name == productName);

                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                    MessageBox.Show("Товар удален успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Товар не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки "Назад"
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
