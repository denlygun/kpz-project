using CalorieTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalorieTracker
{
    public partial class SettingsWindow : Window
    {
        public UserProfile UserProfile { get; private set; }

        public SettingsWindow(UserProfile userProfile)
        {
            InitializeComponent();

            UserProfile = new UserProfile
            {
                Name = userProfile.Name,
                Age = userProfile.Age,
                Gender = userProfile.Gender,
                Height = userProfile.Height,
                Weight = userProfile.Weight,
                ActivityLevel = userProfile.ActivityLevel,
                DailyCalorieGoal = userProfile.DailyCalorieGoal
            };

            DataContext = new
            {
                UserProfile = UserProfile,
                Genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList(),
                ActivityLevels = Enum.GetValues(typeof(ActivityLevel)).Cast<ActivityLevel>().ToList()
            };
        }

        private void CalculateGoalButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserProfile.Height > 0 && UserProfile.Weight > 0 && UserProfile.Age > 0)
            {
                double tdee = UserProfile.DailyCalorieNeed;

                UserProfile.DailyCalorieGoal = tdee;

                MessageBox.Show(
                    $"Розрахована ціль: {UserProfile.DailyCalorieGoal:F0} калорій на день\n" +
                    $"Базовий метаболізм: {UserProfile.BasalMetabolicRate:F0} калорій\n" +
                    $"TDEE: {tdee:F0} калорій",
                    "Розрахунок завершено", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Заповніть всі поля для розрахунку!", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserProfile.Name))
            {
                MessageBox.Show("Введіть ім'я!", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (UserProfile.DailyCalorieGoal <= 0)
            {
                MessageBox.Show("Введіть коректну ціль калорій!", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
