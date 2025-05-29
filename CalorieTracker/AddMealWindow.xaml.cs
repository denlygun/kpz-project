using CalorieTracker.Models;
using CalorieTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class AddMealWindow : Window
    {
        public FoodEntryViewModel FoodEntryViewModel { get; private set; }

        public AddMealWindow(ObservableCollection<Product> products, Product selectedProduct = null)
        {
            InitializeComponent();

            FoodEntryViewModel = new FoodEntryViewModel();
            if (selectedProduct != null)
            {
                FoodEntryViewModel.SelectedProduct = selectedProduct;
            }

            DataContext = new
            {
                FoodEntry = FoodEntryViewModel,
                Products = products,
                MealTypes = Enum.GetValues(typeof(MealType)).Cast<MealType>().ToList()
            };
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (FoodEntryViewModel.SelectedProduct == null)
            {
                MessageBox.Show("Оберіть продукт!", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (FoodEntryViewModel.Quantity <= 0)
            {
                MessageBox.Show("Введіть коректну кількість!", "Помилка",
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
