using CalorieTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Repositories
{
    public class UserProfileRepository
    {
        private readonly string _filePath;
        private UserProfile _profile;

        public UserProfileRepository()
        {
            var dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CalorieTracker");
            Directory.CreateDirectory(dataFolder);
            _filePath = Path.Combine(dataFolder, "user_profile.json");
            LoadProfile();
        }

        public UserProfile GetProfile()
        {
            if (_profile == null)
            {
                _profile = CreateDefaultProfile();
            }
            return _profile;
        }

        public void SaveProfile(UserProfile profile)
        {
            _profile = profile;
            try
            {
                var json = JsonConvert.SerializeObject(profile, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save user profile", ex);
            }
        }

        private void LoadProfile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    _profile = JsonConvert.DeserializeObject<UserProfile>(json);
                }
            }
            catch (Exception ex)
            {
                _profile = CreateDefaultProfile();
                throw new InvalidOperationException($"Failed to load user profile", ex);
            }
        }

        private UserProfile CreateDefaultProfile()
        {
            return new UserProfile
            {
                Name = "User",
                Age = 25,
                Weight = 70,
                Height = 170,
                Gender = Gender.Male,
                ActivityLevel = ActivityLevel.ModeratelyActive,
                DailyCalorieGoal = 2000
            };
        }
    }
}
