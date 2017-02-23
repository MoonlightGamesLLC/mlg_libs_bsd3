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
	}
}

