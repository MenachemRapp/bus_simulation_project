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

        public override string ToString()
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
            string registrationString = String.Format("{0}-{1}-{2}", prefix, middle, suffix);

            return String.Format("[ {0}, {1} ]", registrationString, aliya.ToShortDateString());
        }

     
    }
}
