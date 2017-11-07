using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

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
        public static void WriteEx(Exception ex)
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

        /// <summary>
        /// Unix Epoch
        /// </summary>
        /// <returns>Converts a unix time to DateTime</returns>
        public static DateTime UnixToDateTime(int secondsSinceEpoch)
        {
            DateTime epochTime = GetEpoch();
            return epochTime.AddSeconds(secondsSinceEpoch);
        }

        /// <summary>
        /// Compresses the string.
        /// Courtesy of http://dotnet-snippets.com/snippet/compress-and-decompress-strings/612
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// Courtesy of http://dotnet-snippets.com/snippet/compress-and-decompress-strings/612
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
        }

		public static string UppercaseFirst(string s)
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}

			// Return char and concat substring.
			return char.ToUpper(s[0]) + s.Substring(1);
		}
	}
}

