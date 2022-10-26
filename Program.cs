using System;
using System.Threading;
using System.IO; //used for reading files
namespace Broken_Petrol_Ltd_2
{
    class Program
    {
        public static int VehicleCounter = 0;
        public static bool cont = false; //will signal functions to stop when it becomes false, will become true if login successful
        public static int carsFuelled = 0;
        public static int carsLeft = 0;

        public static String[] username = { "Admin", "Admin1", "Admin2" }; //usernames that can be used to login
        public static String[] password = { "Admin12", "Admin123", "Admin1234" }; //respective passwords that must be used appropriately with the correct username to login
        public static String currentUsername = "............"; //a placeholder username for when the user has not entered a username yet to login
        public static String currentPasswordInStars = "************"; //to prevent a security hazard, a p
        public static String currentPassword;
        public static int selectedUsername;
        
        public static Vehicle[] existingVehicles = new Vehicle[5]; //a queue that cars are sent to, to wait for their turns
        public static Vehicle[] fuelling = new Vehicle[9]; //a queue that cars are sent to from existingVehicles whilst they fuel
        
        public static Pump[] lane1 = { new Pump(1), new Pump(2), new Pump(3) };
        public static Pump[] lane2 = { new Pump(4), new Pump(5), new Pump(6) };
        public static Pump[] lane3 = { new Pump(7), new Pump(8), new Pump(9) };
        public static Pump[][] allLanes = { lane1, lane2, lane3 }; //a jagged array containing all the lanes that each contain the pumps with numbers.
        static void Main(string[] args)
        {

            Random rnd = new Random();
            Timer carAdder = new Timer(CarAdder, null, 0, rnd.Next(1500, 2200));
            bool temp = false;
            Login(0, 3);
            while (cont)
            {
                if (cont && !temp) //run one time once correct username/password used
                {
                    Timer ender = new Timer(Stopper, null, 30000, 30000);
                    temp = true;
                }
                Assigner();
                WaitKick();
                FuelledKick();
                NotFuellingAnymore();
            }
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
        public static void EndOfTheDay()
        {
            Console.Clear();
            double unleaded = 0;
            double diesel = 0;
            double lpg = 0;
            foreach (Pump[] lane in allLanes)
            {
                foreach (Pump pump in lane)
                {
                    unleaded += pump.unleadedDispensed;
                    diesel += pump.dieselDispensed;
                    lpg += pump.lpgDispensed;
                }
            }
            try
            {
                Console.WriteLine($"The Day has ended {selectedUsername}, please note the following:");
                Console.WriteLine($"The number of cars that were fuelled today is: {carsFuelled}");
                Console.WriteLine($"The number of cars that left before fuelling today is: {carsLeft}");
                Console.WriteLine($"The amount of Unleaded fuel that was dispensed today is: {unleaded}litres");
                Console.WriteLine($"The amount of Diesel fuel that was dispensed today is: {diesel}litres");
                Console.WriteLine($"The amount of LPG fuel that was dispensed today is: {lpg}litres");
                Console.WriteLine("Please choose one of the below options by typing its respective number:");
                Console.WriteLine("[1] View previous day/s results");
                Console.WriteLine("[2] Delete previous day/s results");
                Console.WriteLine("[3] Calculate tomorrow's fuel average");
                Console.WriteLine("[4] Logout");
                Console.WriteLine("Please now select your desired option");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1: PreviousDay(0);

                    case 2: PreviousDay(1);

                    case 3: PreviousDay(2);

                    case 4: Console.WriteLine("Logging out..."); Thread.Sleep(500); Environment.Exit(0);
                }

            }
            catch
            {
                Console.WriteLine("Please enter a valid input");
                Thread.Sleep(500);
                EndOfTheDay();
            }
        }
        public static void PreviousDay(int option)
        { 
            if
        }
        public static void Assigner() //an asynchronous method that will run throughout the entire program to ensure that vehicles are assigned as they come in
        {
            //assign vehicles to an available pump
            for (int i=0; i< existingVehicles.Length; i++)
            {
                if (existingVehicles[i] != null)
                {
                    if (!existingVehicles[i].isFuelling && !existingVehicles[i].hasWaited)
                    {

                        foreach (Pump[] lane in allLanes)
                        {
                            if (lane[0].inUse) //if the first pump in the lane is not free, automatically check the next lane
                            {
                                Console.WriteLine("Closest pump is in use");
                                continue;
                            }
                            else if (lane[1].inUse && !lane[0].inUse) //if the second pump in the lane is being used but the first pump is not, go to first pump
                            {
                                lane[0].inUse = true;
                                fuelling[lane[0].pumpNumber - 1] = existingVehicles[i];
                                existingVehicles[i] = null;
                                fuelling[lane[0].pumpNumber - 1].StartingFuelling();
                                AddFuel(0, lane);
                                Displayer();
                                Console.WriteLine("Middle pump in use so going to closest");
                            }
                            else if (!lane[2].inUse && !lane[1].inUse && !lane[0].inUse) //if all pumps are free, furthest will be selected to fuel at.
                            {
                                lane[2].inUse = true;
                                fuelling[lane[2].pumpNumber - 1] = existingVehicles[i];
                                fuelling[lane[2].pumpNumber - 1].StartingFuelling();
                                existingVehicles[i] = null;
                                AddFuel(2, lane);
                                Displayer();
                                Console.WriteLine("No pumps in use going to farthest");
                            }
                        }
                    }
                }
            }
        }
        public static void AddFuel(int laneNum, Pump[] lane)
        {
            if (fuelling[lane[laneNum].pumpNumber - 1].fuelType.Equals("Unleaded"))
            {
                lane[laneNum].unleadedDispensed += (fuelling[lane[laneNum].pumpNumber - 1].fuellingTimeInt / 1000) * 1.5; //based on how long the vehicle was meant to fuel for, divided by 1000 to get the time in seconds and multiplied by 1.5 to work out how much fuel was dispensed
            }
            else if (fuelling[lane[laneNum].pumpNumber - 1].fuelType.Equals("Diesel"))
            {
                lane[laneNum].dieselDispensed += (fuelling[lane[laneNum].pumpNumber - 1].fuellingTimeInt / 1000) * 1.5;
            }
            else
            {
                lane[laneNum].lpgDispensed += (fuelling[lane[laneNum].pumpNumber - 1].fuellingTimeInt / 1000) * 1.5;
            }
        }
        public static void WaitKick() //an asynchronous method that will run throughout the entire program to ensure that vehicles are kicked from the list if they finish waiting
        {

            //checks list of vehicles to see which want to leave
            for(int i=0; i< existingVehicles.Length; i++)
            {
                if (existingVehicles[i] != null)
                {
                    if (existingVehicles[i].hasWaited)
                    {
                        existingVehicles[i] = null;
                        carsLeft++;
                        Displayer();
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
                        Displayer();
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
                        temp = Array.IndexOf(fuelling, item);
                        fuelling[temp] = null;
                        if (temp <= 3) //lane 1
                        {
                            lane1[temp - 1].inUse = false;
                        }
                        else if (temp <= 6) //lane 2
                        {
                            lane2[temp - 1].inUse = false;
                        }
                        else //lane 3
                        {
                            lane3[temp - 1].inUse = false;
                        }
                        carsFuelled++;
                    }
                }
            }
        }
        public static void Stopper(object o) //will stop the program from running when required by making cont false.
        {
            Console.WriteLine("the program should stop now hopefully");
            cont = false;
        }
        public static void CarAdder(object o)
        {
            for (int i = 0; i < existingVehicles.Length; i++)
            {
                if (existingVehicles[i] == null)
                {
                    existingVehicles[i] = new Vehicle(VehicleCounter);
                }
            }
        }
        public static void Displayer()
        {
            Console.Clear();
            Console.WriteLine($"This terminal is in use by: {username[selectedUsername]}");
            Console.WriteLine($"Pump {lane1[2].pumpNumber}: {lane1[2].inUse} |Pump {lane2[2].pumpNumber}: {lane2[2].inUse} |Pump {lane3[2].pumpNumber}: {lane3[2].inUse}");
            Console.WriteLine($"Pump {lane1[1].pumpNumber}: {lane1[1].inUse} |Pump {lane2[1].pumpNumber}: {lane2[1].inUse} |Pump {lane3[1].pumpNumber}: {lane3[1].inUse}");
            Console.WriteLine($"Pump {lane1[0].pumpNumber}: {lane1[0].inUse} |Pump {lane2[0].pumpNumber}: {lane2[0].inUse} |Pump {lane3[0].pumpNumber}: {lane3[0].inUse}");
            for (int i = 0; i < existingVehicles.Length; i++)
            {
                if (existingVehicles[i] != null)
                {
                    Console.WriteLine(existingVehicles[i].type);
                }
            }
            double unleaded = 0;
            double diesel = 0;
            double lpg = 0;
            foreach (Pump[] lane in allLanes)
            {
                foreach (Pump pump in lane)
                {
                    unleaded += pump.unleadedDispensed;
                    diesel += pump.dieselDispensed;
                    lpg += pump.lpgDispensed;
                }
            }
            Console.WriteLine($"Unleaded dispensed: {unleaded}litres");
            Console.WriteLine($"Diesel dispensed: {diesel}litres");
            Console.WriteLine($"LPG dispensed: {lpg}litres");
        }
    }

}