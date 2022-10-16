using System;
using System.Timers; //used for waiting and fuelling
using System.Collections; //used for Arraylists
using System.Threading;

namespace Broken_Petrol_Ltd_2
{
    class Program
    {
        public static int carCounter = 0;
        //public static var existingVehicles = new ArrayList();
        
        static void Main(string[] args)
        {

            Random rnd = new Random();
            System.Timers.Timer carAdder = new(rnd.Next(1500, 2200));
            carAdder.AutoReset = true;
            carAdder.Enabled = true;
            carAdder.Elapsed += (sender, e) => CarAdder_Elapsed(sender, e, rnd.Next(0, 3)); //https://stackoverflow.com/questions/9977393/how-do-i-pass-an-object-into-a-timer-event
            carAdder.Start();
            int choice = rnd.Next(0, 3);

        }

        private static void CarAdder_Elapsed(object? sender, ElapsedEventArgs e, int carType)
        {
            if (carType == 0)
            {

            }
            else if (carType == 1)
            {

            }
            else
            { 
                
            }
        }
    }

}