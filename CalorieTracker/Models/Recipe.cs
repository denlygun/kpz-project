using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CalorieTracker.Models;


namespace CalorieTracker.Models
{
    public class Recipe : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _instructions = string.Empty;
        private int _servings = 1;

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

        public string Instructions
        {
            get => _instructions;
            set
            {
                _instructions = value;
                OnPropertyChanged();
            }
        }

        public int Servings
        {
            get => _servings;
            set
            {
                _servings = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CaloriesPerServing));
            }
        }

        public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();

        public double TotalCalories => Ingredients.Sum(i => i.TotalCalories);

        public double CaloriesPerServing => Servings > 0 ? TotalCalories / Servings : 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
