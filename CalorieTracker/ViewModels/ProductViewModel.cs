using CalorieTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        private double _caloriesPerGram;
        private double _protein;
        private double _carbs;
        private double _fat;
        private ProductCategory _category;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public double CaloriesPerGram
        {
            get => _caloriesPerGram;
            set => SetProperty(ref _caloriesPerGram, value);
        }

        public double Protein
        {
            get => _protein;
            set => SetProperty(ref _protein, value);
        }

        public double Carbs
        {
            get => _carbs;
            set => SetProperty(ref _carbs, value);
        }

        public double Fat
        {
            get => _fat;
            set => SetProperty(ref _fat, value);
        }

        public ProductCategory Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public Product ToProduct()
        {
            return new Product
            {
                Name = Name,
                CaloriesPerGram = CaloriesPerGram,
                Protein = Protein,
                Carbs = Carbs,
                Fat = Fat,
                Category = Category
            };
        }

        public void FromProduct(Product product)
        {
            Name = product.Name;
            CaloriesPerGram = product.CaloriesPerGram;
            Protein = product.Protein;
            Carbs = product.Carbs;
            Fat = product.Fat;
            Category = product.Category;
        }
    }
}
