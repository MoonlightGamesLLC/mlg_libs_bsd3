using System;
using System.Diagnostics;

namespace MoonlightGames.Net
{
    /// <summary>
    /// The SmartStopWatch can print the time since its constructor
    /// was called
    /// </summary>
    public class SmartStopwatch
    {
        private readonly Stopwatch watch;

        /// <summary>
        /// Starts the stopwatch
        /// </summary>
        public SmartStopwatch()
        {
            watch = new Stopwatch();
            watch.Start();
        }

        /// <summary>
        /// Stops the stopwatch and prints the time
        /// </summary>
        /// <param name="meta"></param>
        public void Print(string meta)
        {
            watch.Stop();
            Debug.WriteLine(string.Format("{0}:{1}ms", meta, watch.ElapsedMilliseconds), "Timer");
        }
    }
}

