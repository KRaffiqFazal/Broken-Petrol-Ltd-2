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
        public bool hasWaited;
        public Vehicle(int value)
        {
            Random rnd = new Random();
            waitingTime = new Timer(WaitingTime_Elapsed, null, 0, rnd.Next(1000, 2000));
            waitingTime.AutoReset = false;
            waitingTime.Enabled = true;
            vehicleID = value;
            hasWaited = false;

        }

        void WaitingTime_Elapsed(object o)
        {
            this.hasWaited = true;
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
