using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Win32;

namespace PIPM_4LAB
{
    public partial class Add : Window
    {
        private ProductsEntities db = ProductsEntities.GetContext();
        public Products NewProduct { get; private set; }

        public Add()
        {
            InitializeComponent();
        }
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImageTextBox.Text = openFileDialog.FileName;
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string productName = NameTextBox.Text.Trim();
            string priceText = PriceTextBox.Text.Trim();
            string quantityText = QuantityTextBox.Text.Trim();
            string imageText = ImageTextBox.Text.Trim();

            if (string.IsNullOrEmpty(productName))
            {
                MessageBox.Show("Название товара должно содержать хотя бы одну букву.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(quantityText, out int quantity))
            {
                MessageBox.Show("Количество должно быть целым числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(imageText))
            {
                Uri imageUri = null;
                bool isValidUri = Uri.TryCreate(imageText, UriKind.Absolute, out imageUri) &&
                                  (imageUri.Scheme == Uri.UriSchemeHttp || imageUri.Scheme == Uri.UriSchemeHttps || imageUri.Scheme == Uri.UriSchemeFile);

                if (!isValidUri)
                {
                    MessageBox.Show("Некорректный путь к изображению. Убедитесь, что путь правильный.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            try
            {
                var product = new Products
                {
                    Name = productName,
                    Price = price,
                    Quantity = quantity,
                    Image = imageText  
                };

                db.Products.Add(product);
                db.SaveChanges();

                NewProduct = product;
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
