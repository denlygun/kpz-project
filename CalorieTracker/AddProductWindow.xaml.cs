using CalorieTracker.Models;
using CalorieTracker.ViewModels;
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

namespace CalorieTracker
{
    public partial class AddProductWindow : Window
    {
        public ProductViewModel ProductViewModel { get; private set; }

        public AddProductWindow(Product product = null)
        {
            InitializeComponent();

            ProductViewModel = new ProductViewModel();
            if (product != null)
            {
                ProductViewModel.FromProduct(product);
                Title = "Редагувати продукт";
            }
            else
            {
                Title = "Додати продукт";
            }

            DataContext = new
            {
                Product = ProductViewModel,
                Categories = Enum.GetValues(typeof(ProductCategory)).Cast<ProductCategory>().ToList()
            };
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductViewModel.Name))
            {
                MessageBox.Show("Введіть назву продукту!", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ProductViewModel.CaloriesPerGram <= 0)
            {
                MessageBox.Show("Введіть коректну кількість калорій!", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
