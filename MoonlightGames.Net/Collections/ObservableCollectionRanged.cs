using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

/*
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS 
 * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */

namespace MoonlightGames.Net.Collections
{
    /// <summary>
    /// Allows adding and removing a range of objects while only
	/// sending out one notification.
    /// </summary>
    public class ObservableCollectionRanged<T> : ObservableCollection<T>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MoonlightGames.Xamarin.Collections.ObservableCollectionRanged`1"/> class.
		/// </summary>
        public ObservableCollectionRanged() : base() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MoonlightGames.Xamarin.Collections.ObservableCollectionRanged`1"/> class.
		/// </summary>
		/// <param name="collection">Collection.</param>
        public ObservableCollectionRanged(IEnumerable<T> collection) : base(collection) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MoonlightGames.Xamarin.Collections.ObservableCollectionRanged`1"/> class.
		/// </summary>
		/// <param name="list">List.</param>
        public ObservableCollectionRanged(List<T> list) : base(list) { }

        /// <summary>
        /// Refresh this collection.
        /// </summary>
		public void Refresh()
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		/// <summary>
		/// Adds the range to the current list of items and sends one notification.
		/// </summary>
		/// <param name="range">Range. Throws ArgumentNullException</param>
		public void AddRange(IEnumerable<T> range)
		{
			if (range == null)
			{
				throw new ArgumentNullException(nameof(range));
			}

			foreach (T item in range)
			{
				Items.Add(item);
			}

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, range));
		}

        /// <summary>
        /// Inserts the range and sends one notification.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="range">Range.</param>
		public void InsertRange(int index, IEnumerable<T> range)
		{
			if (range == null)
			{
				throw new ArgumentNullException(nameof(range));
			}

			if (index < 0 || index > Items.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

            int count = range.Count();
			for (int i = 0; i < count; i++)
			{
                Items.Insert(index + i, range.ElementAt(i));
			}

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, range, index));
		}

		/// <summary>
		/// Removes each item in the range, if it is in the current collection, and sends one notification.
		/// </summary>
		/// <param name="range">Range. Throws ArgumentNullException</param>
		public void RemoveRange(IEnumerable<T> range)
		{
			if (range == null)
			{
				throw new ArgumentNullException(nameof(range));
			}

            //Store the items that are found and removed
            var removedItems = new List<T>(range.Count());
			foreach (var item in range)
			{
				if (Items.Contains(item))
				{
					Items.Remove(item);
                    removedItems.Add(item);
				}
			}

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
		}

		/// <summary>
		/// Replaces all current items with the items from the range and sends one notification.
		/// </summary>
		/// <param name="range">Range. Clears if null.</param>
		public void AssumeRange(IEnumerable<T> range)
        {
            //Store the old items
			T[] oldItems = new T[Items.Count()];
			Items.CopyTo(oldItems, 0);

			Items.Clear();
			if (range != null)
			{
				foreach (T current in range)
				{
					Items.Add(current);
				}
			}

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Items, oldItems));
        }
    }
}
