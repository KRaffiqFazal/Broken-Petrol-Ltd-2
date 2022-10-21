//used for waiting and fuelling
using System.Collections.Generic; //used for Lists

namespace Broken_Petrol_Ltd_2
{
    class Program
    {
        public static int VehicleCounter = 0;
        public static bool cont = true; //will signal async functions to stop when it becomes false.
        public static List<Vehicle> existingVehicles = new List<Vehicle>();
        public static Pump[] lane1 = { new Pump(1), new Pump(2), new Pump(3) };
        public static Pump[] lane2 = { new Pump(4), new Pump(5), new Pump(6) };
        public static Pump[] lane3 = { new Pump(7), new Pump(8), new Pump(9) };
        public static Pump[][] allLanes = { lane1, lane2, lane3 }; //a jagged array containing all the lanes that each contain the pumps with numbers.
        static void Main(string[] args)
        {

            Random rnd = new Random();
            Timer carAdder = new Timer(CarAdder, null, 0, rnd.Next(1500, 2200));
            Assigner();
            WaitKick();
            FuelledKick();
        }

        public static async Task Assigner() //an asynchronous method that will run throughout the entire program to ensure that vehicles are assigned as they come in
        {
            await Task.Run(() => 
            {
                while (cont)
                {
                    //assign vehicles to an available pump
                    foreach (Vehicle item in existingVehicles)
                    { 
                        
                    }
                }
            });
        }
        public static async Task WaitKick() //an asynchronous method that will run throughout the entire program to ensure that vehicles are kicked from the list if they finish waiting
        {
            await Task.Run(() =>
            {
                while (cont)
                {
                    //checks list of vehicles to see which want to leave
                    foreach (Vehicle item in existingVehicles)
                    {
                        if (Vehicle.hasWaited == true)
                        {
                            existingVehicles.Remove(item);
                        }
                    }
                }
            });
        }
        public static async Task FuelledKick() //an asynchronous method that will run throughout the entire program to ensure that vehicles are kicked from the list once they start fuelling.
        {    
            await Task.Run(() =>
            {
                while (cont)
                {
                    //checks list of vehicles to see which have started fuelling and so can be kicked
                    foreach (Vehicle item in existingVehicles)
                    {
                        if (Vehicle.isFuelling == true)
                        {
                            existingVehicles.Remove(item);
                        }
                    }
                }
            });
        }
        public static void CarAdder(object o)
        { 
            existingVehicles.Add(new Vehicle(VehicleCounter));
            VehicleCounter++;
        }
    }

}