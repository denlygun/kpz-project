using CalorieTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CalorieTracker.Repositories
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository() : base("products.json") { }

        public override Product GetById(int id) => _entities.FirstOrDefault(p => p.Id == id);

        public override void Update(Product entity)
        {
            var existing = GetById(entity.Id);
            if (existing != null)
            {
                existing.Name = entity.Name;
                existing.CaloriesPerGram = entity.CaloriesPerGram;
                existing.Protein = entity.Protein;
                existing.Carbs = entity.Carbs;
                existing.Fat = entity.Fat;
                existing.Category = entity.Category;
            }
        }

        public override void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _entities.Remove(entity);
            }
        }

        protected override void SetEntityId(Product entity, int id) => entity.Id = id;
        protected override int GetEntityId(Product entity) => entity.Id;

        public IEnumerable<Product> GetByCategory(ProductCategory category)
        {
            return _entities.Where(p => p.Category == category);
        }

        public IEnumerable<Product> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return GetAll();

            return _entities.Where(p => p.Name != null && p.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        protected override void InitializeDefaultData()
        {
            var defaultProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Apple", CaloriesPerGram = 0.52, Protein = 0.3, Carbs = 14, Fat = 0.2, Category = ProductCategory.Fruits },
                new Product { Id = 2, Name = "Banana", CaloriesPerGram = 0.89, Protein = 1.1, Carbs = 23, Fat = 0.3, Category = ProductCategory.Fruits },
                new Product { Id = 3, Name = "Chicken Breast", CaloriesPerGram = 1.65, Protein = 31, Carbs = 0, Fat = 3.6, Category = ProductCategory.Meat },
                new Product { Id = 4, Name = "Rice", CaloriesPerGram = 1.30, Protein = 2.7, Carbs = 28, Fat = 0.3, Category = ProductCategory.Grains },
                new Product { Id = 5, Name = "Milk", CaloriesPerGram = 0.42, Protein = 3.4, Carbs = 5, Fat = 1, Category = ProductCategory.Dairy },
                new Product { Id = 6, Name = "Bread", CaloriesPerGram = 2.65, Protein = 9, Carbs = 49, Fat = 3.2, Category = ProductCategory.Grains },
                new Product { Id = 7, Name = "Egg", CaloriesPerGram = 1.55, Protein = 13, Carbs = 1.1, Fat = 11, Category = ProductCategory.Dairy },
                new Product { Id = 8, Name = "Broccoli", CaloriesPerGram = 0.34, Protein = 2.8, Carbs = 7, Fat = 0.4, Category = ProductCategory.Vegetables },
                new Product { Id = 9, Name = "Pasta", CaloriesPerGram = 1.31, Protein = 5, Carbs = 25, Fat = 0.9, Category = ProductCategory.Grains },
                new Product { Id = 10, Name = "Cheese", CaloriesPerGram = 4.02, Protein = 25, Carbs = 1.3, Fat = 33, Category = ProductCategory.Dairy }
            };

            _entities = defaultProducts.Cast<Product>().ToList();
            _nextId = 11;
        }
    }
}
