using CalorieTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CalorieTracker.Repositories
{
    public class FoodEntryRepository : BaseRepository<FoodEntry>
    {
        private readonly ProductRepository _productRepository;

        public FoodEntryRepository(ProductRepository productRepository) : base("food_entries.json")
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public override FoodEntry GetById(int id) => _entities.FirstOrDefault(e => e.Id == id);

        public override void Update(FoodEntry entity)
        {
            var existing = GetById(entity.Id);
            if (existing != null)
            {
                existing.Product = entity.Product;
                existing.Quantity = entity.Quantity;
                existing.DateTime = entity.DateTime;
                existing.MealType = entity.MealType;
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

        protected override void SetEntityId(FoodEntry entity, int id) => entity.Id = id;
        protected override int GetEntityId(FoodEntry entity) => entity.Id;

        public IEnumerable<FoodEntry> GetByDate(DateTime date)
        {
            return _entities.Where(e => e.DateTime.Date == date.Date);
        }

        public IEnumerable<FoodEntry> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return _entities.Where(e => e.DateTime.Date >= startDate.Date && e.DateTime.Date <= endDate.Date);
        }

        public IEnumerable<FoodEntry> GetByMealType(MealType mealType)
        {
            return _entities.Where(e => e.MealType == mealType);
        }

        public double GetTotalCaloriesForDate(DateTime date)
        {
            return GetByDate(date).Sum(e => e.TotalCalories);
        }

        public override void LoadData()
        {
            base.LoadData();

            if (_entities == null) return;

            foreach (var entry in _entities.ToList()) 
            {
                if (entry?.Product?.Id > 0)
                {
                    try
                    {
                        entry.Product = _productRepository.GetById(entry.Product.Id) ?? entry.Product;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading product: {ex.Message}");
                    }
                }
            }
        }
    }

}
