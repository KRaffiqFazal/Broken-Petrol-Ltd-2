﻿namespace Broken_Petrol_Ltd_2
{
    internal class Vehicle
    {
        public static String type;
        public static String fuelType;
        public static int maxFuelCapacity;
        public static int fuelInTank;
        public static Timer fuellingTime;
        public static int fuellingTimeInt;
        public static Timer waitingTime;
        public static int vehicleID;
        public static bool hasWaited;
        public static bool hasFuelled;
        public Vehicle(int value)
        {
            Random rnd = new Random();
            waitingTime = new Timer(WaitingTime_Elapsed, null, 0, rnd.Next(1000, 2000));
            vehicleID = value;
            hasWaited = false;
            fuellingTime = new Timer(FuellingTime_Elapsed, null, Timeout.Infinite, fuellingTimeInt);

        }

        void WaitingTime_Elapsed(object o)
        {
            hasWaited = true;
            waitingTime.Dispose();
            fuellingTime.Dispose();
        }
        void FuellingTime_Elapsed(object o)
        {
            hasFuelled = true;
            fuellingTime.Dispose();

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
