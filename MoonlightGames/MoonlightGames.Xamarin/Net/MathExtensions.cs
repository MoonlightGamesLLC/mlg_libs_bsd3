using System;
namespace MoonlightGames.Xamarin.Net
{
	/// <summary>
	/// A collection of math related functions
	/// </summary>
	public static class MathExtensions
	{
		/// <summary>
		/// Clamp the specified val between min and max, inclusive.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Clamp<T> (this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo (min) < 0) return min;
			else if (val.CompareTo (max) > 0) return max;
			else return val;
		}
	}
}
