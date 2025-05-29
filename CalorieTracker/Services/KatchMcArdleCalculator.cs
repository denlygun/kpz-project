using CalorieTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public class KatchMcArdleCalculator : ICalorieCalculator
    {
        public double CalculateBasalMetabolicRate(UserProfile profile)
        {
            var estimatedBodyFat = EstimateBodyFatPercentage(profile);
            var leanBodyMass = profile.Weight * (1 - estimatedBodyFat / 100);
            return 370 + (21.6 * leanBodyMass);
        }

        public double CalculateDailyCalorieNeed(UserProfile profile)
        {
            var bmr = CalculateBasalMetabolicRate(profile);
            var multiplier = GetActivityMultiplier(profile.ActivityLevel);
            return bmr * multiplier;
        }

        private double EstimateBodyFatPercentage(UserProfile profile)
        {
            if (profile.Gender == Gender.Male)
            {
                switch (profile.ActivityLevel)
                {
                    case ActivityLevel.Sedentary:
                        return 20;
                    case ActivityLevel.LightlyActive:
                        return 18;
                    case ActivityLevel.ModeratelyActive:
                        return 15;
                    case ActivityLevel.VeryActive:
                        return 12;
                    case ActivityLevel.ExtraActive:
                        return 10;
                    default:
                        return 15;
                }
            }
            else
            {
                switch (profile.ActivityLevel)
                {
                    case ActivityLevel.Sedentary:
                        return 28;
                    case ActivityLevel.LightlyActive:
                        return 25;
                    case ActivityLevel.ModeratelyActive:
                        return 22;
                    case ActivityLevel.VeryActive:
                        return 18;
                    case ActivityLevel.ExtraActive:
                        return 15;
                    default:
                        return 22;
                }
            }
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
