using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public interface ICalorieGoalObserver
    {
        void OnCalorieGoalUpdated(double newGoal);
        void OnDailyCaloriesChanged(DateTime date, double totalCalories);
    }
}
