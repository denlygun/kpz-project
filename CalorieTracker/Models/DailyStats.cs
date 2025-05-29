using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Models
{
    public class DailyStats
    {
        public DateTime Date { get; set; }
        public double TotalCalories { get; set; }
        public double Goal { get; set; }
        public List<FoodEntry> Entries { get; set; } = new List<FoodEntry>();
    }
}
