using System.Reflection;
using System;
using System.Timers;
namespace Broken_Petrol_Ltd_2
{
    class Vehicle
    {
        public String type;
        public String fuelType;
        public int maxFuelCapacity;
        public int fuelInTank;
        public System.Timers.Timer fuellingTime;
        public int fuellingTimeInt;
        public System.Timers.Timer waitingTime;
        public bool hasWaited = false; //will always be true for newly created vehicles
        public bool isFuelling = false;
        public bool isFuelled = false;
        public Vehicle()
        {
            Random rnd = new Random();

            String[] model = { "Car", "Van", "HGV" }; //types of vehicles that can be created
            int[] maxFuel = { 50, 80, 150 }; //respective max fuel capacity of those vehicles
            String[] fuelTypes = { "Unleaded", "Diesel", "LPG"};

            waitingTime = new System.Timers.Timer(rnd.Next(1000, 2000)); //a timer that will only trigger once to signal that the vehicle must leave
            waitingTime.Enabled = true;
            waitingTime.Elapsed += WaitingTime_Elapsed;

            int temp = rnd.Next(0, 3); //used to select the vehicle type and hence its max fuel capacity.

            type = model[temp]; //the fuel capacity is based off of the type of car and the fuel in the car is based off its max capacity
            maxFuelCapacity = maxFuel[temp];
            fuelInTank = rnd.Next(5, maxFuelCapacity / 2);

            temp = Convert.ToInt32(Math.Floor((maxFuelCapacity - fuelInTank) / 1.5)); //works out the time in seconds it would take to fuel the car to max to use as a top boundary for how long it can fuel for
            fuellingTimeInt = rnd.Next(1000, temp * 1000);
            fuellingTime = new System.Timers.Timer(fuellingTimeInt); //once specified will trigger to signal the time it takes for a vehicle to fuel
            fuellingTime.AutoReset = false;
            fuellingTime.Elapsed += FuellingTime_Elapsed;
            fuellingTime.Enabled = false;
            if (type.Equals("Car"))
            {
                fuelType = fuelTypes[rnd.Next(0, 3)]; //Car can use any type of fuel
            }
            else if (type.Equals("HGV"))
            {
                fuelType = fuelTypes[1]; //HGV can only use diesel
            }
            else
            {
                fuelType = fuelTypes[rnd.Next(1, 3)]; //Van can only use diesel or LPG
            }

        }

        private void WaitingTime_Elapsed(object? sender, ElapsedEventArgs e)
        {
            hasWaited = true;
            waitingTime.Dispose();
            fuellingTime.Dispose();
        }

        private void FuellingTime_Elapsed(object? sender, ElapsedEventArgs e)
        {
            isFuelled = true;
            fuellingTime.Stop();
            fuellingTime.Dispose();
        }
        public void StartingFuelling()
        {
            waitingTime.Stop();
            waitingTime.Dispose();
            fuellingTime.Enabled = true;
            isFuelling = true;
            waitingTime.Dispose();
        }
    }
}
