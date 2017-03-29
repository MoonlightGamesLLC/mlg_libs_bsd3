using System;
using System.Diagnostics;

namespace MoonlightGames.Net
{
    /// <summary>
    /// A collection of utilties for .NET projects
    /// </summary>
	public static class Utils
	{
        /// <summary>
        /// Easily log your exception with the stack trace
        /// </summary>
        /// <param name="ex"></param>
		public static void Write(Exception ex)
		{
			Debug.WriteLine(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
		}

        /// <summary>
        /// Convert a date time
        /// </summary>
        /// <param name="time"></param>
        /// <returns>Returns the difference in date time in milliseconds</returns>
        public static long GetDateTimeMS(DateTime time)
        {
            var epoch = GetEpoch();

            TimeSpan span = time - epoch;
            return (long)span.TotalMilliseconds;
        }

        /// <summary>
        /// Convert a date time
        /// </summary>
        /// <param name="time"></param>
        /// <returns>Returns the difference in date time in seconds</returns>
        public static long GetDateTimeS(DateTime time)
        {
            var epoch = GetEpoch();

            TimeSpan span = time - epoch;
            return (long)span.TotalSeconds;
        }

        /// <summary>
        /// Fetch the current time
        /// </summary>
        /// <returns>Current time in Seconds</returns>
        public static long GetNowTimeS()
        {
            var epoch = GetEpoch();
            DateTime time = DateTime.UtcNow;

            TimeSpan span = time - epoch;
            return (long)span.TotalSeconds;
        }


        /// <summary>
        /// Unix Epoch
        /// </summary>
        /// <returns>Returns the unix epoch DateTime</returns>
        private static DateTime GetEpoch()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch;
        }
    }
}

