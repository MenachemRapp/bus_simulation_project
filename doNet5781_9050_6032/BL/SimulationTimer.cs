﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace BL
{
    sealed class SimulationTimer
    {

        #region singleton
        static readonly SimulationTimer instance = new SimulationTimer();
        static SimulationTimer() { }// static ctor to ensure instance init is done just before first usage
        SimulationTimer() { } // default => private
        public static SimulationTimer Instance { get => instance; }// The public Instance property to use
        #endregion

        public Stopwatch stopwatch= new Stopwatch();
        public int TimerRate { get; set; }
        public TimeSpan timerTimeWithDays { get; set; }

        event EventHandler ValueChanged;
        
        public Action<TimeSpan> ChangedAction;
        TimeSpan simulationTime;
        
        public void replaceEvent(Action<TimeSpan> newAction)
        {
            ValueChanged = null;
            ValueChanged += (x, y) => newAction(((ValueChangedEventArgs)y).NewValue); 
        }

        public void clearEvent()
        {
            ValueChanged = null;
        }

        public void run(TimeSpan startTime, int rate)
        {
            TimerRate = rate;
            stopwatch.Restart();
            while (stopwatch.IsRunning)
            {
                timerTimeWithDays = TimeSpan.FromTicks(stopwatch.Elapsed.Ticks * TimerRate) + startTime;
                     
                SimulationTime = timerTimeWithDays- TimeSpan.FromDays(timerTimeWithDays.Days);
                Thread.Sleep(1000);
            }

            
        }
        
        void OnValueChanged(ValueChangedEventArgs args)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, args);
            }
            if (ChangedAction!=null)
            {
                ChangedAction(args.NewValue);
            }
        }

        public TimeSpan SimulationTime
        {
            get { return simulationTime; }

            set
            {
                ValueChangedEventArgs args = new ValueChangedEventArgs(simulationTime, value);
                simulationTime = value;
                OnValueChanged(args);
            }
        }

       


    }
    public class ValueChangedEventArgs : EventArgs
    {
        public readonly TimeSpan OldValue, NewValue;

        public ValueChangedEventArgs(TimeSpan oldTemp, TimeSpan newTemp)
        {
            OldValue = oldTemp;
            NewValue = newTemp;
        }
    }
}
