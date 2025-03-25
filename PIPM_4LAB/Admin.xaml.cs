using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


namespace PIPM_4LAB
{
    public partial class Admin : Window
    {
        private ProductsEntities db = ProductsEntities.GetContext();
        private ObservableCollection<Products> productsList;

        public Admin()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            productsList = new ObservableCollection<Products>(db.Products.ToList());
            UsersDataGrid.ItemsSource = productsList;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один продукт для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<Products> selectedProducts = new List<Products>();

            foreach (var item in UsersDataGrid.SelectedItems)
            {
                if (item is Products product)
                {
                    selectedProducts.Add(product);
                }
            }

            if (selectedProducts.Count == 0)
            {
                MessageBox.Show("Выбраны некорректные элементы. Попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var product in selectedProducts)
            {
                db.Products.Remove(product);
                productsList.Remove(product);
            }

            try
            {
                db.SaveChanges();
                MessageBox.Show("Выбранные товары удалены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Add();
            if (addWindow.ShowDialog() == true)
            {
                productsList.Add(addWindow.NewProduct);
            }
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Products selectedProduct)
            {
                var changeWindow = new Change(selectedProduct);
                if (changeWindow.ShowDialog() == true)
                {
                    UsersDataGrid.Items.Refresh(); 
                }
            }
            else
            {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
