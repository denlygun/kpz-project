using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Services
{
    public class CalorieCalculatorFactory : ICalorieCalculatorFactory
    {
        public ICalorieCalculator CreateCalculator(CalculatorType type)
        {
            switch (type)
            {
                case CalculatorType.HarrisBenedict:
                    return new HarrisBenedictCalculator();
                case CalculatorType.MifflinStJeor:
                    return new MifflinStJeorCalculator();
                case CalculatorType.KatchMcArdle:
                    return new KatchMcArdleCalculator();
                default:
                    return new HarrisBenedictCalculator();
            }
        }
    }

    public enum CalculatorType
    {
        HarrisBenedict,
        MifflinStJeor,
        KatchMcArdle
    }
}
