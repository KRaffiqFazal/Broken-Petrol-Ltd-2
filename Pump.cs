namespace Broken_Petrol_Ltd_2
{
    class Pump //each pump will be an object of this class
    {
        //implement new feature where pumps have finite amount of fuel that can be dispensed, if it runs out the pump cannot be used on that day
        public int PumpNumber;
        public double LpgDispensed = 0;
        public double DieselDispensed = 0;
        public double UnleadedDispensed = 0;
        public double TotalUnleadedLeft = 400; //if this reaches 0, the pump cannot be used anymore for unleaded fuels
        public double TotalDieselLefft = 400;
        public double TotalLpgLeft = 400;
        public bool InUse = false;

        public Pump(int pumpNum)
        {
            PumpNumber = pumpNum;

        }


    }
}
