using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalorieTracker.Models
{
    public class Product : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private double _caloriesPerGram;
        private double _protein;
        private double _carbs;
        private double _fat;

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public double CaloriesPerGram
        {
            get => _caloriesPerGram;
            set => SetField(ref _caloriesPerGram, value);
        }

        public double Protein
        {
            get => _protein;
            set => SetField(ref _protein, value);
        }

        public double Carbs
        {
            get => _carbs;
            set => SetField(ref _carbs, value);
        }

        public double Fat
        {
            get => _fat;
            set => SetField(ref _fat, value);
        }

        public ProductCategory Category { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public enum ProductCategory
    {
        Fruits,
        Vegetables,
        Meat,
        Dairy,
        Grains,
        Beverages,
        Snacks,
        Other
    }
}
