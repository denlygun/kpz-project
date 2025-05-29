using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public double CaloriesPerGram
        {
            get => _caloriesPerGram;
            set
            {
                _caloriesPerGram = value;
                OnPropertyChanged();
            }
        }

        public double Protein
        {
            get => _protein;
            set
            {
                _protein = value;
                OnPropertyChanged();
            }
        }

        public double Carbs
        {
            get => _carbs;
            set
            {
                _carbs = value;
                OnPropertyChanged();
            }
        }

        public double Fat
        {
            get => _fat;
            set
            {
                _fat = value;
                OnPropertyChanged();
            }
        }

        public ProductCategory Category { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
