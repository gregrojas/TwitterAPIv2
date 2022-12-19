using System;
using System.Threading;

namespace TwitterAPIv2.Application
{
    public class Program
    {
        private Timer _timer = null;

        public static void Main(string[] args)
        {
            Timer _timer = new Timer(TimerCallback, null, 0, 10000);

            try
            {
                //Call controller to read volume stream and log metrics
                for (; ;)
                {
                    // add a sleep for 100 mSec to reduce CPU usage
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                _timer.Dispose();
            }
        }

        private static void TimerCallback(Object o)
        {
            // Display the date/time when this method got called.
            Console.WriteLine("In TimerCallback: " + DateTime.Now);
        }
    }
}
