using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

// TimerQueueTimer
using PrecisionTimers;

// Stopwatch, Debug
using System.Diagnostics;

// WPF Timer
using System.Windows.Threading;

// Threads
using System.Threading;

// Timer.Timer
using System.Timers;


namespace TimerDemo
{
    partial class Model : INotifyPropertyChanged
    {
        // Stopwatch is a class that allows us to measure time very accurately
        Stopwatch stopWatch;

        // flags indicating whether timers are active. 
        // these help to make for a smoother exit
        bool _mmTimerRunning = false;
        bool _netDispatchTimerRunning = false;
        bool _netThreadTimerRunning = false;
        bool _netTimerTimerRunning = false;

        public Model()
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }


        public void StopTimers()
        {
            MMTimerStart(false);
            NETDispatchTimerStart(false);
            NetThreadSleepTimerStart(false);
            NETTimerTimerStart(false);
        }

        #region MM Timer Stuff

        // used for measuring the period of the Multi Media timer
        uint MMTimerTicks = 0;
        long MMTimerTotalTime = 0;
        long MMPreviousTime;

        // this delegate is needed for the multi media timer defined 
        // in the TimerQueueTimer class.
        PrecisionTimers.TimerQueueTimer.WaitOrTimerDelegate timerCallbackDelegate;
        PrecisionTimers.TimerQueueTimer qt;

        public bool MMTimerStart(bool startStop)
        {
            if (startStop == true)
            {
                // reset out timing counters
                MMTimerTicks = 0;
                MMTimerTotalTime = 0;
                MMPreviousTime = stopWatch.ElapsedMilliseconds;

                // create our multi-media timer
                qt = new TimerQueueTimer();
                timerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(MMTimerCallback);


                try
                {
                    // create a Multi Media Hi Res timer.
                    qt.Create(MMTimerPeriod, MMTimerPeriod, timerCallbackDelegate);
                }
                catch (QueueTimerException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("Error from GetLastError = {0}", ex.Error);
                    return false;
                }

                _mmTimerRunning = true;
                return true;
            }
            else if (_mmTimerRunning == true)
            {
                try
                {
                    // delete Multi Media timer
                    qt.Delete();
                }
                catch (QueueTimerException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("Error from GetLastError = {0}", ex.Error);
                    return false;
                }

                _mmTimerRunning = false;
                return true;
            }

            return true;
        }

        private void MMTimerCallback(IntPtr pWhat, bool success)
        {

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!qt.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            float averageTime;

            // add time elapsed since previous callback to our total time
            MMTimerTotalTime += stopWatch.ElapsedMilliseconds - MMPreviousTime;

            // resent previous time to current time
            MMPreviousTime = stopWatch.ElapsedMilliseconds;

            // increment the number of times the callback was called over the time period
            MMTimerTicks++;

            try
            {
                // calculate average period over last 500ms and display
                if (MMTimerTicks >= 500 / MMTimerPeriod)
                {
                    averageTime = (float)MMTimerTotalTime / (float)MMTimerTicks;
                    MMTimerAverage = averageTime;

                    // reset counters
                    MMTimerTicks = 0;
                    MMTimerTotalTime = 0;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // done in callback. OK to delete timer
            qt.DoneExecutingCallback();
        }

    #endregion

        #region .NET Dispatch Timer Stuff

    // used for measuring the period of the .NET dispatch timer
    uint NETDispatchTimerTicks = 0;
        long NETDispatchTimerTotalTime = 0;
        long NETDispatchTimerPreviousTime;

        DispatcherTimer dotNetDispatchTimer;

        public bool NETDispatchTimerStart(bool startStop)
        {
            if (startStop == true)
            {
                // reset counters for timing
                NETDispatchTimerTicks = 0;
                NETDispatchTimerTotalTime = 0;
                NETDispatchTimerPreviousTime = stopWatch.ElapsedMilliseconds;

                // set timer interval from GUI and start timer
                dotNetDispatchTimer = new DispatcherTimer();
                dotNetDispatchTimer.Tick += new EventHandler(NETDispatchTimerOnTick);
                dotNetDispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)NETDispatchTimerPeriod);
                dotNetDispatchTimer.Start();

                _netDispatchTimerRunning = true;
            }
            else if (_netDispatchTimerRunning == true)
            {
                // stop timer
                dotNetDispatchTimer.Stop();
            }

            return true;
        }

