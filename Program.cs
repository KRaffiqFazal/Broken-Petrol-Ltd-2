using System;
using System.Threading;
using System.IO; //used for reading files
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.ComponentModel;

namespace Broken_Petrol_Ltd_2 //make text file uneditable by an employee, create new text file to store path, fuel costs and login details
{
    partial class Program
    {
        static String PATH = System.Reflection.Assembly.GetExecutingAssembly().Location; //Automatically finds the directory to reduce what the end user needs to do

        static double UNLEADED_COST, DIESEL_COST, LPG_COST; //number of pounds per litre

        static List<String> username = new List<String>();
        static List<String> password = new List<String>(); //values will be found via a file, but making them accessible to other functions allows them to be changed without needing a function to call and change it

        const int DAY_LENGTH = 30000; //the length of a day in the program in milliseconds
        static Vehicle[] existingVehicles = new Vehicle[5], fuelling = new Vehicle[9]; //5 vehicles can wait at one time and 9 vehicles can be fulled (at each pump) at the same time

        static Stopwatch[] unoccupiedCounter = new Stopwatch[9]; //times how long each pump has been unoccupied
        static int carsFuelled = 0, carsLeft = 0; //counts the number of cars that have fuelled and left and waited and left
        
        static Pump[] lane1 = { new Pump(1), new Pump(2), new Pump(3) }, lane2 = { new Pump(4), new Pump(5), new Pump(6)}, lane3 = { new Pump(7), new Pump(8), new Pump(9)}; //there are 3 lanes in the station, each has 3 pumps
        static Pump[][] allLanes = { lane1, lane2, lane3 }; //a jagged array containing all the lanes that each contain the pumps with numbers.
        
        static User currentUser = new User();
        static bool cont = false; //will signal functions to stop when it becomes false, will become true if login successful

        static Stopwatch dayWatch = new Stopwatch(); //declares a stopwatch used to find efficiency ratings of all the pumps
        public static void Main(string[] args)
        {

            Random rnd = new Random();
            Vehicle[] existingVehicles = new Vehicle[5]; //a queue that cars are sent to, to wait for their turns
            Vehicle[] fuelling = new Vehicle[9]; //a queue that cars are sent to from existingVehicles whilst they fuel
            System.Threading.Timer carAdder = new System.Threading.Timer(CarAdder, null, 0, rnd.Next(1500, 2201));
            System.Threading.Timer ender = new System.Threading.Timer(Stopper, null, Timeout.Infinite, 0);
            bool temp = false; //used to run a function after the login only once which starts the ender timer
            if (!cont)
            {
                PATH = PATH.Substring(0, PATH.IndexOf(@"Broken Petrol Ltd 2\") + @"Broken Petrol Ltd 2\".Length - 1);//works out the path of the program by getting rid of directories that I do not want to store text files in
            }
            UpdateLogins();
            UpdatePrices();//makes sure that logins and prices are the newest versions 
            if (!cont)
            {
                Login(0, 3);
            }
            while (cont) //cont will become false once the "day" is over, this stops vehicles from being assigned/kicked/leaving.
            {
                if (cont && !temp) //run one time once correct username/password used
                {
                    
                    dayWatch.Reset();
                    dayWatch.Start();
                    for (int i=0; i < unoccupiedCounter.Length; i++) 
                    {
                        unoccupiedCounter[i] = Stopwatch.StartNew();
                    }
                    ender.Change(DAY_LENGTH, 0);
                    temp = true;
                }
                Assigner();
                WaitKick();
                FuelledKick();
                NotFuellingAnymore();
            }
            carAdder.Dispose(); //disposes the timer that creates vehicles now that the day has finished
            ender.Dispose();
            cont = true;
            while (cont) //will run through to simulate the last vehicles that are waiting/fuelling to leave without new vehicles from being created
            {
                cont = false;
                Assigner();
                WaitKick();
                FuelledKick();
                NotFuellingAnymore();
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
            WriteToFile();
            EndOfTheDay();
        }

    }

}