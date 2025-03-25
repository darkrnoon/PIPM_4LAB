using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace PIPM_4LAB
{
    public partial class Change : Window
    {
        private ProductsEntities db = ProductsEntities.GetContext();
        private Products product;

        public Change(Products selectedProduct)
        {
            InitializeComponent();
            product = selectedProduct;
            LoadProductData();
        }

        private void LoadProductData()
        {
            ProductNameTextBox.Text = product.Name;
            ProductPriceTextBox.Text = product.Price.ToString();
            ProductQuantityTextBox.Text = product.Quantity.ToString();
            ProductImageTextBox.Text = product.Image;
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = ProductNameTextBox.Text.Trim();
            string priceText = ProductPriceTextBox.Text.Trim();
            string quantityText = ProductQuantityTextBox.Text.Trim();
            string newImagePath = ProductImageTextBox.Text.Trim();

            if (string.IsNullOrEmpty(newName))
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

            if (!string.IsNullOrEmpty(newImagePath))
            {
                Uri imageUri;
                bool isValidUri = Uri.TryCreate(newImagePath, UriKind.Absolute, out imageUri) &&
                                  (imageUri.Scheme == Uri.UriSchemeHttp || imageUri.Scheme == Uri.UriSchemeHttps || imageUri.Scheme == Uri.UriSchemeFile);

                if (!isValidUri)
                {
                    MessageBox.Show("Некорректный путь или URL изображения. Проверьте ввод.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            try
            {
                product.Name = newName;
                product.Price = price;
                product.Quantity = quantity;
                product.Image = newImagePath;

                db.SaveChanges();
                MessageBox.Show("Товар изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

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
