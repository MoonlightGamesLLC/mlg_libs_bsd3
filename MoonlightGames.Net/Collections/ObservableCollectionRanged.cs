using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
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
		private const string CountName = nameof(Count);
		private const string IndexerName = "Item[]";

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
			OnPropertyChanged(new PropertyChangedEventArgs(CountName));
			OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		/// <summary>
		/// Adds the range to the current list of items and sends one notification.
		/// </summary>
		/// <param name="collection">Range. Throws ArgumentNullException</param>
		public void AddRange(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection));
			}

			CheckReentrancy();

			int startingIndex = Items.Count;
			foreach (T item in collection)
			{
				Items.Add(item);
			}

			var newItems = collection.ToList<T>();
			Debug.Assert(newItems != null, $"Failed to cast {nameof(collection)} to {nameof(List<T>)}");

			OnPropertyChanged(new PropertyChangedEventArgs(CountName));
			OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, startingIndex));
		}

		/// <summary>
		/// Inserts the range and sends one notification.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="collection">Range.</param>
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection));
			}

			if (index < 0 || index > Items.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			CheckReentrancy();

			int count = collection.Count();
			for (int i = 0; i < count; i++)
			{
				Items.Insert(index + i, collection.ElementAt(i));
			}

			var newItems = collection.ToList<T>();
			Debug.Assert(newItems != null, $"Failed to cast {nameof(collection)} to {nameof(List<T>)}");

			OnPropertyChanged(new PropertyChangedEventArgs(CountName));
			OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, index));
		}

		/// <summary>
		/// Inserts the range and sends one notification.
		/// </summary>
		/// <param name="index">Index to start replacing.</param>
		/// <param name="count">Number of items to remove.</param>
		/// <param name="collection">Range.</param>
		public void ReplaceRange(int index, int count, IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection));
			}

            if (index < 0 || index > Items.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if ((index + count) > Items.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			CheckReentrancy();

			//Store the old items
			var oldItems = Items.ToList<T>();
			
            for (int i = 0; i < count; i++)
			{
                Items.RemoveAt(index);
			}

			int collectionCount = collection.Count();
			for (int i = 0; i < collectionCount; i++)
            {
                Items.Insert(i + index, collection.ElementAt(i));
            }

			var newItems = collection.ToList<T>();
			Debug.Assert(newItems != null, $"Failed to cast {nameof(collection)} to {nameof(List<T>)}");

			OnPropertyChanged(new PropertyChangedEventArgs(CountName));
			OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems, index));
		}

		///// <summary>
		///// Removes each item in the range, if it is in the current collection, and sends one notification.
		///// </summary>
		///// <param name="collection">Range. Throws ArgumentNullException</param>
		//public void RemoveRange(IEnumerable<T> collection)
		//{
		//	if (collection == null)
		//	{
		//		throw new ArgumentNullException(nameof(collection));
		//	}

		//	CheckReentrancy();

		//	//Store the items that are found and removed
		//	var changedItems = new List<T>(collection.Count());
		//	foreach (var item in collection)
		//	{
		//		bool rem = Items.Remove(item);

		//		if (rem == true)
		//		{
		//			changedItems.Add(item);
		//		}
		//	}

		//	Debug.Assert(changedItems != null, $"Failed to cast {nameof(collection)} to {nameof(List<T>)}");

		//	OnPropertyChanged(new PropertyChangedEventArgs(CountName));
		//	OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
		//	OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItems, -1));
		//}

		/// <summary>
		/// Replaces all current items with the items from the range and sends one notification.
		/// </summary>
		/// <param name="collection">Range. Clears if null.</param>
		public void AssumeRange(IEnumerable<T> collection)
		{
			CheckReentrancy();

			if (collection != null && collection.Any())
			{
				//Store the old items
				var oldItems = Items.ToList<T>();

				Items.Clear();
				if (collection != null)
				{
					foreach (T current in collection)
					{
						Items.Add(current);
					}
				}

				var newItems = collection.ToList<T>();
				Debug.Assert(newItems != null, $"Failed to cast {nameof(collection)} to {nameof(List<T>)}");

				OnPropertyChanged(new PropertyChangedEventArgs(CountName));
				OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
                try
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems));
                }
                catch(Exception ex)
                {
                    Utils.WriteEx(ex);
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
			}
			else
			{
				Items.Clear();

				OnPropertyChanged(new PropertyChangedEventArgs(CountName));
				OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}
	}
}
