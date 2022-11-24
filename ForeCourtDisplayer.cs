namespace Broken_Petrol_Ltd_2
{
	partial class Program
	{
		public static void Displayer(Vehicle recentVehicle, int recentPumpNum)
		{
			Console.Clear();
			String[] available = { "Available", "Busy     " }; //both use same number of characters so it should be oriented appropriately
			foreach (Pump[] lane in allLanes)
			{
				foreach (Pump pump in lane)
				{
                    if ((double)dayWatch.ElapsedMilliseconds == 0)
					{
						pump.EfficiencyPercentage = 100;
						continue;
					}
					pump.EfficiencyPercentage = 100 - ((double)unoccupiedCounter[pump.PumpNumber - 1].ElapsedMilliseconds / (double)dayWatch.ElapsedMilliseconds * 100); //percentage based on how long a pump is occupied divided by total time passed (and then taken away from 100 to yield %)
				}
			}
			Console.WriteLine($"This terminal is in use by: {currentUser.Username}");
			Console.WriteLine($"Pump {lane1[2].PumpNumber}: {available[Convert.ToInt32(lane1[2].InUse)]} ({lane1[2].EfficiencyPercentageString()}% efficient) | Pump {lane2[2].PumpNumber}: {available[Convert.ToInt32(lane2[2].InUse)]} ({lane2[2].EfficiencyPercentageString()}% efficient)  | Pump {lane3[2].PumpNumber}: {available[Convert.ToInt32(lane3[2].InUse)]} ({lane3[2].EfficiencyPercentageString()}% efficient)");
			Console.WriteLine($"Pump {lane1[1].PumpNumber}: {available[Convert.ToInt32(lane1[1].InUse)]} ({lane1[1].EfficiencyPercentageString()}% efficient) | Pump {lane2[1].PumpNumber}: {available[Convert.ToInt32(lane2[1].InUse)]} ({lane2[1].EfficiencyPercentageString()}% efficient)  | Pump {lane3[1].PumpNumber}: {available[Convert.ToInt32(lane3[1].InUse)]} ({lane3[1].EfficiencyPercentageString()}% efficient)");
			Console.WriteLine($"Pump {lane1[0].PumpNumber}: {available[Convert.ToInt32(lane1[0].InUse)]} ({lane1[0].EfficiencyPercentageString()}% efficient) | Pump {lane2[0].PumpNumber}: {available[Convert.ToInt32(lane2[0].InUse)]} ({lane2[0].EfficiencyPercentageString()}% efficient)  | Pump {lane3[0].PumpNumber}: {available[Convert.ToInt32(lane3[0].InUse)]} ({lane3[0].EfficiencyPercentageString()}% efficient) \n");
			Console.WriteLine("Vehicles Waiting\n");
			for (int i = 0; i < existingVehicles.Length; i++)
			{
				if (existingVehicles[i] != null)
				{
					Console.WriteLine(existingVehicles[i].Type);
				}
			}
			double unleaded = AllUnleaded();
			double diesel = AllDiesel();
			double lpg = AllLpg();
			currentUser.totalCost = Math.Round(unleaded * UNLEADED_COST + diesel * DIESEL_COST + lpg * LPG_COST, 2);
			currentUser.comission = Math.Round(currentUser.totalCost / 100, 2);
			Console.WriteLine($"Unleaded dispensed: {unleaded} litres");
			Console.WriteLine($"Diesel dispensed: {diesel} litres");
			Console.WriteLine($"LPG dispensed: {lpg} litres\n");
			Console.WriteLine($"Number of Vehicles that have fuelled: {carsFuelled} | Number of Vehicles that have left: {carsLeft}");
			Console.WriteLine($"Total Fuel Price: £{currentUser.CostInString()}");
			Console.WriteLine($"1% Commission £{currentUser.CommissionInString()}");
			if (recentVehicle != null)
			{
				Console.WriteLine($"\nA new {recentVehicle.Type} has started fuelling at Pump {recentPumpNum}");
			}

		}
	}
}