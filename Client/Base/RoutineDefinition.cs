﻿using System;
using System.Diagnostics;
using LunaClient.Utilities;

namespace LunaClient.Base
{
    public enum RoutineExecution
    {
        Update,
        FixedUpdate
    }

    public class RoutineDefinition
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// Routine name is the name of the method it runs
        /// </summary>
        public string Name => Method.Method.Name;

        /// <summary>
        /// Interval in ms at witch this routine will be executed. Set it to 0 if you want to run it on every update/fixed update
        /// </summary>
        public int IntervalInMs { get; set; }

        /// <summary>
        /// Method that this routine will execute
        /// </summary>
        public Action Method { private get; set; }

        /// <summary>
        /// Select here if the routine must run in update or in fixed update
        /// </summary>
        public RoutineExecution Execution { get; set; }

        /// <summary>
        /// Profiler to see the performance of the routine
        /// </summary>
        public ProfilerData Profiler { get; } = new ProfilerData();

        #region Constructors

        private RoutineDefinition()
        {
            _stopwatch.Start();
        }

        /// <summary>
        /// Create a routine definition. Set the interval to 0 if you want to execute it on every update/fixed update
        /// </summary>
        public RoutineDefinition(int intervalInMs, RoutineExecution execution, Action method) : this()
        {
            IntervalInMs = intervalInMs;
            Execution = execution;
            Method = method;
        }

        #endregion

        /// <summary>
        /// Call this method to try to run the routine if the interval is ok
        /// </summary>
        public void RunRoutine()
        {
            if (IntervalInMs <= 0 || _stopwatch.ElapsedMilliseconds > IntervalInMs)
            {
                var startClock = ProfilerData.LmpReferenceTime.ElapsedTicks;

                Method.Invoke();

                Profiler.ReportTime(startClock);
                _stopwatch.Reset();
                _stopwatch.Start();
            }
        }
    }
}
