using System;
using System.Timers;

namespace Broken_Petrol_Ltd_2
{
    class CarAdderTimer: System.Timers.Timer //use inheritance to make a custom timer type that has methods and attributes
    {
        public Vehicle[] existingVehicles = new Vehicle[5];

        public CarAdderTimer()
        {
            Random rnd;
            this.AutoReset = true;
            this.Interval = rnd.Next(1500, 2201);


        }
    }
}
