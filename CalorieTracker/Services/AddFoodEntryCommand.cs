using CalorieTracker.Models;
using CalorieTracker.Repositories;

namespace CalorieTracker.Services
{
    public class AddFoodEntryCommand : FoodEntryCommandBase
    {
        public AddFoodEntryCommand(FoodEntryRepository repository, FoodEntry foodEntry, ICalorieGoalSubject notificationService)
            : base(repository, notificationService)
        {
            Entry = foodEntry;
        }

        protected override void ExecuteInternal()
        {
            Repository.Add(Entry);
        }

        protected override void UndoInternal()
        {
            Repository.Delete(Entry.Id);
        }
    }

}
