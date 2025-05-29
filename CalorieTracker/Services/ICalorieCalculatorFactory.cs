using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public interface ICalorieCalculatorFactory
    {
        ICalorieCalculator CreateCalculator(CalculatorType type);
    }
}
