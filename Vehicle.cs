using System.Reflection;

namespace Broken_Petrol_Ltd_2
{
    internal class Vehicle
    {
        public static String type;
        public static String fuelType;
        public static int maxFuelCapacity;
        public static int fuelInTank;
        public static Timer fuellingTime;
        public static int fuellingTimeInt;
        public static Timer waitingTime;
        public static int vehicleID;
        public static bool hasWaited;
        public static bool isFuelling;
        public static bool isFuelled;
        public Vehicle(int value)
        {
            Random rnd = new Random();

            String[] model = { "Car", "Van", "HGV" };
            int[] maxFuel = { 50, 80, 150 };

            waitingTime = new Timer(WaitingTime_Elapsed, null, 0, rnd.Next(1000, 2000));
            vehicleID = value; //a unique id that can be used to find a specific vehicle.
            hasWaited = false;
            fuellingTime = new Timer(FuellingTime_Elapsed, null, Timeout.Infinite, fuellingTimeInt); //once specified will trigger to signal the time it takes for a vehicle to fuel
            int temp = rnd.Next(0, 3); //used to select the vehicle type and hence its max fuel capacity.
            type = model[temp];
            maxFuelCapacity = maxFuel[temp];
            fuelInTank = rnd.Next(5, maxFuelCapacity / 2);
            temp = Convert.ToInt32(Math.Floor((maxFuelCapacity - fuelInTank) / 1.5));
            fuellingTimeInt = rnd.Next(1000, temp);

        }

        void WaitingTime_Elapsed(object o)
        {
            hasWaited = true;
            waitingTime.Dispose();
            fuellingTime.Dispose();
        }
        void FuellingTime_Elapsed(object o)
        {    
            fuellingTime.Dispose();
            isFuelled = true;

        }
        public static void StartingFuelling()
        {
            fuellingTime.Change(0, fuellingTimeInt);
            isFuelling = true;
            waitingTime.Dispose();
        }


    }
}
