using System.Globalization;

namespace Broken_Petrol_Ltd_2
{
    class Pump //each pump will be an object of this class
    {
        public int PumpNumber
        { get; set; }
        public double LpgDispensed
        { get; set; }

        public double DieselDispensed
        { get; set; }

        public double UnleadedDispensed
        { get; set; }

        public bool InUse
        { get; set; }

        public double EfficiencyPercentage
        { get; set; }

        public Pump(int pumpNum)
        {
            PumpNumber = pumpNum;
            LpgDispensed = 0;
            DieselDispensed = 0;
            UnleadedDispensed = 0;
            EfficiencyPercentage = 0;
            InUse = false;

        }
        public String EfficiencyPercentageString()
        {
            return EfficiencyPercentage.ToString("C", CultureInfo.CurrentCulture).Substring(1);
        }


    }
}