        public void NETDispatchTimerOnTick(object obj, EventArgs ea)
        {
            float averageTime;

            // add time elapsed since previous callback to our total time
            NETDispatchTimerTotalTime += stopWatch.ElapsedMilliseconds - NETDispatchTimerPreviousTime;

            // resent previous time to current time
            NETDispatchTimerPreviousTime = stopWatch.ElapsedMilliseconds;

            // increment the number of times the callback was called over the time period
            NETDispatchTimerTicks++;

            try
            {
                // calculate average period over last 500ms and display
                if (NETDispatchTimerTicks >= 500 / NETDispatchTimerPeriod)
                {
                    averageTime = (float)NETDispatchTimerTotalTime / (float)NETDispatchTimerTicks;
                    NETDispatchTimerAverage = averageTime;

                    // reset counters
                    NETDispatchTimerTicks = 0;
                    NETDispatchTimerTotalTime = 0;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        #endregion

        #region .NET Timer Timer

        // used for measuring the period of the .NET timer timer
        uint NETTimerTimerTicks = 0;
        long NETTimerTimerTotalTime = 0;
        long NETTimerTimerPreviousTime;

        System.Timers.Timer dotNetTimerTimer;

        public bool NETTimerTimerStart(bool startStop)
        {
            if (startStop == true)
            {
                dotNetTimerTimer = new System.Timers.Timer(NETTimerTimerPeriod);
                dotNetTimerTimer.Elapsed += new ElapsedEventHandler(NetTimerTimerHandler);
                dotNetTimerTimer.Start();

            }
            else if (_netTimerTimerRunning)
            {
                dotNetTimerTimer.Stop();
            }

            return true;
        }

        private void NetTimerTimerHandler(object source, ElapsedEventArgs e)
        {
            float averageTime;

            // add time elapsed since previous callback to our total time
            NETTimerTimerTotalTime += stopWatch.ElapsedMilliseconds - NETTimerTimerPreviousTime;

            // resent previous time to current time
            NETTimerTimerPreviousTime = stopWatch.ElapsedMilliseconds;

            // increment the number of times the callback was called over the time period
            NETTimerTimerTicks++;

            try
            {
                // calculate average period over last 500ms and display
                if (NETTimerTimerTicks >= 500 / NETTimerTimerPeriod)
                {
                    averageTime = (float)NETTimerTimerTotalTime / (float)NETTimerTimerTicks;
                    NETTimerTimerAverage = averageTime;

                    // reset counters
                    NETTimerTimerTicks = 0;
                    NETTimerTimerTotalTime = 0;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #region .NET Thread

        // Thread
        Thread threadSleepTimerThread;
        Int32 threadSleepTime;
        bool sleepTimerThreadIsRunning;

        // used for measureing period of Thread.Sleep
        uint NETThreadSleepTicks = 0;
        long NETThreadSleepTotalTime = 0;
        long NETThreadSleepPreviousTime = 0;

        public bool NetThreadSleepTimerStart(bool startStop)
        {
            if (startStop == true)
            {
                // set timer interval from GUI and start timer
                threadSleepTime = (int)NETThreadSleepPeriod;

                // reset counters for timing
                NETThreadSleepTicks = 0;
                NETThreadSleepTotalTime = 0;

                NETThreadSleepPreviousTime = stopWatch.ElapsedMilliseconds;

                // create a new thread
                threadSleepTimerThread = new Thread(new ThreadStart(ThreadFunction));
                threadSleepTimerThread.Start();

                _netThreadTimerRunning = true;
            }
            else if (_netThreadTimerRunning == true)
            {
                sleepTimerThreadIsRunning = false;
                threadSleepTimerThread.Join();
            }

            return true;
        }

        public void ThreadFunction()
        {
            float averageTime;

            sleepTimerThreadIsRunning = true;

            try
            {
                while (sleepTimerThreadIsRunning == true)
                {
                    // add time elapsed since previous callback to our total time
                    NETThreadSleepTotalTime += stopWatch.ElapsedMilliseconds - NETThreadSleepPreviousTime;

                    // resent previous time to current time
                    NETThreadSleepPreviousTime = stopWatch.ElapsedMilliseconds;

                    // increment the number of times the callback was called over the time period
                    NETThreadSleepTicks++;

                    // calculate average period over last 500ms and display
                    if (NETThreadSleepTicks == 500 / NETThreadSleepPeriod)
                    {
                        averageTime = (float)NETThreadSleepTotalTime / (float)NETThreadSleepTicks;
                        NETThreadSleepAverage = averageTime;

                        // reset counters
                        NETThreadSleepTicks = 0;
                        NETThreadSleepTotalTime = 0;
                    }

                    Thread.Sleep(threadSleepTime);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        #endregion


    }
}
