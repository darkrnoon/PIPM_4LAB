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
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = ProductNameTextBox.Text.Trim();
            string priceText = ProductPriceTextBox.Text.Trim();
            string quantityText = ProductQuantityTextBox.Text.Trim();

            // Проверка, чтобы название не было пустым и содержало хотя бы одну букву
            if (string.IsNullOrEmpty(newName) || !Regex.IsMatch(newName, @"[A-Za-z]"))
            {
                MessageBox.Show("Название товара должно содержать хотя бы одну букву.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка на цену (должна быть числом)
            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка на количество (должно быть числом)
            if (!int.TryParse(quantityText, out int quantity))
            {
                MessageBox.Show("Количество должно быть целым числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Обновляем все данные, включая название
                product.Name = newName;
                product.Price = price;
                product.Quantity = quantity;

                db.SaveChanges();
                MessageBox.Show("Товар изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true; // Сигнал для обновления DataGrid
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
