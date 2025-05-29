using CalorieTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieTracker.Repositories;

namespace CalorieTracker.Services
{
    public interface ICalorieCalculatorService
    {
        double CalculateDailyCalories(DateTime date);
        DailyStats GetDailyStats(DateTime date);
        List<DailyStats> GetWeeklyStats(DateTime startDate);
        List<DailyStats> GetMonthlyStats(DateTime month);
        double CalculateAverageCalories(DateTime startDate, DateTime endDate);
        CalorieGoalStatus GetCalorieGoalStatus(DateTime date);
    }
}
