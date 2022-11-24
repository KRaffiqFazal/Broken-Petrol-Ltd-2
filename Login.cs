using System.Text;

namespace Broken_Petrol_Ltd_2
{
	partial class Program
	{
		public static String[] managerLogins = { "Test", "Test123" };
		public static void Login(int step, int tries)
		{
			UpdateLogins();
			Console.Clear();
            File.SetAttributes(PATH + @"\PricesUsers.txt", FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.ReadOnly);
            Console.WriteLine("|-------BROKEN PETROL LTD TERMINAL-------|\n");
			Console.WriteLine($"ATTEMPTS LEFT: {tries}\n");
			Console.WriteLine($"USERNAME:{currentUser.Username}\n");
			Console.WriteLine($"PASSWORD:{currentUser.HidePassword()}\n");
			Console.WriteLine("|----------------------------------------|");
			if (step == 0)
			{
				Console.WriteLine("Please enter your username");
				currentUser.Username = Console.ReadLine();
				Console.Clear();
				Login(1, tries);
			}
			else if (step == 1)
			{
				Console.WriteLine("Please enter your password");
				currentUser.Password = Console.ReadLine();
				Console.Clear();
				Login(2, tries);
			}
			else if (step == 2)
			{
				Console.WriteLine("Checking details...");
				int temp1 = username.IndexOf(currentUser.Username); //checks to see if entered details matches that within the array
				int temp2 = password.IndexOf(currentUser.Password);

				if (currentUser.Username.Equals(managerLogins[0]) && currentUser.Password.Equals(managerLogins[1]))
				{
					currentUser.ClearFields();
                    ManagerLogin(false);
				}
                if (temp1 == -1 || temp2 == -1 || temp1 != temp2)
				{
					try
					{
						if (username[temp1] == password[temp1])
						{
                            Console.WriteLine("The details you entered were correct");
                            Thread.Sleep(800);
                            Console.Clear();
                            Console.Write("Logging in");
                            Thread.Sleep(300);
                            Console.Write(".");
                            Thread.Sleep(300);
                            Console.Write(".");
                            Thread.Sleep(300);
                            Console.Write(".");
                            cont = true;
                            Main(null);
                        }
					}
					catch { } //makes sure that another user that shares the password does not affect the success of your login attempt
					tries--;
					if (tries <= 0)
					{
						Console.WriteLine("INTRUSION DETECTED. TERMINAL DISABLED.");
						Environment.Exit(0);
					}
					else
					{
						Console.WriteLine($"The entered details do not match, try again ({tries} attempt(s) left).");
						currentUser.ClearFields();
					}
					Thread.Sleep(2000);
					Console.Clear();
					Login(0, tries);
				}
				else if (temp1 == temp2)
				{
					Console.WriteLine("The details you entered were correct");
					Thread.Sleep(800);
					Console.Clear();
					Console.Write("Logging in");
					Thread.Sleep(300);
					Console.Write(".");
					Thread.Sleep(300);
					Console.Write(".");
					Thread.Sleep(300);
					Console.Write(".");
					cont = true;
					Main(null);

				}
			}
		}
		public static void UpdatePrices()//will run once on runtime and then however many times prices are updated
		{
			DataFileExists();
            String[] lines;
            lines = File.ReadAllLines(PATH + @"\PricesUsers.txt");
            String petrol = lines[0]; //contains just the petrol prices
            lines = null; //clears the array as we only want the prices
            lines = petrol.Split(','); //splits line into prices in array
            double[] prices = new double[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                prices[i] = Convert.ToDouble(lines[i]);
            }
            UNLEADED_COST = prices[0];
            DIESEL_COST = prices[1];
			LPG_COST = prices[2];
        }
		public static void UpdateLogins()//runs everytime Login is called
		{
			DataFileExists();
            String[] lines;
			String[] logins = new string[2];
            lines = File.ReadAllLines(PATH + @"\PricesUsers.txt");
			username.Clear();
			password.Clear(); //reset values to null to prevent duplicates or mismatched details
			
			for (int i=1; i<lines.Length; i++) 
			{
				logins = lines[i].Split(",");
				username.Add(logins[0]);
				password.Add(logins[1]);
			}
        }
		public static void DataFileExists() //checks if the file exists which contains the information for the logins and petrol prices
		{
            if (!File.Exists(PATH + @"\PricesUsers.txt")) //PATH is where the directory of this program, the text file should be in this directory and remain here, however if maliciously deleted, the program does not stop running
            {
                using (FileStream records = File.Create(PATH + @"\PricesUsers.txt"))
                {
                    File.SetAttributes(PATH + @"\PricesUsers.txt", FileAttributes.Normal); //allow it to be read, not hidden and can be written in

                    Byte[] reload = new UTF8Encoding(true).GetBytes("1.6225,1.8656,0.8635" + Environment.NewLine + "Admin,Admin12" + Environment.NewLine + "Admin1,Admin123" + Environment.NewLine + "Admin2,Admin1234"); //unleaded, diesel, lpg prices per litre in GBP to be set as these as default, the next lines are basic logins that can be viewed/changed via other options
                    records.Write(reload, 0, reload.Length);
                }
            }
        }
		public static void ManagerLogin(bool login)
		{
			Console.Clear(); //need to create a file, work out where to put it and find a way to make login read its contents into that file, then might be done
			if (!login)
			{
				DateTime now = DateTime.Now;
				if (!File.Exists(PATH + @"\Log.txt")) //PATH is where the directory of this program, the text file should be in this directory and remain here, however if maliciously deleted, the program does not stop running
				{
					using (FileStream logger = File.Create(PATH + @"\Log.txt"))
					{
						File.SetAttributes(PATH + @"\Log.txt", FileAttributes.Normal); //allow it to be read, not hidden and can be written in
						Byte[] logged = new UTF8Encoding(true).GetBytes(now.ToString() + Environment.NewLine);
						logger.Write(logged, 0, logged.Length);
					}
				}
				else
				{
					File.SetAttributes(PATH + @"\Log.txt", FileAttributes.Normal);
					File.AppendAllText(PATH + @"\Log.txt", now.ToString() + Environment.NewLine);
				} //this is used to log that the manager had logged in at this point in time
			}
            if (!File.Exists(PATH + @"\PricesUsers.txt")) //PATH is where the directory of this program, the text file should be in this directory and remain here, however if maliciously deleted, the program does not stop running
            {
                using (FileStream records = File.Create(PATH + @"\PricesUsers.txt"))
                {
                    File.SetAttributes(PATH + @"\PricesUsers.txt", FileAttributes.Normal); //allow it to be read, not hidden and can be written in

                    Byte[] reload = new UTF8Encoding(true).GetBytes("1.6225,1.8656,0.8635" + Environment.NewLine + "Admin,Admin12" + Environment.NewLine + "Admin1,Admin123" + Environment.NewLine + "Admin2,Admin1234"); //unleaded, diesel, lpg prices per litre in GBP to be set as these as default, the next lines are basic logins that can be viewed/changed via other options
                    records.Write(reload, 0, reload.Length);
                }
            }
			Console.Clear();
            Console.WriteLine("---WELCOME TO THE MANAGER LOGIN SCREEN---");
			Console.WriteLine("This login has been recorded for security purposes"); //if an employee has obtained the manager's information their login will be dated in a text file and logged
			Console.WriteLine("Please enter the option that you wish to choose");
			Console.WriteLine("[1] Edit Fuel Prices");
			Console.WriteLine("[2] Edit Employee login details");
			Console.WriteLine("[3] View manager login log");
			Console.WriteLine("[4] Log out");
			Console.WriteLine("Please enter an option:");
			String option = Console.ReadLine();
			switch(option) 
			{
				case "1":
					FuelPriceChange();
                    break;
				
				case "2":
					EmployeeLoginChange();
					break;

				case "3":
					ManagerLog();
					break;

				case "4":
					Environment.Exit(0);
					break;

				default:
					Console.WriteLine("The option is unrecognised, please try again");
					Thread.Sleep(500);
					break;
			}
			ManagerLogin(true);
		}

	}
}