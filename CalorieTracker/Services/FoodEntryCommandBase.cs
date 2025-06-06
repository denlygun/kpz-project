using CalorieTracker.Models;
using CalorieTracker.Repositories;

namespace CalorieTracker.Services
{
    public abstract class FoodEntryCommandBase : ICommand
    {
        protected readonly FoodEntryRepository Repository;
        protected readonly ICalorieGoalSubject NotificationService;
        protected FoodEntry Entry;

        protected FoodEntryCommandBase(FoodEntryRepository repository, ICalorieGoalSubject notificationService)
        {
            Repository = repository;
            NotificationService = notificationService;
        }

        public void Execute()
        {
            ExecuteInternal();
            Repository.SaveChanges();
            Notify();
        }

        public void Undo()
        {
            UndoInternal();
            Repository.SaveChanges();
            Notify();
        }

        protected void Notify()
        {
            if (Entry != null)
            {
                var totalCalories = Repository.GetTotalCaloriesForDate(Entry.DateTime);
                NotificationService.NotifyDailyCaloriesChanged(Entry.DateTime, totalCalories);
            }
        }

        protected abstract void ExecuteInternal();
        protected abstract void UndoInternal();
    }

}