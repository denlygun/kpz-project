using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public class CalorieGoalNotificationService : ICalorieGoalSubject
    {
        private readonly List<ICalorieGoalObserver> _observers = new List<ICalorieGoalObserver>();

        public void Subscribe(ICalorieGoalObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Unsubscribe(ICalorieGoalObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyCalorieGoalUpdated(double newGoal)
        {
            foreach (var observer in _observers)
            {
                observer.OnCalorieGoalUpdated(newGoal);
            }
        }

        public void NotifyDailyCaloriesChanged(DateTime date, double totalCalories)
        {
            foreach (var observer in _observers)
            {
                observer.OnDailyCaloriesChanged(date, totalCalories);
            }
        }
    }
}
