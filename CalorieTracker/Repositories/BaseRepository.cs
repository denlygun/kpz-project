using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CalorieTracker.Models;

namespace CalorieTracker.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected List<T> _entities = new List<T>();
        protected readonly string _filePath;
        protected int _nextId = 1;

        protected BaseRepository(string fileName)
        {
            var dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CalorieTracker");
            Directory.CreateDirectory(dataFolder);
            _filePath = Path.Combine(dataFolder, fileName);
            LoadData();
        }

        public virtual IEnumerable<T> GetAll() => _entities.ToList();

        public abstract T GetById(int id);

        public virtual void Add(T entity)
        {
            SetEntityId(entity, _nextId++);
            _entities.Add(entity);
        }

        public abstract void Update(T entity);

        public abstract void Delete(int id);

        protected abstract void SetEntityId(T entity, int id);
        protected abstract int GetEntityId(T entity);

        public virtual void SaveChanges()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_entities, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save data to {_filePath}", ex);
            }
        }

       public virtual void LoadData()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    var entities = JsonConvert.DeserializeObject<List<T>>(json);
                    if (entities != null)
                    {
                        _entities = entities;
                        _nextId = _entities.Any() ? _entities.Max(GetEntityId) + 1 : 1;
                    }
                }
                else
                {
                    InitializeDefaultData();
                    SaveChanges();
                }
            }
            catch (Exception ex)
            {
                InitializeDefaultData();
                throw new InvalidOperationException($"Failed to load data from {_filePath}", ex);
            }
        }

        protected virtual void InitializeDefaultData() { }
    }
}
