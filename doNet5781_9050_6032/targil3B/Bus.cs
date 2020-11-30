﻿using System;
using System.Threading;
namespace Targil1
{
    public class Bus
    {
        private string registration;
        private DateTime aliya, maintanence_date;
        private int kilometer_total, kilometer_fuel, kilometer_maintanence;
        private bool dangerous;
        private BUS_STATUS status;
        private int time_status;
        const int TIME_OF_FIXING = 24 * 60 * 60, TIME_OF_REFULING = 2 * 60 * 60, MIN_TIME_KM = (60 * 60) / 20, MAX_TIME_KM = (60 * 60) / 50;
        const int CONVERT_REALY_TIME_TO_TIME_COMPUTER = 1 / 600, TIME_REALY_TO_TIMER=60;

        public bool CanDrive()
        {
            return (status == BUS_STATUS.AVAILABLE && kilometer_fuel > 0 && kilometer_maintanence > 0);
             ////tho chak the date of maintance, 
        }

        public Bus(string registration, DateTime aliya)
        {
            this.aliya = aliya;
            this.Registration = registration;
            this.maintanence_date = aliya;
            this.kilometer_total = 0;
            this.kilometer_maintanence = 0;
            this.kilometer_fuel = 0;
            this.dangerous = false;
            time_status = 0;
            if (DateTime.Now.AddYears(-1) >= maintanence_date)
                this.dangerous = true;
        }

        public BUS_STATUS bus_status
        {
            get
            {
                return status;
            }
            private set
            {
                status = value;
            }
        }

        private void ChangeStatus(BUS_STATUS new_status, int km=0)
        {
            int timer = 0;
            switch (new_status)
            {
                
                case BUS_STATUS.DRIVING:
                    ///to do_random!!!!!!!!!!!!!!!!!!!!!!!!!
                    timer = km * MIN_TIME_KM;
                    break;
                case BUS_STATUS.REFULING:
                    timer = TIME_OF_REFULING;
                    break;
                case BUS_STATUS.FIXING:
                    timer = TIME_OF_FIXING;
                    break;
                case BUS_STATUS.AVAILABLE:
                    break;
                default:
                    break;
            }

        }
        public int TimeStatus
        {
            get
            {
                return time_status;
            }
            set
            {
                if (value > 0)
                {
                    time_status = value;
                    Thread thread1 = new Thread(Timer);
                    thread1.Start();
                }

            }
        }

        private void Timer()
        {
            while (time_status > 0)
            {
                Thread.Sleep(TIME_REALY_TO_TIMER * CONVERT_REALY_TIME_TO_TIME_COMPUTER * 1000);
                time_status -= (TIME_REALY_TO_TIMER * CONVERT_REALY_TIME_TO_TIME_COMPUTER);
            }
            status = BUS_STATUS.AVAILABLE;
            if (time_status < 0)
                time_status = 0;
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
            ChangeStatus(BUS_STATUS.DRIVING, km);
            kilometer_fuel += km;
            kilometer_maintanence += km;
            kilometer_total += km;
            return true;
        }

        //refuels the bus
        public void refuel()
        {
            ChangeStatus(BUS_STATUS.REFULING);
            this.kilometer_fuel =0;
        }

        //maintains the bus
        public void maintain()
        {
            ChangeStatus(BUS_STATUS.FIXING);
            this.kilometer_maintanence = 0;
            this.maintanence_date= DateTime.Now;
            this.dangerous = false;
        }
        


    }
}
