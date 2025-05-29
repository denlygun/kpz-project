using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Models
{
    public class UserProfile : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private int _age;
        private double _weight;
        private double _height;
        private Gender _gender;
        private ActivityLevel _activityLevel;
        private double _dailyCalorieGoal;
        private double _dailyCarbsGoal;
        private double _dailyProteinGoal;
        private double _dailyFatGoal;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BasalMetabolicRate));
                OnPropertyChanged(nameof(DailyCalorieNeed));
            }
        }

        public double Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BasalMetabolicRate));
                OnPropertyChanged(nameof(DailyCalorieNeed));
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BasalMetabolicRate));
                OnPropertyChanged(nameof(DailyCalorieNeed));
            }
        }

        public Gender Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BasalMetabolicRate));
                OnPropertyChanged(nameof(DailyCalorieNeed));
            }
        }

        public ActivityLevel ActivityLevel
        {
            get => _activityLevel;
            set
            {
                _activityLevel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DailyCalorieNeed));
            }
        }

        public double DailyCalorieGoal
        {
            get => _dailyCalorieGoal;
            set
            {
                _dailyCalorieGoal = value;
                OnPropertyChanged();
            }
        }
        public double DailyProteinGoal
        {
            get => _dailyProteinGoal;
            set
            {
                _dailyProteinGoal = value;
                OnPropertyChanged();
            }
        }
        public double DailyCarbsGoal
        {
            get => _dailyCarbsGoal;
            set
            {
                _dailyCarbsGoal = value;
                OnPropertyChanged();
            }
        }
        public double DailyFatGoal
        {
            get => _dailyFatGoal;
            set
            {
                _dailyFatGoal = value;
                OnPropertyChanged();
            }
        }

        public double BasalMetabolicRate
        {
            get
            {
                if (Gender == Gender.Male)
                {
                    return 88.362 + (13.397 * Weight) + (4.799 * Height) - (5.677 * Age);
                }
                else
                {
                    return 447.593 + (9.247 * Weight) + (3.098 * Height) - (4.330 * Age);
                }
            }
        }

        public double DailyCalorieNeed
        {
            get
            {
                double multiplier;

                switch (ActivityLevel)
                {
                    case ActivityLevel.Sedentary:
                        multiplier = 1.2;
                        break;
                    case ActivityLevel.LightlyActive:
                        multiplier = 1.375;
                        break;
                    case ActivityLevel.ModeratelyActive:
                        multiplier = 1.55;
                        break;
                    case ActivityLevel.VeryActive:
                        multiplier = 1.725;
                        break;
                    case ActivityLevel.ExtraActive:
                        multiplier = 1.9;
                        break;
                    default:
                        multiplier = 1.2;
                        break;
                }

                return BasalMetabolicRate * multiplier;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum ActivityLevel
    {
        Sedentary,
        LightlyActive,
        ModeratelyActive,
        VeryActive,
        ExtraActive
    }

}
