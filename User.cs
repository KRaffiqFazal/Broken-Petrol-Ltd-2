using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (totalCost % 0.1 == 0)
            {
                return totalCost.ToString() + "0";
            }
            else
            { 
                return totalCost.ToString();
            }
        }
        public String CommissionInString() 
        {
            if (comission % 0.1 == 0)
            {
                return comission.ToString() + "0";
            }
            else
            {
                return comission.ToString();
            }
        }
    }
}

