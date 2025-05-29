using CalorieTracker.Models;
using CalorieTracker.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public class DeleteFoodEntryCommand : ICommand
    {
        private readonly FoodEntryRepository _repository;
        private readonly int _entryId;
        private readonly ICalorieGoalSubject _notificationService;
        private FoodEntry _deletedEntry;

        public DeleteFoodEntryCommand(FoodEntryRepository repository, int entryId, ICalorieGoalSubject notificationService)
        {
            _repository = repository;
            _entryId = entryId;
            _notificationService = notificationService;
        }

        public void Execute()
        {
            _deletedEntry = _repository.GetById(_entryId);
            if (_deletedEntry != null)
            {
                var date = _deletedEntry.DateTime;
                _repository.Delete(_entryId);
                _repository.SaveChanges();
                var totalCalories = _repository.GetTotalCaloriesForDate(date);
                _notificationService.NotifyDailyCaloriesChanged(date, totalCalories);
            }
        }

        public void Undo()
        {
            if (_deletedEntry != null)
            {
                _repository.Add(_deletedEntry);
                _repository.SaveChanges();
                var totalCalories = _repository.GetTotalCaloriesForDate(_deletedEntry.DateTime);
                _notificationService.NotifyDailyCaloriesChanged(_deletedEntry.DateTime, totalCalories);
            }
        }
    }
}
