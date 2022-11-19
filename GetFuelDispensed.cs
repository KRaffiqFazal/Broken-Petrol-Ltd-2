namespace Broken_Petrol_Ltd_2
{
	partial class Program
	{
		public static double AllUnleaded() //returns a double value of unleaded that has been dispensed through all pumps
		{
			double unleaded = 0;
			foreach (Pump[] lane in allLanes)
			{
				foreach (Pump pump in lane)
				{
					unleaded += pump.UnleadedDispensed;
				}
			}
			return unleaded;
		}
		public static double AllDiesel() //returns a double value of diesel that has been dispensed through all pumps
		{
			double diesel = 0;
			foreach (Pump[] lane in allLanes)
			{
				foreach (Pump pump in lane)
				{
					diesel += pump.DieselDispensed;
				}
			}
			return diesel;
		}
		public static double AllLpg() //returns a double value of LPG that has been dispensed through all pumps
		{
			double lpg = 0;
			foreach (Pump[] lane in allLanes)
			{
				foreach (Pump pump in lane)
				{
					lpg += pump.LpgDispensed;
				}
			}
			return lpg;
		}
		public static double AveragePercentage()
		{
			double averageEfficiency = 0;
			foreach (Pump[] lane in allLanes)
			{
				foreach (Pump pump in lane)
				{
					averageEfficiency += pump.EfficiencyPercentage;
				}
			}
			return Math.Round((double)averageEfficiency / 9, 2);
		}
	}
}
