using System;

namespace Targil1
{
    public class Bus
    {
        private string registration;
        private DateTime aliya;

        public Bus(string registration, DateTime aliya)
        {
            this.aliya = aliya;
            this.Registration = registration;
        }

        public string Registration
        {
            //  get => regisration;
            get
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
    }
}