//used for waiting and fuelling
using System.Collections; //used for Arraylists

namespace Broken_Petrol_Ltd_2
{
    class Program
    {
        public static int VehicleCounter = 0;
        public static ArrayList existingVehicles = new ArrayList();
        public static Pump[] lane1 = { new Pump(1), new Pump(2), new Pump(3) };
        public static Pump[] lane2 = { new Pump(4), new Pump(5), new Pump(6) };
        public static Pump[] lane3 = { new Pump(7), new Pump(8), new Pump(9) };
        public static Pump[][] allLanes = { lane1, lane2, lane3 }; //a jagged array containing all the lanes that each contain the pumps with numbers.
        static void Main(string[] args)
        {

            Random rnd = new Random();
            Timer carAdder = new Timer(CarAdder, null, 0, rnd.Next(1500, 2200));
        }

        public static async Task Assigner() //an asynchronous method that will run throughout the entire program to ensure that vehicles are assigned as they come in
        {
            await Task.Run(() => 
            {
                while (true)
                { 
                    //assign vehicles to an available pump
                }
            });
        }

        public static void CarAdder(object o)
        {
            Random rnd = new Random();
            int carType = rnd.Next(0, 3);
            if (carType == 0)
            {
                existingVehicles.Add(new Car(VehicleCounter));
            }
            else if (carType == 1)
            {
                existingVehicles.Add(new Van(VehicleCounter));
            }
            else
            {
                existingVehicles.Add(new HGV(VehicleCounter));
            }
            VehicleCounter++;
        }
    }

}