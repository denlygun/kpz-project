using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieTracker.Models;
using CalorieTracker.Repositories;
using CalorieTracker.Services;

namespace CalorieTracker.Services
{
    public class CalorieCalculatorService : ICalorieCalculatorService
    {
        private readonly FoodEntryRepository _foodEntryRepository;
        private readonly UserProfileRepository _userProfileRepository;

        public CalorieCalculatorService(FoodEntryRepository foodEntryRepository, UserProfileRepository userProfileRepository)
        {
            _foodEntryRepository = foodEntryRepository;
            _userProfileRepository = userProfileRepository;
        }

        public double CalculateDailyCalories(DateTime date)
        {
            return _foodEntryRepository.GetTotalCaloriesForDate(date);
        }

        public DailyStats GetDailyStats(DateTime date)
        {
            var entries = _foodEntryRepository.GetByDate(date).ToList();
            var totalCalories = entries.Sum(e => e.TotalCalories);
            var userProfile = _userProfileRepository.GetProfile();

            return new DailyStats
            {
                Date = date,
                TotalCalories = totalCalories,
                Goal = userProfile.DailyCalorieGoal,
                Entries = entries
            };
        }

        public List<DailyStats> GetWeeklyStats(DateTime startDate)
        {
            var stats = new List<DailyStats>();
            var userProfile = _userProfileRepository.GetProfile();

            for (int i = 0; i < 7; i++)
            {
                var date = startDate.AddDays(i);
                var entries = _foodEntryRepository.GetByDate(date).ToList();
                var totalCalories = entries.Sum(e => e.TotalCalories);

                stats.Add(new DailyStats
                {
                    Date = date,
                    TotalCalories = totalCalories,
                    Goal = userProfile.DailyCalorieGoal,
                    Entries = entries
                });
            }

            return stats;
        }

        public List<DailyStats> GetMonthlyStats(DateTime month)
        {
            var stats = new List<DailyStats>();
            var userProfile = _userProfileRepository.GetProfile();
            var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(month.Year, month.Month, day);
                var entries = _foodEntryRepository.GetByDate(date).ToList();
                var totalCalories = entries.Sum(e => e.TotalCalories);

                stats.Add(new DailyStats
                {
                    Date = date,
                    TotalCalories = totalCalories,
                    Goal = userProfile.DailyCalorieGoal,
                    Entries = entries
                });
            }

            return stats;
        }

        public double CalculateAverageCalories(DateTime startDate, DateTime endDate)
        {
            var entries = _foodEntryRepository.GetByDateRange(startDate, endDate);
            var totalDays = (endDate - startDate).Days + 1;
            var totalCalories = entries.Sum(e => e.TotalCalories);

            return totalDays > 0 ? totalCalories / totalDays : 0;
        }

        public CalorieGoalStatus GetCalorieGoalStatus(DateTime date)
        {
            var totalCalories = CalculateDailyCalories(date);
            var userProfile = _userProfileRepository.GetProfile();
            var goal = userProfile.DailyCalorieGoal;
            var difference = totalCalories - goal;
            var percentage = goal > 0 ? (totalCalories / goal) * 100 : 0;

            return new CalorieGoalStatus
            {
                Date = date,
                TotalCalories = totalCalories,
                Goal = goal,
                Difference = difference,
                Percentage = percentage,
                Status = GetGoalStatusType(percentage)
            };
        }

        private GoalStatusType GetGoalStatusType(double percentage)
        {
            if (percentage < 80)
            {
                return GoalStatusType.UnderGoal;
            }
            else if (percentage >= 80 && percentage <= 120)
            {
                return GoalStatusType.OnTrack;
            }
            else if (percentage > 120)
            {
                return GoalStatusType.OverGoal;
            }
            else
            {
                return GoalStatusType.OnTrack;
            }
        }
    }
}
