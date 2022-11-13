using System;
using System.Threading;
using System.IO; //used for reading files
using System.Text;

namespace Broken_Petrol_Ltd_2
{
    class Program
    {
        public static String PATH = @"C:\Users\raffi\source\repos\Broken Petrol Ltd 2"; //message to end-user: please replace this with the path of the program on the petrol station console
        public static double UNLEADED_COST = 1.6638; //number of pounds per litre
        public static double DIESEL_COST = 1.9051;
        public static double LPG_COST = 0.8939;

        public static bool cont = false; //will signal functions to stop when it becomes false, will become true if login successful
        public static int carsFuelled = 0; //counts the number of cars that have fuelled and then have left
        public static int carsLeft = 0; //counts the number of cars that have left before fuelling
        public static String[] username = { "Admin", "Admin1", "Admin2" }; //usernames that can be used to login
        public static String[] password = { "Admin12", "Admin123", "Admin1234" }; //respective passwords that must be used appropriately with the correct username to login
        public static String currentUsername = "............"; //a placeholder username for when the user has not entered a username yet to login
        public static String currentPasswordInStars = "************"; //to prevent social engineering (shoulder surfing)
        public static String currentPassword;
        public static int selectedUsername;
        
        public static Pump[] lane1 = { new Pump(1), new Pump(2), new Pump(3) };
        public static Pump[] lane2 = { new Pump(4), new Pump(5), new Pump(6) };
        public static Pump[] lane3 = { new Pump(7), new Pump(8), new Pump(9) };
        public static Pump[][] allLanes = { lane1, lane2, lane3 }; //a jagged array containing all the lanes that each contain the pumps with numbers.
        static void Main(string[] args)
        {

            Random rnd = new Random();
            Vehicle[] existingVehicles = new Vehicle[5]; //a queue that cars are sent to, to wait for their turns
            Vehicle[] fuelling = new Vehicle[9]; //a queue that cars are sent to from existingVehicles whilst they fuel
            CarTimer carAdder = new Timer(CarAdder, existingVehicles, 0, rnd.Next(1500, 2200));
            bool temp = false;
            Login(0, 3);
            while (cont) //cont will become false once the "day" is over, this stops vehicles from being assigned/kicked/leaving.
            {
                if (cont && !temp) //run one time once correct username/password used
                {
                    Timer ender = new Timer(Stopper, null, 30000, 0);
                    temp = true;
                }
                Assigner(existingVehicles, fuelling);
                WaitKick(existingVehicles);
                FuelledKick(existingVehicles);
                NotFuellingAnymore(fuelling);
            }
            carAdder.Dispose(); //disposes the timer that creates vehicles now that the day has finished
            cont = true;
            while (cont) //will run through to simulate the last vehicles that are waiting/fuelling to leave without new vehicles from being created
            {
                cont = false;
                Assigner(existingVehicles, fuelling);
                WaitKick(existingVehicles);
                FuelledKick(existingVehicles);
                NotFuellingAnymore(fuelling);
                foreach (Vehicle item in existingVehicles)
                {
                    if (item != null)
                    {
                        cont = true;
                    }
                }
                foreach (Vehicle item in fuelling)
                {
                    if (item != null)
                    {
                        cont = false;
                    }
                }
            }
            WriteToFile(AllUnleaded(), AllDiesel(), AllLpg());
            EndOfTheDay();

        }

