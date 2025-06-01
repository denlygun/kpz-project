using System;
using System.Collections.Generic;
using System.Linq;
using CalorieTracker.Models;
using CalorieTracker.Repositories;
using CalorieTracker.Services;

namespace CalorieTracker.Services
{
    public class CalorieCalculatorService : ICalorieCalculatorService
    {
        private readonly FoodEntryRepository _foodEntryRepository;
        private readonly UserProfileRepository _userProfileRepository;
        private readonly IGoalStatusCalculator _goalStatusCalculator;
        private readonly IDailyStatsFactory _dailyStatsFactory;

        // Constants for goal status thresholds
        private const double UNDER_GOAL_THRESHOLD = 80.0;
        private const double OVER_GOAL_THRESHOLD = 120.0;

        public CalorieCalculatorService(
            FoodEntryRepository foodEntryRepository,
            UserProfileRepository userProfileRepository,
            IGoalStatusCalculator goalStatusCalculator = null,
            IDailyStatsFactory dailyStatsFactory = null)
        {
            _foodEntryRepository = foodEntryRepository;
            _userProfileRepository = userProfileRepository;
            _goalStatusCalculator = goalStatusCalculator ?? new GoalStatusCalculator();
            _dailyStatsFactory = dailyStatsFactory ?? new DailyStatsFactory();
        }

        public double CalculateDailyCalories(DateTime date)
        {
            return _foodEntryRepository.GetTotalCaloriesForDate(date);
        }

        public DailyStats GetDailyStats(DateTime date)
        {
            var entries = _foodEntryRepository.GetByDate(date).ToList();
            var userProfile = _userProfileRepository.GetProfile();

            return _dailyStatsFactory.CreateDailyStats(date, entries, userProfile.DailyCalorieGoal);
        }

        public List<DailyStats> GetWeeklyStats(DateTime startDate)
        {
            return GetStatsForDateRange(startDate, 7);
        }

        public List<DailyStats> GetMonthlyStats(DateTime month)
        {
            var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);

            return GetStatsForDateRange(firstDayOfMonth, daysInMonth);
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

            return _goalStatusCalculator.CalculateGoalStatus(date, totalCalories, userProfile.DailyCalorieGoal);
        }

        // Extracted method to eliminate code duplication
        private List<DailyStats> GetStatsForDateRange(DateTime startDate, int numberOfDays)
        {
            var stats = new List<DailyStats>();
            var userProfile = _userProfileRepository.GetProfile();

            // Optimize by getting all entries at once instead of multiple repository calls
            var endDate = startDate.AddDays(numberOfDays - 1);
            var allEntries = _foodEntryRepository.GetByDateRange(startDate, endDate)
                .GroupBy(e => e.DateTime.Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            for (int i = 0; i < numberOfDays; i++)
            {
                var date = startDate.AddDays(i);
                var entries = allEntries.ContainsKey(date.Date) ? allEntries[date.Date] : new List<FoodEntry>();

                stats.Add(_dailyStatsFactory.CreateDailyStats(date, entries, userProfile.DailyCalorieGoal));
            }

            return stats;
        }
    }

    // Extracted interface and class for goal status calculation
    public interface IGoalStatusCalculator
    {
        CalorieGoalStatus CalculateGoalStatus(DateTime date, double totalCalories, double goal);
    }

    public class GoalStatusCalculator : IGoalStatusCalculator
    {
        private const double UNDER_GOAL_THRESHOLD = 80.0;
        private const double OVER_GOAL_THRESHOLD = 120.0;

        public CalorieGoalStatus CalculateGoalStatus(DateTime date, double totalCalories, double goal)
        {
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
            if (percentage < UNDER_GOAL_THRESHOLD)
                return GoalStatusType.UnderGoal;

            if (percentage > OVER_GOAL_THRESHOLD)
                return GoalStatusType.OverGoal;

            return GoalStatusType.OnTrack;
        }
    }

    // Extracted interface and class for DailyStats creation
    public interface IDailyStatsFactory
    {
        DailyStats CreateDailyStats(DateTime date, List<FoodEntry> entries, double dailyCalorieGoal);
    }

    public class DailyStatsFactory : IDailyStatsFactory
    {
        public DailyStats CreateDailyStats(DateTime date, List<FoodEntry> entries, double dailyCalorieGoal)
        {
            var totalCalories = entries.Sum(e => e.TotalCalories);

            return new DailyStats
            {
                Date = date,
                TotalCalories = totalCalories,
                Goal = dailyCalorieGoal,
                Entries = entries
            };
        }
    }
}