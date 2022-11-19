namespace Broken_Petrol_Ltd_2
{
	partial class Program
	{
		public static void Login(int step, int tries)
		{
			String[] username = { "Admin", "Admin1", "Admin2" }; //usernames that can be used to login
			String[] password = { "Admin12", "Admin123", "Admin1234" }; //respective passwords that must be used appropriately with the correct username to login
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
				int temp1 = Array.IndexOf(username, currentUser.Username); //checks to see if entered details matches that within the array
				int temp2 = Array.IndexOf(password, currentUser.Password);

				if (temp1 == -1 || temp2 == -1 || temp1 != temp2)
				{
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

				}
			}
		}
	}
}