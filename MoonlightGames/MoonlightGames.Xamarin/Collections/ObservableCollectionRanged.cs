using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

/*
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS 
 * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */

namespace MoonlightGames.Xamarin.Collections
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
		/// Adds the range to the current list of items and sends one notification.
		/// </summary>
		/// <param name="range">Range.</param>
        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Items.Add(item);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Removes all current items and adds the items from the range and sends one notification.
        /// </summary>
        /// <param name="range"></param>
        public void AssumeRange(IEnumerable<T> range)
        {
            Items.Clear();
            AddRange(range);
        }

		/// <summary>
		/// Removes each item in the range, if it is in the current collection, and sends one notification.
		/// </summary>
		/// <param name="range">Range.</param>
        public void RemoveRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
				if(Items.Contains(item))
				{
					Items.Remove(item);
				}
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
