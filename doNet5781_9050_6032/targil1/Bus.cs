﻿using System;

namespace Targil1
{
    public class Bus
    {
        private string registration;
        private DateTime aliya, maintanence_date;
        private int kilometer_total, kilometer_fuel, kilometer_maintanence;
        private bool dangerous;


        public Bus(string registration, DateTime aliya)
        {
            this.aliya = aliya;
            this.Registration = registration;
            this.maintanence_date = aliya;
            this.kilometer_total = 0;
            this.kilometer_maintanence = 0;
            this.kilometer_fuel = 0;
            this.dangerous = false;
            if (DateTime.Now.AddYears(-1) >= maintanence_date)
                this.dangerous = true;
        }

        public string Registration
        {
            //  get => regisration;
            get
            {
                return registration;
            }

            
            private set
            {
                if (aliya.Year >= 2018 && value.Length == 8)
                {
                    //checks
                    registration = value;
                }
                else if (value.Length == 7)
                {
                    registration = value;
                }
                else
                {
                    throw new Exception("taarich lo takin");
                }
            }
        }

        public int Kilometer_maintanence
        {
            get
            {
                return kilometer_maintanence;
            }
        }

        //adds hypnes between parts of the regestration number
        public override string ToString()
        {
            return String.Format("[ {0}, {1} ]", str_registration(), aliya.ToShortDateString());
        }

        //The function returns a string of registration with hyphens in the middle
        public string str_registration()
        {
            string prefix, middle, suffix;
            if (aliya.Year < 2018)
            {
                prefix = registration.Substring(0, 2);
                middle = registration.Substring(2, 3);
                suffix = registration.Substring(5, 2);
            }
            else
            {
                prefix = registration.Substring(0, 3);
                middle = registration.Substring(3, 2);
                suffix = registration.Substring(5, 3);
            }
            return String.Format("{0}-{1}-{2}", prefix, middle, suffix);
        }

 /*
 * The function checks if the bus is suitable for travel.
If not returns false.
If it is suitable, returns true, and updates the mileage and fuel
 */
        public bool drive(int km)
        {
            //
            if (kilometer_maintanence+km>=20000 || (DateTime.Now.AddYears(-1)>= maintanence_date) || kilometer_fuel+km>1200)
             return false;
            kilometer_fuel += km;
            kilometer_maintanence += km;
            kilometer_total += km;
            return true;
        }

        //refuels the bus
        public void refuel()
        {
            this.kilometer_fuel =0;
        }

        //maintains the bus
        public void maintain()
        {
            this.kilometer_maintanence = 0;
            this.maintanence_date= DateTime.Now;
            this.dangerous = false;
        }
        


    }
}
