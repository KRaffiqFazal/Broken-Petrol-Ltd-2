using System.ComponentModel;

namespace Broken_Petrol_Ltd_2
{
	partial class Program
	{
		public static void Stopper(object o) //will stop the program from running when required by making cont false.
		{
			cont = false;

		}
		public static void CarAdder(object o)
		{

			for (int i = 0, j = 1; i < existingVehicles.Length && j == 1; i++)
			{
				if (existingVehicles[i] == null)
				{
					existingVehicles[i] = new Vehicle();
					j = 0; //breaks out of the loop if a vehicle has been added
				}
			}
		}
	}
}