using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; //^the above is imported by default when a class file is created in VS
using System.Timers; //used for waiting and fuelling

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
            vehicleID = value;

        }

        private void WaitingTime_Elapsed(object? sender, ElapsedEventArgs e)
        {
            
        }
    }
    internal class Car : Vehicle
    {
        public Car(int value) : base(value) //cars will have a fixed name to signify their type and a max fuel capacity, therfore I can use inheritance to show this
        {
            type = "Car";
            maxFuelCapacity = 50;
        }
    }
    internal class Van : Vehicle
    {
        public Van(int value) : base(value)//vans will have a fixed name to signify their type and a max fuel capacity, therfore I can use inheritance to show this
        {
            type = "Van";
            maxFuelCapacity = 80;
        }
    }
    internal class HGV : Vehicle
    {
        public HGV(int value) : base(value)//HGVs will have a fixed name to signify their type and a max fuel capacity, therfore I can use inheritance to show this
        {
            type = "HGV";
            maxFuelCapacity = 150;
        }
    }
}
