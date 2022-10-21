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
            Timer ender = new Timer(Stopper, null, 0, 30000);
            int i = 0;
            while (cont)
            {
                Assigner();
                WaitKick();
                FuelledKick();
                Console.WriteLine(i);
                i++;
            }

        }

        public static async Task Assigner() //an asynchronous method that will run throughout the entire program to ensure that vehicles are assigned as they come in
        {
            await Task.Run(() =>
            {
                    //assign vehicles to an available pump
                    foreach (Vehicle item in existingVehicles)
                    {
                        if (!item.isFuelling && !item.hasWaited)
                        {
                            foreach (Pump[] lane in allLanes)
                            {
                                if (lane[0].inUse) //if the first pump in the lane is not free, automatically check the next lane
                                {
                                    continue;
                                }
                                else if (lane[1].inUse && !lane[0].inUse) //if the second pump in the lane is being used but the first pump is not, go to first pump
                                {
                                    lane[0].inUse = true;
                                    item.StartingFuelling();
                                    fuelling[lane[0].pumpNumber] = item;
                                    Displayer();
                                }
                                else if (!lane[2].inUse && !lane[1].inUse && !lane[0].inUse) //if all pumps are free, furthest will be selected to fuel at.
                                {
                                    lane[2].inUse = true;
                                    item.StartingFuelling();
                                    fuelling[lane[2].pumpNumber] = item;
                                    Displayer();
                                }
                            }
                        }
                    }
            });
        }
        public static async Task WaitKick() //an asynchronous method that will run throughout the entire program to ensure that vehicles are kicked from the list if they finish waiting
        {
            await Task.Run(() =>
            {
                    //checks list of vehicles to see which want to leave
                    foreach (Vehicle item in existingVehicles)
                    {
                        if (item.hasWaited == true)
                        {
                            existingVehicles.Remove(item);
                            Displayer();
                        }
                    }
            });
        }
        public static async Task FuelledKick() //an asynchronous method that will run throughout the entire program to ensure that vehicles are kicked from the list once they start fuelling.
        {
            await Task.Run(() =>
            {                
                    //checks list of vehicles to see which have started fuelling and so can be kicked
                    foreach (Vehicle item in existingVehicles)
                    {
                        if (item.isFuelling == true)
                        {
                            existingVehicles.Remove(item);
                            Displayer();
                        }
                    }
                
            });
        }
        public static void Stopper(object o) //will stop the program from running when required by making cont false.
        {
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
            Console.WriteLine($"Pump {lane1[2]}: {lane1[2].inUse} |Pump {lane2[2]}: {lane2[2].inUse} |Pump {lane3[2]}: {lane3[2].inUse}");
            Console.WriteLine($"Pump {lane1[1]}: {lane1[1].inUse} |Pump {lane2[1]}: {lane2[1].inUse} |Pump {lane3[1]}: {lane3[1].inUse}");
            Console.WriteLine($"Pump {lane1[0]}: {lane1[0].inUse} |Pump {lane2[0]}: {lane2[0].inUse} |Pump {lane3[0]}: {lane3[0].inUse}");
            foreach (Vehicle item in existingVehicles)
            {
                if (!item.hasWaited && !item.isFuelling)
                {
                    Console.WriteLine(item.type);
                }
            }
        }
    }

}