        public static void Login(int step, int tries)
        {
            Console.WriteLine("|-------BROKEN PETROL LTD TERMINAL-------|\n");
            Console.WriteLine($"ATTEMPTS LEFT: {tries}\n");
            Console.WriteLine($"USERNAME:{currentUsername}\n");
            Console.WriteLine($"PASSWORD:{currentPasswordInStars}\n");
            Console.WriteLine("|----------------------------------------|");
            if (step == 0)
            {
                Console.WriteLine("Please enter your username");
                currentUsername = Console.ReadLine();
                Console.Clear();
                Login(1, tries);
            }
            else if (step == 1)
            {
                Console.WriteLine("Please enter your password");
                currentPassword = Console.ReadLine();
                int passwordSize = currentPassword.Length;
                currentPasswordInStars = "";
                for (int i = 0; i < passwordSize; i++)
                {
                    currentPasswordInStars += "*";
                }
                Console.Clear();
                Login(2, tries);
            }
            else if (step == 2)
            {
                Console.WriteLine("Checking details...");
                int temp1 = Array.IndexOf(username, currentUsername); //checks to see if entered details matches that within the array
                int temp2 = Array.IndexOf(password, currentPassword);

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
                        Console.WriteLine($"The entered details do not match, try again ({tries} attempts left).");
                    }
                    currentUsername = "............";
                    currentPasswordInStars = "************";
                    Thread.Sleep(2000);
                    Console.Clear();
                    Login(0, tries);
                }
                else if (temp1 == temp2)
                {
                    Console.WriteLine("The details you entered were correct");
                    cont = true;
                    selectedUsername = temp1; //making the username a value that can be displayed in the petrol UI
                    return;
                }
            }
        }
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
        public static void EndOfTheDay()
        {
            Console.Clear();
            try
            {
                double totalCost = Math.Round(AllUnleaded() * UNLEADED_COST + AllDiesel() * DIESEL_COST + AllLpg() * LPG_COST, 2);
                Console.WriteLine($"The Day has ended {username[selectedUsername]}, please note the following:");
                Console.WriteLine($"The number of cars that were fuelled today is: {carsFuelled}");
                Console.WriteLine($"The number of cars that left before fuelling today is: {carsLeft}");
                Console.WriteLine($"The amount of Unleaded fuel that was dispensed today is: {AllUnleaded()}litres");
                Console.WriteLine($"The amount of Diesel fuel that was dispensed today is: {AllDiesel()}litres");
                Console.WriteLine($"The amount of LPG fuel that was dispensed today is: {AllLpg()}litres");
                Console.WriteLine($"The total cost of fuel is £{totalCost} and your 1% is £{Math.Round(totalCost / 100, 2)}");
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
                        PreviousDay(0, AllUnleaded(), AllDiesel(), AllLpg()); 
                        break;

                    case 2: 
                        PreviousDay(1, AllUnleaded(), AllDiesel(), AllLpg()); 
                        break;

                    case 3: 
                        PreviousDay(2, AllUnleaded(), AllDiesel(), AllLpg()); 
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
        public static void WriteToFile(double unleaded, double diesel, double lpg)
        {
            DateTime temp = DateTime.Today;
            Byte[] today = new UTF8Encoding(true).GetBytes(temp + "," + unleaded + "," + diesel + "," + lpg + Environment.NewLine);
            if (!File.Exists(PATH + @"\BrokenPetrolLtd.txt")) //PATH is where the directory of this program, the text file should be in this directory from previous days of use, if not, its created.
            {
                using (FileStream records = File.Create(PATH + @"\BrokenPetrolLtd.txt"))
                {
                    records.Write(today, 0, today.Length);
                }

            }
            else
            {
                File.AppendAllText(PATH + @"\BrokenPetrolLtd.txt",username[selectedUsername] + "," + Math.Round((unleaded * UNLEADED_COST + diesel * DIESEL_COST + lpg * LPG_COST) / 100, 2) + "," + carsFuelled + "," + carsLeft + "," + temp + "," + unleaded + "," + diesel + "," + lpg + Environment.NewLine);
            }
        }
        public static void PreviousDay(int option, double unleaded, double diesel, double lpg) //Make date the first value and rejig everything based on that
        {
            String[] lines = null;
            String[] temp2; //will hold array with split values
            int i = 0;

            lines = File.ReadAllLines(PATH + @"\BrokenPetrolLtd.txt");
            foreach (String line in lines)
            {
                temp2 = line.Split(",");
                Console.WriteLine($"[{i + 1}] Employee: {temp2[0]} | Employee's 1%: £{temp2[1]} | Vehicles Fuelled: {temp2[2]} | Vehicles Left: {temp2[3]} | Date: {temp2[4]} | Unleaded: {temp2[5]} | Diesel: {temp2[6]} | LPG: {temp2[7]}");
                i++;
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
                        File.WriteAllText(PATH + @"\BrokenPetrolLtd.txt", string.Empty); //clears the text file
                        foreach (String line in lines)
                        {
                            if (line == null)
                            {
                                continue;
                            }
                            File.AppendAllText(PATH + @"\BrokenPetrolLtd.txt", line + Environment.NewLine); //rewrites the file line by line

                        }
                        Console.WriteLine("Press any key to go back.");
                        Console.ReadKey();
                    }
                    catch
                    {
                        Console.WriteLine("Something has gone wrong.");
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
                        unleadedMean += Convert.ToDouble(temp2[5]);
                        unleadedSdArray[Array.IndexOf(lines, line)] = Math.Pow(Convert.ToDouble(temp2[5]), 2);
                        dieselMean += Convert.ToDouble(temp2[6]);
                        dieselSdArray[Array.IndexOf(lines, line)] = Math.Pow(Convert.ToDouble(temp2[6]), 2);
                        lpgMean += Convert.ToDouble(temp2[7]);
                        lpgSdArray[Array.IndexOf(lines, line)] = Math.Pow(Convert.ToDouble(temp2[7]), 2);
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
        public static void Assigner(Vehicle[] existingVehicles, Vehicle[] fuelling) //a method that will run throughout the entire program to ensure that vehicles are assigned as they come in
        {
            //assign vehicles to an available pump
            for (int i=0; i < existingVehicles.Length; i++)
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
                            lane[0].InUse = true;
                            existingVehicles[i] = null;
                            AddFuel(0, lane, fuelling);
                            Displayer(fuelling[lane[0].PumpNumber - 1], lane[0].PumpNumber);
                            carsFuelled++;
                        }
                        else if (lane[2].InUse && !lane[1].InUse && existingVehicles[i] != null) //this needs to be fixed
                        {
                            fuelling[lane[1].PumpNumber - 1] = existingVehicles[i];
                            fuelling[lane[1].PumpNumber - 1].StartingFuelling();
                            lane[1].InUse = true;
                            existingVehicles[i] = null;
                            AddFuel(1, lane, fuelling);
                            Displayer(fuelling[lane[1].PumpNumber - 1], lane[1].PumpNumber);
                            carsFuelled++;
                        }
                        else if (!lane[2].InUse && !lane[1].InUse && !lane[0].InUse && existingVehicles[i] != null) //if all pumps are free, furthest will be selected to fuel at.
                        {
                            fuelling[lane[2].PumpNumber - 1] = existingVehicles[i];
                            fuelling[lane[2].PumpNumber - 1].StartingFuelling();
                            lane[2].InUse = true;
                            existingVehicles[i] = null;
                            AddFuel(2, lane, fuelling);
                            Displayer(fuelling[lane[2].PumpNumber - 1], lane[2].PumpNumber);
                            carsFuelled++;
                        }
                        
                    }
                }
            }
        }
        public static void AddFuel(int laneNum, Pump[] lane, Vehicle[] fuelling)
        {
            if (fuelling[lane[laneNum].PumpNumber - 1].fuelType.Equals("Unleaded"))
            {
                lane[laneNum].UnleadedDispensed += (fuelling[lane[laneNum].PumpNumber - 1].fuellingTimeInt / 1000) * 1.5; //based on how long the vehicle was meant to fuel for, divided by 1000 to get the time in seconds and multiplied by 1.5 to work out how much fuel was dispensed
            }
            else if (fuelling[lane[laneNum].PumpNumber - 1].fuelType.Equals("Diesel"))
            {
                lane[laneNum].DieselDispensed += (fuelling[lane[laneNum].PumpNumber - 1].fuellingTimeInt / 1000) * 1.5;
            }
            else
            {
                lane[laneNum].LpgDispensed += (fuelling[lane[laneNum].PumpNumber - 1].fuellingTimeInt / 1000) * 1.5;
            }
        }
        public static void WaitKick(Vehicle[] existingVehicles) //an method that will run throughout the entire program to ensure that vehicles are kicked from the list if they finish waiting
        {

            //checks list of vehicles to see which want to leave
            for(int i=0; i< existingVehicles.Length; i++)
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
        public static void FuelledKick(Vehicle[] existingVehicles) //kicks vehicles from waiting list once they start fuelling as they will be at pump
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
        public static void NotFuellingAnymore(Vehicle[] fuelling) //checks to see if a vehicle's isFuelled boolean is true,
        {
            int temp;
            foreach (Vehicle item in fuelling)
            {
                if (item != null)
                {
                    if (item.isFuelled)
                    {
                        temp = Array.IndexOf(fuelling, item); //pump1 is index 0, 2 is 1, etc
                        fuelling[temp] = null;
                        if (temp <= 2) //lane 1
                        {
                            lane1[temp].InUse = false;
                        }
                        else if (temp <= 5) //lane 2
                        {
                            lane2[temp % 3].InUse = false;
                        }
                        else //lane 3
                        {
                            lane3[temp % 3].InUse = false;
                        }
                    }
                }
            }
        }
        public static void Stopper(object o) //will stop the program from running when required by making cont false.
        {
            cont = false;
            
        }
        public static void CarAdder(object o)
        {
            
            for (int i = 0; i < existingVehicles.Length; i++)
            {
                if (existingVehicles[i] == null)
                {
                    existingVehicles[i] = new Vehicle();
                    break;
                }
            }
        }
        public static void Displayer(Vehicle recentVehicle, int recentPumpNum)
        {
            Console.Clear();
            String[] available = { "Available", "Busy" };
            Console.WriteLine($"This terminal is in use by: {username[selectedUsername]}");
            Console.WriteLine($"Pump {lane1[2].PumpNumber}: {available[Convert.ToInt32(lane1[2].InUse)]} |Pump {lane2[2].PumpNumber}: {available[Convert.ToInt32(lane2[2].InUse)]} |Pump {lane3[2].PumpNumber}: {available[Convert.ToInt32(lane3[2].InUse)]}");
            Console.WriteLine($"Pump {lane1[1].PumpNumber}: {available[Convert.ToInt32(lane1[1].InUse)]} |Pump {lane2[1].PumpNumber}: {available[Convert.ToInt32(lane2[1].InUse)]} |Pump {lane3[1].PumpNumber}: {available[Convert.ToInt32(lane3[1].InUse)]}");
            Console.WriteLine($"Pump {lane1[0].PumpNumber}: {available[Convert.ToInt32(lane1[0].InUse)]} |Pump {lane2[0].PumpNumber}: {available[Convert.ToInt32(lane2[0].InUse)]} |Pump {lane3[0].PumpNumber}: {available[Convert.ToInt32(lane3[0].InUse)]}\n");
            Console.WriteLine("Vehicles Waiting\n");
            for (int i = 0; i < existingVehicles.Length; i++)
            {
                if (existingVehicles[i] != null)
                {
                    Console.WriteLine(existingVehicles[i].type);
                }
            }
            double unleaded = AllUnleaded();
            double diesel = AllDiesel();
            double lpg = AllLpg();
            double totalCost = Math.Round(unleaded * UNLEADED_COST + diesel * DIESEL_COST + lpg * LPG_COST, 2);
            Console.WriteLine($"Unleaded dispensed: {unleaded} litres");
            Console.WriteLine($"Diesel dispensed: {diesel} litres");
            Console.WriteLine($"LPG dispensed: {lpg}litres");
            Console.WriteLine($"Number of Vehicles that have fuelled: {carsFuelled} | Number of Vehicles that have left: {carsLeft}");
            Console.WriteLine($"Total Fuel Price: £{totalCost}");
            Console.WriteLine($"1% Commission £{Math.Round(totalCost / 100, 2)}");
            if (recentVehicle != null)
            {
                Console.WriteLine($"\nA new {recentVehicle.type} has started fuelling at Pump {recentPumpNum}");
            }
            
        }
    }

}