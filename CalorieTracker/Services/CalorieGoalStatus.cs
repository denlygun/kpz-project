using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public class CalorieGoalStatus
    {
        public DateTime Date { get; set; }
        public double TotalCalories { get; set; }
        public double Goal { get; set; }
        public double Difference { get; set; }
        public double Percentage { get; set; }
        public GoalStatusType Status { get; set; }
    }

    public enum GoalStatusType
    {
        UnderGoal,
        OnTrack,
        OverGoal
    }
}
