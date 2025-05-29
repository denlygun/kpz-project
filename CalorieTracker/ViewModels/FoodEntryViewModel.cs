using CalorieTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.ViewModels
{
    public class FoodEntryViewModel : ViewModelBase
    {
        private Product _selectedProduct = new Product();
        private double _quantity;
        private MealType _mealType;

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (SetProperty(ref _selectedProduct, value))
                {
                    OnPropertyChanged(nameof(TotalCalories));
                }
            }
        }

        public double Quantity
        {
            get => _quantity;
            set
            {
                if (SetProperty(ref _quantity, value))
                {
                    OnPropertyChanged(nameof(TotalCalories));
                }
            }
        }

        public MealType MealType
        {
            get => _mealType;
            set => SetProperty(ref _mealType, value);
        }

        public double TotalCalories => SelectedProduct?.CaloriesPerGram * Quantity ?? 0;
    }
}
