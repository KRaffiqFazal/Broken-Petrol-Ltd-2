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
        public Vehicle()
        { 
            
        }
    }
    internal class Car : Vehicle
    {
        public Car()
        { 
        
        }
    }
    internal class Van : Vehicle
    {
        public Van()
        { 
        
        }
    }
    internal class HGV : Vehicle
    {
        public HGV()
        { 
        
        }
    }
}
