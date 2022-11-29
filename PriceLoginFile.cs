using System.Text;

namespace Broken_Petrol_Ltd_2
{
	partial class Program
	{
		public static void FuelPriceChange()
		{
			Console.Clear();
			Console.WriteLine("These are the current fuel prices in pence per litre:");
			Console.WriteLine($"[1] Unleaded: {Math.Round(UNLEADED_COST * 100, 2)}");
			Console.WriteLine($"[2] Diesel: {Math.Round(DIESEL_COST * 100, 2)}");
			Console.WriteLine($"[3] LPG: {Math.Round(LPG_COST * 100, 2)}");
			Console.WriteLine("[4] Go Back");
			Console.WriteLine("Please enter the option for the fuel type that you wish to change:");
			String option = Console.ReadLine();
			int index = 0; //will change based on which option the user picks
			String[] fuels = { "unleaded", "diesel", "LPG" };
			double temp = 0;
			switch (option)
			{
				case "1":
					index = 0;
					
					break;
				case "2":
					index = 1;
					break;
				case "3":
					index = 2;
					break;
				case "4":
					ManagerLogin(true);
					break;
				default:
					Console.WriteLine("Please enter a valid option");
					FuelPriceChange();
					break;
			}
			Console.WriteLine($"Please enter the new price of {fuels[index]} in pence per litre");
			try
			{
				temp = Convert.ToDouble(Console.ReadLine()); //will use this value to replace the desired temp in the first line
			}
			catch
			{
				Console.WriteLine("Please enter a valid value");
				Thread.Sleep(500);
				FuelPriceChange();
			}
			String[] temp2;
			String[] temp3;
			double[] newPrices = new double[3];
			temp2 = File.ReadAllLines(PATH + @"\PricesUsers.txt");
			temp2 = temp2[0].Split(",");
			temp2[index] = Convert.ToString(temp / 100);
			temp3 = File.ReadAllLines(PATH + @"\PricesUsers.txt");
			temp3[0] = temp2[0] + "," + temp2[1] + "," + temp2[2];
			File.SetAttributes(PATH + @"\PricesUsers.txt", FileAttributes.Normal);
			File.WriteAllLines(PATH + @"\PricesUsers.txt", temp3);
			File.SetAttributes(PATH + @"\PricesUsers.txt", FileAttributes.Hidden | FileAttributes.ReadOnly);
			UpdateLogins();
			UpdatePrices();
			FuelPriceChange();
		}

		public static void EmployeeLoginChange() //Please fix this
		{
			Console.Clear();
			Console.WriteLine("These are the employee logins currently:");

			String[] temp = File.ReadAllLines(PATH + @"\PricesUsers.txt"); //needs to get rid of price line
			String[] splitDetails = new String[2]; //splits username and password
			for (int i = 1; i < temp.Length; i++)
			{
				splitDetails = temp[i].Split(',');
				Console.WriteLine($"[{i}] Username: {splitDetails[0]} | Password: {splitDetails[1]}");
			}
			Console.WriteLine($"[{temp.Length}] Go back");
			Console.WriteLine("Please select a value to edit its details");
			try
			{
				int option = Convert.ToInt32(Console.ReadLine());
				if (option == temp.Length) //to go back
				{
					ManagerLogin(true);
				}
				else if (option < 1 || option > temp.Length) //not within the options allowed
				{
					Console.WriteLine("Please enter a suitable value");
					Thread.Sleep(500);
					EmployeeLoginChange();
				}
				Console.WriteLine("Please enter a new username");
				String newUsername = Console.ReadLine();
				Console.WriteLine("Please enter a new password");
				String newPassword = Console.ReadLine();
				if (newUsername.Equals(managerLogins[0]) || newPassword.Equals(managerLogins[1]) || (username.IndexOf(newUsername) != -1  && username.IndexOf(newUsername) != option - 1) || (password.IndexOf(newPassword) != -1 && password.IndexOf(newPassword) != option - 1) || newUsername.IndexOf(",") != -1 || newPassword.IndexOf(",") != -1)
				{
					Console.WriteLine("These details are not valid, please don't use ',' or match other values in the system");
					Thread.Sleep(900);
					EmployeeLoginChange();
				}
				String replaceDetails = newUsername + "," + newPassword;
				temp[option] = replaceDetails;
				File.SetAttributes(PATH + @"\PricesUsers.txt", FileAttributes.Normal);
                File.WriteAllLines(PATH + @"\PricesUsers.txt", temp);
				File.SetAttributes(PATH + @"\PricesUsers.txt", FileAttributes.Hidden | FileAttributes.ReadOnly);
				UpdateLogins();
                EmployeeLoginChange();
			}
			catch
			{
				Console.WriteLine("Please enter a suitable value");
				Thread.Sleep(500);
				EmployeeLoginChange();
			}
		}

		public static void ManagerLog()
		{
			Console.Clear();
			File.SetAttributes(PATH + @"\Log.txt", FileAttributes.Normal);
			String[] log = File.ReadAllLines(PATH + @"\Log.txt");
			File.SetAttributes(PATH + @"\Log.txt", FileAttributes.ReadOnly | FileAttributes.Hidden);
			Console.WriteLine("This is the manager login log");
			foreach (String line in log)
			{
				Console.WriteLine(line);
			}
			Console.WriteLine("Please press any key to go back");
			Console.ReadKey();
			ManagerLogin(true);
        }
	}
}