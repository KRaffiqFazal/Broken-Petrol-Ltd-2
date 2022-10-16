using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Broken_Petrol_Ltd_2
{
    internal class Vehicle
    {
        public String type;
        public String fuelType;
        public int maxFuelCapacity;
        public int fuelInTank;
        public System.Timers.Timer fuellingTime;
        public int fuellingTimerInt;
        public System.Timers.Timer waitingTime;
        public int vehicleID;
        public Vehicle(int value)
        {
            Random rnd = new Random();
            waitingTime = new(rnd.Next(1500, 2200));
            waitingTime.AutoReset = false;
            waitingTime.Enabled = true;
            waitingTime.Elapsed += WaitingTime_Elapsed;
            //vehicleID = somethingFromMain

        }

        private void WaitingTime_Elapsed(object? sender, ElapsedEventArgs e)
        {
            
        }
    }
    internal class Car : Vehicle
    {
        public Car()
        {
            type = "Car";
            maxFuelCapacity = 50;
        }
    }
    internal class Van : Vehicle
    {
        public Van()
        {
            type = "Van";
            maxFuelCapacity = 80;
        }
    }
    internal class HGV : Vehicle
    {
        public HGV()
        {
            type = "HGV";
            maxFuelCapacity = 150;
        }
    }
}
