using System.Text;

namespace Broken_Petrol_Ltd_2
{
	partial class Program
	{
		public static void EndOfTheDay()
		{
			Console.Clear();
			try
			{
				currentUser.totalCost = Math.Round(AllUnleaded() * UNLEADED_COST + AllDiesel() * DIESEL_COST + AllLpg() * LPG_COST, 2);
				currentUser.comission = Math.Round(currentUser.totalCost / 100, 2);
				Console.WriteLine($"The Day has ended {currentUser.Username}, please note the following:");
				Console.WriteLine($"The number of vehicles that were fuelled today is: {carsFuelled}");
				Console.WriteLine($"The number of vehicles that left before fuelling today is: {carsLeft}");
				Console.WriteLine($"The amount of Unleaded fuel that was dispensed today is: {AllUnleaded()} litres");
				Console.WriteLine($"The amount of Diesel fuel that was dispensed today is: {AllDiesel()} litres");
				Console.WriteLine($"The amount of LPG fuel that was dispensed today is: {AllLpg()} litres");
				Console.WriteLine($"The total cost of fuel is £{currentUser.CostInString()} and your 1% is £{currentUser.CommissionInString()}");
				Console.WriteLine($"Your wages based on your work today is £{currentUser.Wages()}");
				Console.WriteLine($"The average pump efficiency is {AveragePercentage()}%");
				Console.WriteLine("Please choose one of the below options by typing its respective number:");
				Console.WriteLine("[1] View previous day/s results");
				Console.WriteLine("[2] Delete previous day/s results");
				Console.WriteLine("[3] Calculate tomorrow's fuel average");
				Console.WriteLine("[4] Logout");
				Console.WriteLine("Please now select your desired option");
				int option = Convert.ToInt32(Console.ReadLine());
				switch (option)
				{
					case 1:
						PreviousDay(0);
						break;

					case 2:
						PreviousDay(1);
						break;

					case 3:
						PreviousDay(2);
						break;

					case 4:
						Console.WriteLine("Logging out...");
						Thread.Sleep(500);
						Environment.Exit(0);
						break;

					default:
						Console.WriteLine("Something has gone wrong, please enter a valid option.");
						Thread.Sleep(500);
						EndOfTheDay();
						break;

				}

			}
			catch
			{
				Console.WriteLine("Something has gone wrong, please enter a valid option.");
				Thread.Sleep(500);
				EndOfTheDay();
			}
		}
		public static void WriteToFile()
		{
			DateTime temp = DateTime.Today;
			String today = temp.ToString().Substring(0, temp.ToString().Length - 9) + "," + currentUser.Username + "," + AllUnleaded() + "," + AllDiesel() + "," + AllLpg() + "," + currentUser.CostInString() + "," + currentUser.Wages() + "," + carsFuelled + "," + carsLeft + "," + AveragePercentage() + "," + Environment.NewLine;
            Byte[] todayInBytes = new UTF8Encoding(true).GetBytes(today); //what needs to be written to a file as a byte data type
			if (!File.Exists(PATH + @"\BrokenPetrolLtd.txt")) //PATH is where the directory of this program, the text file should be in this directory from previous days of use, if not, its created.
			{
				using (FileStream records = File.Create(PATH + @"\BrokenPetrolLtd.txt"))
				{
                    File.SetAttributes(PATH + @"\BrokenPetrolLtd.txt", FileAttributes.Normal); //allow it to be read, not hidden and can be written in
                    records.Write(todayInBytes, 0, todayInBytes.Length);
				}


			}
			else
			{
                File.SetAttributes(PATH + @"\BrokenPetrolLtd.txt", FileAttributes.Normal);
                File.AppendAllText(PATH + @"\BrokenPetrolLtd.txt", today);
			}
            File.SetAttributes(PATH + @"\BrokenPetrolLtd.txt", FileAttributes.Hidden | FileAttributes.ReadOnly); //hides the file to prevent social engineering via employees
        }
		public static void LogChecker()
		{
			if (!Directory.Exists(PATH + @"\Logs"))
			{
				Directory.CreateDirectory(PATH + @"\Logs");
			}
		}
		public static void LogDisplay() //displays all transactions for the day
		{
			Console.Clear();
			String[] lines = File.ReadAllLines(PATH + @"\Logs\" + logDay + ".txt");
			foreach (String line in lines) 
			{
				Console.WriteLine(line);
			}
			Console.WriteLine("\nThis will be saved in the Logs folder, press any key to continue.");
			Console.ReadKey();
		}
		public static void PreviousDay(int option)
		{
			String[] lines = null;
			String[] temp2; //will hold array with split values
			int i = 0;

			lines = File.ReadAllLines(PATH + @"\BrokenPetrolLtd.txt");
			Console.Clear();
			foreach (String line in lines)
			{
				try
				{
					temp2 = line.Split(",");
					Console.WriteLine($"[{i + 1}] Date: {temp2[0]} | User: {temp2[1]} | Unleaded Sold: {temp2[2]} | Diesel Sold: {temp2[3]} " +
						$"\n     LPG Sold: {temp2[4]} | Cost of Fuel: £{temp2[5]} | Wages Earned: £{temp2[6]} | Cars Fuelled: {temp2[7]}" +
						$"\n     Cars Left: {temp2[8]} | Average Pump Efficiency: {temp2[9]}%\n");
					i++;
				}
                catch //if a line does not conform to this standard it will be ignored and deleted
                {
					PreviousDay(option);
					
				}

            }
            switch (option)
			{
				case 0:
					Console.WriteLine("Please enter any key to return.");
					Console.ReadKey();
					EndOfTheDay();
					break;
				case 1:
					Console.WriteLine("Please type in the index that you wish to delete, please note that today's results cannot be deleted"); //prevents complete loss of statistics if console is breached
					try
					{
						int toDelete = Convert.ToInt32(Console.ReadLine()) - 1;
						if (lines[toDelete] != lines.Last()) //prevents newest piece of data from being deleted for security reasons
						{
							lines[toDelete] = null;
						}
						else
						{
							Console.WriteLine("The selected item is today's work which cannot be deleted.");
							Thread.Sleep(2000);
							EndOfTheDay();
						}
						File.SetAttributes(PATH + @"\\BrokenPetrolLtd.txt", FileAttributes.Normal); //allows file to be written into
						File.WriteAllText(PATH + @"\BrokenPetrolLtd.txt", string.Empty); //clears the text file
						foreach (String line in lines)
						{
							if (line == null)
							{
								continue;
							}
							File.AppendAllText(PATH + @"\BrokenPetrolLtd.txt", line + Environment.NewLine); //rewrites the file line by line

						}
						File.SetAttributes(PATH + @"\BrokenPetrolLtd.txt", FileAttributes.ReadOnly | FileAttributes.Hidden);

						Console.WriteLine("Press any key to go back.");
						Console.ReadKey();
					}
					catch(Exception ex)
					{
						Console.WriteLine("Something has gone wrong.");
						Console.WriteLine(ex.Message);
						Thread.Sleep(500);
						EndOfTheDay();
					}
					EndOfTheDay();
					break;
				case 2:
					Console.Clear();
					double unleadedMean = 0;
					double[] unleadedSdArray = new double[lines.Length];//works out the standard deviation of all fuels
					double dieselMean = 0;
					double[] dieselSdArray = new double[lines.Length];
					double lpgMean = 0;
					double[] lpgSdArray = new double[lines.Length];
					int numTotal = 0;

					foreach (String line in lines)
					{
						temp2 = line.Split(",");
						unleadedMean += Convert.ToDouble(temp2[2]);
						unleadedSdArray[Array.IndexOf(lines, line)] = Math.Pow(Convert.ToDouble(temp2[2]), 2);
						dieselMean += Convert.ToDouble(temp2[3]);
						dieselSdArray[Array.IndexOf(lines, line)] = Math.Pow(Convert.ToDouble(temp2[3]), 2);
						lpgMean += Convert.ToDouble(temp2[4]);
						lpgSdArray[Array.IndexOf(lines, line)] = Math.Pow(Convert.ToDouble(temp2[4]), 2);
						numTotal++;

					}
					unleadedMean /= numTotal;
					dieselMean /= numTotal;
					lpgMean /= numTotal;
					double unleadedSd = Math.Sqrt((unleadedSdArray.Sum() / numTotal) - (Math.Pow(unleadedMean, 2))); //works out standard deviation of each
					double dieselSd = Math.Sqrt((dieselSdArray.Sum() / numTotal) - (Math.Pow(dieselMean, 2)));
					double lpgSd = Math.Sqrt((lpgSdArray.Sum() / numTotal) - (Math.Pow(lpgMean, 2)));
					double tmp1 = Math.Round(unleadedMean - (3 * unleadedSd), 2);
					double tmp2 = Math.Round(dieselMean - (3 * dieselSd), 2);
					double tmp3 = Math.Round(lpgMean - (3 * lpgSd), 2);

					if (unleadedMean - (3 * unleadedSd) < 0) //checks if standard deviation is below 0 and if it is to replace the lower boundary with 0 instead.
					{
						tmp1 = 0;
					}
					if (dieselMean - (3 * dieselSd) < 0)
					{
						tmp2 = 0;
					}
					if (lpgMean - (3 * lpgSd) < 0)
					{
						tmp3 = 0;
					}
					Console.WriteLine($"Based on data from previous days, the expected unleaded fuel tomorrow will be: {tmp1} - {Math.Round(unleadedMean + (3 * unleadedSd), 2)} litres;");
					Console.WriteLine($"Based on data from previous days, the expected diesel fuel tomorrow will be: {tmp2} - {Math.Round(dieselMean + (3 * dieselSd), 2)} litres;");
					Console.WriteLine($"Based on data from previous days, the expected LPG fuel tomorrow will be: {tmp3} - {Math.Round(lpgMean + (3 * lpgSd), 2)} litres.");
					Console.WriteLine("Press any key to go back.");
					Console.ReadKey();
					EndOfTheDay();

					break;
			}
		}
	}
}