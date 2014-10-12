using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Diagnostics;
using System.Web.UI.WebControls;

namespace SteamGamesPlayed
{
    public class AsynchroneRequest
    {
        private int minimum;
        private int maximum;
        private int currentValue = 0;
        private HttpContext context;
        private MainPage mainPage;

        private Thread mProgressThread;

        public int CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        public int Minimum
        {
            get { return minimum; }
            set { minimum = value; }
        }

        public int Maximum
        {
            get { return maximum; }
            set { maximum = value; }
        }

        public MainPage MainPage
        {
            get { return mainPage; }
            set { mainPage = value; }
        }

        public AsynchroneRequest(MainPage mainPage, Thread mProgressThread)
        {
            minimum = 0;
            maximum = 0;
            currentValue = 0;
            context = HttpContext.Current;
            this.mainPage = mainPage;

            this.mProgressThread = mProgressThread;
        }

        // Main function
        public void Start()
        {
            Debug.WriteLine("Retrieving scores...");

            RunAsynchronously(() =>
            {
                AsyncMethod();
            },
            () => // If not aborted and no error, then we do the callback
            {
                Debug.WriteLine("Retrieving scores ended successfully...");
            });
        }

        // Starts the operations of "AsyncMethod" asynchronously
        private void RunAsynchronously(Action method, Action callback)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    HttpContext.Current = context;        
                    method();
                }
                catch (ThreadAbortException ex) 
                {
                    Debug.WriteLine("ThreadAbortException... " + ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("RunAsynchronously failed... " + ex.Message);
                }

                // note: this will not be called if the thread is aborted
                if (callback != null) callback();
            });
        }

        private void AsyncMethod()
        {
            // Do your asynchrone stuff here
        }
    }
}