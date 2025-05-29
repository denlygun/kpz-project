using CalorieTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public class MifflinStJeorCalculator : ICalorieCalculator
    {
        public double CalculateBasalMetabolicRate(UserProfile profile)
        {
            if (profile.Gender == Gender.Male)
            {
                return (10 * profile.Weight) + (6.25 * profile.Height) - (5 * profile.Age) + 5;
            }
            else
            {
                return (10 * profile.Weight) + (6.25 * profile.Height) - (5 * profile.Age) - 161;
            }
        }

        public double CalculateDailyCalorieNeed(UserProfile profile)
        {
            var bmr = CalculateBasalMetabolicRate(profile);
            var multiplier = GetActivityMultiplier(profile.ActivityLevel);
            return bmr * multiplier;
        }

        private double GetActivityMultiplier(ActivityLevel level)
        {
            switch (level)
            {
                case ActivityLevel.Sedentary:
                    return 1.2;
                case ActivityLevel.LightlyActive:
                    return 1.375;
                case ActivityLevel.ModeratelyActive:
                    return 1.55;
                case ActivityLevel.VeryActive:
                    return 1.725;
                case ActivityLevel.ExtraActive:
                    return 1.9;
                default:
                    return 1.2;
            } 
        }
    }

}
