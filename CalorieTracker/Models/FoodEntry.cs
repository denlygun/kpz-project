using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Models
{
    public class FoodEntry : INotifyPropertyChanged
    {
        private Product _product = new Product();
        private double _quantity;

        public int Id { get; set; }

        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalCalories));
            }
        }

        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalCalories));
            }
        }

        public DateTime DateTime { get; set; }
        public MealType MealType { get; set; }

        public double TotalCalories => Product.CaloriesPerGram * Quantity;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum MealType
    {
        Breakfast,
        Lunch,
        Dinner,
        Snack
    }
}
