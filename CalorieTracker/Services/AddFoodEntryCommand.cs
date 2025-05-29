using CalorieTracker.Models;
using CalorieTracker.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public class AddFoodEntryCommand : ICommand
    {
        private readonly FoodEntryRepository _repository;
        private readonly FoodEntry _foodEntry;
        private readonly ICalorieGoalSubject _notificationService;

        public AddFoodEntryCommand(FoodEntryRepository repository, FoodEntry foodEntry, ICalorieGoalSubject notificationService)
        {
            _repository = repository;
            _foodEntry = foodEntry;
            _notificationService = notificationService;
        }

        public void Execute()
        {
            _repository.Add(_foodEntry);
            _repository.SaveChanges();
            var totalCalories = _repository.GetTotalCaloriesForDate(_foodEntry.DateTime);
            _notificationService.NotifyDailyCaloriesChanged(_foodEntry.DateTime, totalCalories);
        }

        public void Undo()
        {
            _repository.Delete(_foodEntry.Id);
            _repository.SaveChanges();
            var totalCalories = _repository.GetTotalCaloriesForDate(_foodEntry.DateTime);
            _notificationService.NotifyDailyCaloriesChanged(_foodEntry.DateTime, totalCalories);
        }
    }
}
