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
    public partial class EditMealWindow : Window
    {
        public FoodEntryViewModel FoodEntryViewModel { get; private set; }
        private FoodEntry _originalEntry;

        public EditMealWindow(FoodEntry entry, ObservableCollection<Product> products)
        {
            InitializeComponent();

            _originalEntry = entry;
            FoodEntryViewModel = new FoodEntryViewModel
            {
                SelectedProduct = entry.Product,
                Quantity = entry.Quantity,
                MealType = entry.MealType
            };

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

            _originalEntry.Product = FoodEntryViewModel.SelectedProduct;
            _originalEntry.Quantity = FoodEntryViewModel.Quantity;
            _originalEntry.MealType = FoodEntryViewModel.MealType;

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
