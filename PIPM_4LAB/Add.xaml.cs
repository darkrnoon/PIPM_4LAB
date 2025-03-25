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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string productName = NameTextBox.Text.Trim();
            string priceText = PriceTextBox.Text.Trim();
            string quantityText = QuantityTextBox.Text.Trim();
            string imageText = ImageTextBox.Text.Trim();

            // Проверка, чтобы название не было пустым и содержало хотя бы одну букву
            if (string.IsNullOrEmpty(productName) || !Regex.IsMatch(productName, @"[A-Za-z]"))
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

            // Проверка на изображение (может содержать буквы и цифры)
            if (!Regex.IsMatch(imageText, @"^[A-Za-z0-9]+$") && !string.IsNullOrEmpty(imageText))
            {
                MessageBox.Show("Изображение должно содержать только буквы и цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewProduct = new Products
            {
                Name = productName,
                Price = price,
                Quantity = quantity,
                Image = imageText
            };

            try
            {
                db.Products.Add(NewProduct);
                db.SaveChanges();
                MessageBox.Show("Товар добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true; // Сигнал Admin о добавлении
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImageTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
