using System.Globalization;
namespace Broken_Petrol_Ltd_2
{
    class User
    {
        public String Username
        {get; set;}

        public String Password
        { get; set; }

        public double totalCost
        { get; set; }

    public double comission
        { get; set; }
        public User()
        {
            Username = "............"; //a placeholder value when program loads, same with below
            Password = "************";
            totalCost = 0;
            comission = 0;
        }
        
        public String HidePassword()
        {
            String temp = "";
            foreach (char letter in Password)
            {
                temp += "*"; //the entered password is hidden with asterisks
            }
            return temp;
        }
        public void ClearFields()
        {
            Username = "............";
            Password = "************";
        }

        public String CostInString()
        {
            return totalCost.ToString("C", CultureInfo.CurrentCulture).Substring(1);
        }
        public String CommissionInString() 
        {
            return comission.ToString("C", CultureInfo.CurrentCulture).Substring(1);
        }
    }
}

