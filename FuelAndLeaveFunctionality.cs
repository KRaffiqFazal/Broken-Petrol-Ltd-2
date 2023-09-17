using System.Globalization;

namespace Broken_Petrol_Ltd_2
{
	partial class Program
	{
		public static void Assigner() //a method that will run throughout the entire program to ensure that vehicles are assigned as they come in
		{
			//assign vehicles to an available pump
			for (int i = 0; i < existingVehicles.Length; i++)
			{
				if (existingVehicles[i] == null)
				{
					continue; //if the waiting slot is not being occupied, there is no vehicle there to check, therefore the index should be skipped
				}
				if (!existingVehicles[i].isFuelling && !existingVehicles[i].hasWaited)
				{

					foreach (Pump[] lane in allLanes)
					{
						if (lane[0].InUse) //if the first pump in the lane is not free, automatically check the next lane
						{
							continue;
						}
						else if (lane[1].InUse && !lane[0].InUse && existingVehicles[i] != null) //if the second pump in the lane is being used but the first pump is not, go to first pump
						{
							fuelling[lane[0].PumpNumber - 1] = existingVehicles[i];
							fuelling[lane[0].PumpNumber - 1].StartingFuelling();
							unoccupiedCounter[lane[0].PumpNumber - 1].Stop(); //pauses stopwatch when a vehicle starts fuelling
							lane[0].InUse = true;
							existingVehicles[i] = null;
							SortQueue();
							AddFuel(0, lane);
							Displayer(fuelling[lane[0].PumpNumber - 1], lane[0].PumpNumber);
							carsFuelled++;
						}
						else if (lane[2].InUse && !lane[1].InUse && existingVehicles[i] != null) //this needs to be fixed
						{
							fuelling[lane[1].PumpNumber - 1] = existingVehicles[i];
							fuelling[lane[1].PumpNumber - 1].StartingFuelling();
                            				unoccupiedCounter[lane[1].PumpNumber - 1].Stop(); //pauses stopwatch when a vehicle starts fuelling
                            				lane[1].InUse = true;
							existingVehicles[i] = null;
                            				SortQueue();
                            				AddFuel(1, lane);
							Displayer(fuelling[lane[1].PumpNumber - 1], lane[1].PumpNumber);
							carsFuelled++;
						}
						else if (!lane[2].InUse && !lane[1].InUse && !lane[0].InUse && existingVehicles[i] != null) //if all pumps are free, furthest will be selected to fuel at.
						{
							fuelling[lane[2].PumpNumber - 1] = existingVehicles[i];
							fuelling[lane[2].PumpNumber - 1].StartingFuelling();
				                        unoccupiedCounter[lane[2].PumpNumber - 1].Stop(); //pauses stopwatch when a vehicle starts fuelling
				                        lane[2].InUse = true;
							existingVehicles[i] = null;
				                        SortQueue();
				                        AddFuel(2, lane);
							Displayer(fuelling[lane[2].PumpNumber - 1], lane[2].PumpNumber);
							carsFuelled++;
						}

					}
				}
			}
		}
		public static void AddFuel(int laneNum, Pump[] lane)
		{
			if (fuelling[lane[laneNum].PumpNumber - 1].fuelType.Equals("Unleaded"))
			{
				lane[laneNum].UnleadedDispensed += (fuelling[lane[laneNum].PumpNumber - 1].fuellingTimeInt / 1000) * 1.5; //based on how long the vehicle was meant to fuel for, divided by 1000 to get the time in seconds and multiplied by 1.5 to work out how much fuel was dispensed
				todayLog.Add(fuelling[lane[laneNum].PumpNumber - 1].Type + " has fuelled at Pump: " + lane[laneNum].PumpNumber + " and has purchased £" + ((fuelling[lane[laneNum].PumpNumber - 1].fuellingTimeInt / 1000) * 1.5 * UNLEADED_COST).ToString("C", CultureInfo.CurrentCulture).Substring(1) + " of unleaded" + Environment.NewLine);
			}
			else if (fuelling[lane[laneNum].PumpNumber - 1].fuelType.Equals("Diesel"))
			{
				lane[laneNum].DieselDispensed += (fuelling[lane[laneNum].PumpNumber - 1].fuellingTimeInt / 1000) * 1.5;
               todayLog.Add(fuelling[lane[laneNum].PumpNumber - 1].Type + " has fuelled at Pump: " + lane[laneNum].PumpNumber + " and has purchased £" + ((fuelling[lane[laneNum].PumpNumber - 1].fuellingTimeInt / 1000) * 1.5 * DIESEL_COST).ToString("C", CultureInfo.CurrentCulture).Substring(1) + " of diesel" + Environment.NewLine);
            }
			else
			{
				lane[laneNum].LpgDispensed += (fuelling[lane[laneNum].PumpNumber - 1].fuellingTimeInt / 1000) * 1.5;
                todayLog.Add(fuelling[lane[laneNum].PumpNumber - 1].Type + " has fuelled at Pump: " + lane[laneNum].PumpNumber + " and has purchased £" + ((fuelling[lane[laneNum].PumpNumber - 1].fuellingTimeInt / 1000) * 1.5 * LPG_COST).ToString("C", CultureInfo.CurrentCulture).Substring(1) + " of LPG" + Environment.NewLine);
				
            }
		}
		public static void WaitKick() //an method that will run throughout the entire program to ensure that vehicles are kicked from the list if they finish waiting
		{

			//checks list of vehicles to see which want to leave
			for (int i = 0; i < existingVehicles.Length; i++)
			{
				if (existingVehicles[i] != null)
				{
					if (existingVehicles[i].hasWaited && !existingVehicles[i].isFuelling)
					{
						existingVehicles[i] = null;
						carsLeft++;
						Displayer(null, 0);
					}
				}
			}
		}
		public static void FuelledKick() //kicks vehicles from waiting list once they start fuelling as they will be at pump
		{
			//checks list of vehicles to see which have started fuelling and so can be kicked
			for (int i = 0; i < existingVehicles.Length; i++)
			{
				if (existingVehicles[i] != null)
				{
					if (existingVehicles[i].isFuelling)
					{
						existingVehicles[i] = null;
						Displayer(null, 0);
					}
				}
			}
		}
		public static void NotFuellingAnymore() //checks to see if a vehicle's isFuelled boolean is true,
		{
			int temp;
			foreach (Vehicle item in fuelling)
			{
				if (item != null)
				{
					if (item.isFuelled)
					{
						temp = Array.IndexOf(fuelling, item); //pump1 is index 0, pump2 is 1, etc
						fuelling[temp] = null;
						if (temp <= 2) //lane 1
						{
							lane1[temp].InUse = false;
							unoccupiedCounter[temp].Start(); //starts the stopwatch back up once it is unoccupied
						}
						else if (temp <= 5) //lane 2
						{
							lane2[temp % 3].InUse = false;
							unoccupiedCounter[temp].Start();
						}
						else //lane 3
						{
							lane3[temp % 3].InUse = false;
							unoccupiedCounter[temp].Start();
						}
					}
				}
			}
		}
		public static void SortQueue()
		{
			bool sorted = true;
            int nullCounter = 0;
            foreach (Vehicle item in existingVehicles)
            {
                if (item == null)
                {
                    nullCounter++;
                }
            }
			if (nullCounter != existingVehicles.Length) //it is alreaady ordered if there are no vehicles in here
			{
                Vehicle[] temp = new Vehicle[existingVehicles.Length]; //a temp array that will store the vehicle array without null items
				for (int i = 0; i < temp.Length; i++)
				{
					temp[i] = null;
				}
				int counter = 0;
				for (int i = 0; i < existingVehicles.Length; i++)
				{
					if (existingVehicles[i] != null)
					{
						temp[counter] = existingVehicles[i];
						counter++;
					}
				}
				for (int i = 0; i < existingVehicles.Length; i++)
				{
					existingVehicles[i] = temp[i];
				}
			}
		}
	}
}
