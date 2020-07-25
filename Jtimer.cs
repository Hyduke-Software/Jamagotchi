using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;

namespace Jamagotchi
{
    class TimerExample
    {
        public void Jtimer()
        {
           

            // Create an AutoResetEvent to signal the timeout threshold in the
            // timer callback has been reached.
            var autoEvent = new AutoResetEvent(false);

            var statusChecker = new StatusChecker(15);

            // Create a timer that invokes CheckStatus after one second, 

                    
            
            Debug.WriteLine("{0:h:mm:ss.fff} Creating timer.\n", DateTime.Now);

            var stateTimer = new Timer(statusChecker.CheckStatus,
                                       autoEvent, 1000, 250);

            autoEvent.WaitOne();

            //autoEvent.WaitOne();
           stateTimer.Dispose();
          
        }
    }

    class StatusChecker
    {
        private int invokeCount;
        private int maxCount;

        public StatusChecker(int count)
        {
            invokeCount = 0;
            maxCount = count;
        }

        // This method is called by the timer delegate.
        public void CheckStatus(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            Debug.WriteLine("{0} Checking status {1,2}.",
                DateTime.Now.ToString("h:mm:ss.fff"),
                (++invokeCount).ToString());

            if (invokeCount == maxCount)
            {
                // Reset the counter and signal the waiting thread.
                invokeCount = 0;
                autoEvent.Set();
                //probably not where I should put my triggers

             

            }
        }
        public void refreshCaller()
        {
            MainWindow mainw = new MainWindow();
            mainw.RefreshPrintLabel();
            

        }
    }
}
