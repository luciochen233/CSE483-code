using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

// Threading
using System.Threading;

// debug
using System.Diagnostics;


namespace SampleThread
{
    public partial class Model
    {
        private Thread _threadA = null;
        private Thread _threadB = null;
        private Random _randomNumber;
        private bool _threadAIsSuspended = false;
        private bool _threadBIsSuspended = false;
        private bool _isThreadARunning = false;
        private bool _isThreadBRunning = false;

        /// <summary>
        /// constructor
        /// </summary>
        public Model()
        {
            _randomNumber = new Random();
        }

        /// <summary>
        /// called by view when any of the buttons are clicked
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public String ButtonClicked(String buttonName)
        {
            if (buttonName.Contains("Start"))
            {
                return Start(buttonName);
            }
            else if (buttonName.Contains("Stop"))
            {
                return Stop(buttonName);
            }
            else if (buttonName.Contains("Suspend"))
            {
                return Suspend(buttonName);
            }
            else if (buttonName.Contains("Resume"))
            {
                return Resume(buttonName);
            }

            return "Unknown Button Clicked\n";
        }

        /// <summary>
        /// start a thread. this means creating the instance if it does not exist
        /// or simply resuming it if the instance already existes
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        String Start(String name)
        {
            if (name.Contains("A"))
            {
                _threadAIsSuspended = false;
                if (_threadA == null)
                {
                    _threadA = new Thread(new ThreadStart(ThreadAFunction));
                    _isThreadARunning = true;
                    _threadA.Start();
                    return "Thread A Started";
                }
                else
                    return "Thread A Already Started\n";
            }
            else if (name.Contains("B"))
            {
                _threadBIsSuspended = false;
                if (_threadB == null)
                {
                    _threadB = new Thread(new ThreadStart(ThreadBFunction));
                    _isThreadBRunning = true;
                    _threadB.Start();
                    return "Thread B Started";
                }
                else
                    return "Thread B Already Started\n";
            }

            return "";
        }

        /// <summary>
        /// aborts a thread. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        String Stop(String name)
        {
            if (name.Contains("A"))
            {
                if (_threadA != null && _threadA.IsAlive)
                {
                    //                    _threadA.Abort();
                    _isThreadARunning = false;
                    _threadA = null;
                    return "Thread A Aborted\n";
                }
                else
                    return "Thread A is not running\n";
            }
            if (name.Contains("B"))
            {
                if (_threadB != null && _threadB.IsAlive)
                {
                    //                    _threadB.Abort();
                    _isThreadBRunning = false;
                    _threadB = null;
                    return "Thread B Aborted\n";
                }
                else
                    return "Thread B is not running\n";
            }

            return "";
        }

        /// <summary>
        /// suspend a running thread
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        String Suspend(String name)
        {
            if (name.Contains("A"))
            {
                if (_threadA != null && _threadA.IsAlive)
                {
                    _threadAIsSuspended = true;
                    return "Thread A Suspended\n";
                }
                else
                    return "Thread A is Not Running. Cannot Suspend\n";
            }
            else if (name.Contains("B"))
            {
                if (_threadB != null && _threadB.IsAlive)
                {
                    _threadBIsSuspended = true;
                    return "Thread B Suspended\n";
                }
                else
                    return "Thread B is Not Running. Cannot Suspend\n";
            }

            return "";
        }

        /// <summary>
        /// resume a suspended thread
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        String Resume(String name)
        {
            if (name.Contains("A"))
            {
                if (_threadA != null && _threadA.IsAlive && _threadAIsSuspended)
                {
                    _threadAIsSuspended = false;
                    return "Thread A Resumed\n";
                }
                else
                    return "Thread A is Not Running. Cannot be resumed\n";
            }
            else if (name.Contains("B"))
            {
                if (_threadB != null && _threadB.IsAlive && _threadBIsSuspended)
                {
                    _threadBIsSuspended = false;
                    return "Thread B Resumed\n";
                }
                else
                    return "Thread B is Not Running. Cannot be resumed\n";
            }

            return "";
        }

        /// <summary>
        /// Thread A
        /// </summary>
        void ThreadAFunction()
        {
            try
            {
                while (_isThreadARunning)
                {
                    Thread.Sleep(2);
                    if (_threadAIsSuspended) continue;

                    // below are all the things this thread needs to do
                    ThreadAData = _randomNumber.Next().ToString();
                }
            }
            catch (ThreadAbortException)
            {
                Debug.Write("Thread A Aborted\n");
            }
        }

        /// <summary>
        /// Thread B
        /// </summary>
        void ThreadBFunction()
        {
            try
            {
                while (_isThreadBRunning)
                {
                    Thread.Sleep(2);
                    if (_threadBIsSuspended) continue;

                    // below are all the things this thread needs to do
                    ThreadBData = _randomNumber.Next().ToString();
                }
            }
            catch (ThreadAbortException)
            {
                Debug.Write("Thread B Aborted\n");
            }
        }

        /// <summary>
        /// Model cleanup code
        /// </summary>
        public void CleanUp()
        {
            if (_threadA != null && _threadA.IsAlive)
                _threadA.Abort();
            if (_threadB != null && _threadB.IsAlive)
                _threadB.Abort();
        }

    }
}
