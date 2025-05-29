using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public interface ICalorieGoalSubject
    {
        void Subscribe(ICalorieGoalObserver observer);
        void Unsubscribe(ICalorieGoalObserver observer);
        void NotifyCalorieGoalUpdated(double newGoal);
        void NotifyDailyCaloriesChanged(DateTime date, double totalCalories);
    }
}
