using CalorieTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CalorieTracker.Repositories
{
    public class RecipeRepository : BaseRepository<Recipe>
    {
        private readonly ProductRepository _productRepository;

        public RecipeRepository(ProductRepository productRepository) : base("recipes.json")
        {
            _productRepository = productRepository;
        }

        public override Recipe GetById(int id) => _entities.FirstOrDefault(r => r.Id == id);

        public override void Update(Recipe entity)
        {
            var existing = GetById(entity.Id);
            if (existing != null)
            {
                existing.Name = entity.Name;
                existing.Instructions = entity.Instructions;
                existing.Servings = entity.Servings;
                existing.Ingredients = entity.Ingredients;
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

        protected override void SetEntityId(Recipe entity, int id) => entity.Id = id;
        protected override int GetEntityId(Recipe entity) => entity.Id;

        public IEnumerable<Recipe> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return GetAll();

            return _entities.Where(r => r.Name != null && r.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public override void LoadData()
        {
            base.LoadData();
            foreach (var recipe in _entities)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    if (ingredient.Product?.Id > 0)
                    {
                        var product = _productRepository.GetById(ingredient.Product.Id);
                        if (product != null)
                        {
                            ingredient.Product = product;
                        }
                    }
                }
            }
        }
    }
}
