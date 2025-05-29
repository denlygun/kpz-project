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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalorieTracker
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(_viewModel.UserProfile);
            if (settingsWindow.ShowDialog() == true)
            {
                _viewModel.UserProfile = settingsWindow.UserProfile;
                _viewModel.SaveUserProfileCommand.Execute(null);
            }
        }

        private void AddMealButton_Click(object sender, RoutedEventArgs e)
        {
            var addMealWindow = new AddMealWindow(_viewModel.Products);
            if (addMealWindow.ShowDialog() == true)
            {
                _viewModel.AddFoodEntryCommand.Execute(addMealWindow.FoodEntryViewModel);
            }
        }

        private void EditEntryButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var entry = button?.DataContext as FoodEntry;
            if (entry != null)
            {
                var editWindow = new EditMealWindow(entry, _viewModel.Products);
                if (editWindow.ShowDialog() == true)
                {
                    _viewModel.RefreshDataCommand.Execute(null);
                }
            }
        }

        private void DeleteEntryButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var entry = button?.DataContext as FoodEntry;
            if (entry != null)
            {
                var result = MessageBox.Show($"Видалити запис '{entry.Product.Name}'?",
                    "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.DeleteFoodEntryCommand.Execute(entry);
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchTextBox.Text;
        }

        private void AddFoodButton_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProductWindow();
            if (addProductWindow.ShowDialog() == true)
            {
                _viewModel.AddProductCommand.Execute(addProductWindow.ProductViewModel.ToProduct());
            }
        }

        private void EditFoodButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var product = button?.DataContext as Product;
            if (product != null)
            {
                var editWindow = new AddProductWindow(product);
                if (editWindow.ShowDialog() == true)
                {
                    _viewModel.EditProductCommand.Execute(editWindow.ProductViewModel.ToProduct());
                }
            }
        }

        private void AddToMealButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var product = button?.DataContext as Product;
            if (product != null)
            {
                var addMealWindow = new AddMealWindow(_viewModel.Products, product);
                if (addMealWindow.ShowDialog() == true)
                {
                    _viewModel.AddFoodEntryCommand.Execute(addMealWindow.FoodEntryViewModel);
                }
            }
        }

        private void DeleteFoodButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var product = button?.DataContext as Product;
            if (product != null)
            {
                var result = MessageBox.Show($"Видалити продукт '{product.Name}'?",
                    "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.DeleteProductCommand.Execute(product);
                }
            }
        }

        private void ScanBarcodeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функція сканування штрих-коду буде додана в майбутньому!",
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ViewReportsButton_Click(object sender, RoutedEventArgs e)
        {
  
            var tabControl = FindName("MainTabControl") as System.Windows.Controls.TabControl;
            if (tabControl != null)
            {
                tabControl.SelectedIndex = 2;
            }
        }

        private void UpdateGoalsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsButton_Click(sender, e);
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Звіт генерується...", "Інформація",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportDataButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                DefaultExt = "csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                MessageBox.Show($"Дані експортовано до {saveFileDialog.FileName}",
                    "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
