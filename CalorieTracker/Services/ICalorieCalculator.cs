using CalorieTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public interface ICalorieCalculator
    {
        double CalculateBasalMetabolicRate(UserProfile profile);
        double CalculateDailyCalorieNeed(UserProfile profile);
    }
}
