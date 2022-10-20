//used for waiting and fuelling
using System.Collections; //used for Arraylists

namespace Broken_Petrol_Ltd_2
{
    class Program
    {
        int VehicleCounter = 0;
        ArrayList existingVehicles = new ArrayList();
        
        static void Main(string[] args)
        {

            Random rnd = new Random();
            Timer carAdder = new Timer(CarAdder, null, 0, rnd.Next(1500, 2200));
            int choice = rnd.Next(0, 3);

        }

        void CarAdder(object o)
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