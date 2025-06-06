using CalorieTracker.Models;
using CalorieTracker.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public class DeleteFoodEntryCommand : FoodEntryCommandBase
    {
        private readonly int _entryId;

        public DeleteFoodEntryCommand(FoodEntryRepository repository, int entryId, ICalorieGoalSubject notificationService)
            : base(repository, notificationService)
        {
            _entryId = entryId;
        }

        protected override void ExecuteInternal()
        {
            Entry = Repository.GetById(_entryId);
            if (Entry != null)
            {
                Repository.Delete(_entryId);
            }
        }

        protected override void UndoInternal()
        {
            if (Entry != null)
            {
                Repository.Add(Entry);
            }
        }
    }

}
