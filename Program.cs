//used for waiting and fuelling

namespace Broken_Petrol_Ltd_2
{
    class Program
    {
        public static int VehicleCounter = 0;
        public static bool cont = true; //will signal async functions to stop when it becomes false.
        public static List<Vehicle> existingVehicles = new List<Vehicle>();
        public static Vehicle[] fuelling = new Vehicle[9];
        public static Pump[] lane1 = { new Pump(1), new Pump(2), new Pump(3) };
        public static Pump[] lane2 = { new Pump(4), new Pump(5), new Pump(6) };
        public static Pump[] lane3 = { new Pump(7), new Pump(8), new Pump(9) };
        public static Pump[][] allLanes = { lane1, lane2, lane3 }; //a jagged array containing all the lanes that each contain the pumps with numbers.
        static void Main(string[] args)
        {

            Random rnd = new Random();
            Timer carAdder = new Timer(CarAdder, null, 0, rnd.Next(1500, 2200));
            Timer ender = new Timer(Stopper, null, 30000, 30000);
            while (cont)
            {
                Assigner();
                WaitKick();
                FuelledKick();
            }

        }

        public static void Assigner() //an asynchronous method that will run throughout the entire program to ensure that vehicles are assigned as they come in
        {
            //assign vehicles to an available pump
            for (int i = 0; i < existingVehicles.Count; i++)
            {
                if (!existingVehicles[i].isFuelling && !existingVehicles[i].hasWaited)
                {
                    Console.WriteLine(existingVehicles[i].isFuelling + " " + existingVehicles[i].hasWaited);

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
                            existingVehicles[i].StartingFuelling();
                            fuelling[lane[0].pumpNumber] = existingVehicles[i];
                            Displayer();
                            Console.WriteLine("Middle pump in use so going to closest");
                        }
                        else if (!lane[2].inUse && !lane[1].inUse && !lane[0].inUse) //if all pumps are free, furthest will be selected to fuel at.
                        {
                            lane[2].inUse = true;
                            existingVehicles[i].StartingFuelling();
                            fuelling[lane[2].pumpNumber] = existingVehicles[i];
                            Displayer();
                            Console.WriteLine("No pumps in use going to farthest");
                        }
                    }
                }
            }
        }
        public static void WaitKick() //an asynchronous method that will run throughout the entire program to ensure that vehicles are kicked from the list if they finish waiting
        {

            //checks list of vehicles to see which want to leave
            for (int i = 0; i < existingVehicles.Count; i++)
            {
                if (existingVehicles[i].hasWaited)
                {
                    existingVehicles.Remove(existingVehicles[i]);
                    Displayer();
                }
            }
        }
        public static void FuelledKick() //an asynchronous method that will run throughout the entire program to ensure that vehicles are kicked from the list once they start fuelling.
        {
            //checks list of vehicles to see which have started fuelling and so can be kicked
            for (int i = 0; i < existingVehicles.Count; i++)
            {
                if (existingVehicles[i].isFuelling)
                {
                    existingVehicles.Remove(existingVehicles[i]);
                    Displayer();
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
            if (existingVehicles.Count < 7)
            {
                existingVehicles.Add(new Vehicle(VehicleCounter));
                VehicleCounter++;
            }
        }
        public static void Displayer()
        {
            Console.Clear();
            Console.WriteLine($"Pump {lane1[2].pumpNumber}: {lane1[2].inUse} |Pump {lane2[2].pumpNumber}: {lane2[2].inUse} |Pump {lane3[2].pumpNumber}: {lane3[2].inUse}");
            Console.WriteLine($"Pump {lane1[1].pumpNumber}: {lane1[1].inUse} |Pump {lane2[1].pumpNumber}: {lane2[1].inUse} |Pump {lane3[1].pumpNumber}: {lane3[1].inUse}");
            Console.WriteLine($"Pump {lane1[0].pumpNumber}: {lane1[0].inUse} |Pump {lane2[0].pumpNumber}: {lane2[0].inUse} |Pump {lane3[0].pumpNumber}: {lane3[0].inUse}");
            for (int i = 0; i < existingVehicles.Count; i++)
            {
                if (!existingVehicles[i].hasWaited && !existingVehicles[i].isFuelling)
                {
                    Console.WriteLine(existingVehicles[i].type);
                }
            }
        }
    }

}