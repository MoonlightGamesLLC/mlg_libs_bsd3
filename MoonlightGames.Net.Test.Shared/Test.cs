using NUnit.Framework;
using System;
using MoonlightGames.Net.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MoonlightGames.Net.Test.Shared
{
    [TestFixture()]
    public class Test
    {
        readonly List<string> names1;
        readonly List<string> names2;

        public Test()
        {
            names1 = new List<string> { "jim", "rob", "ralph" };
            names2 = new List<string> { "john", "rork", "logan", "paula" };
        }

        [Test()]
        public void TestCaseAdd()
        {
			var ocr = new ObservableCollectionRanged<string>();

            ocr.CollectionChanged += (sender, e) => 
            {
				Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Add);
				Assert.IsNotNull(e.NewItems);
				Assert.IsNull(e.OldItems);
				Assert.IsTrue(e.NewItems.Count == names1.Count);
                Assert.IsTrue(e.NewStartingIndex == 0);
            };

            ocr.AddRange(names1);
        }

		[Test()]
		public void TestCaseAddGrouped()
		{
			var ocr = new ObservableCollectionRanged<IGrouping<int, string>>();
			var groupedList = names1.GroupBy(s => s.Length);

			ocr.CollectionChanged += (sender, e) =>
			{
				Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Add);
				Assert.IsNotNull(e.NewItems);
				Assert.IsNull(e.OldItems);
				Assert.IsTrue(e.NewItems.Count == groupedList.Count());
				Assert.IsTrue(e.NewStartingIndex == 0);
			};

			ocr.AddRange(groupedList);
		}

		[Test()]
		public void TestCaseAddEnd()
		{
			var ocr = new ObservableCollectionRanged<string>();
            ocr.Add("test1");

			ocr.CollectionChanged += (sender, e) =>
			{
				Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Add);
				Assert.IsNotNull(e.NewItems);
				Assert.IsNull(e.OldItems);
				Assert.IsTrue(e.NewItems.Count == 1);
				Assert.IsTrue(e.NewStartingIndex == 1);
			};

            ocr.Add("test2");
		}

		[Test()]
		public void TestCaseInsert()
		{
			var ocr = new ObservableCollectionRanged<string>();

            int index = 0;

            ocr.AddRange(names1);

			ocr.CollectionChanged += (sender, e) =>
			{
                Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Add);
				Assert.IsNotNull(e.NewItems);
                Assert.IsNull(e.OldItems);
				Assert.IsTrue(e.NewItems.Count == names2.Count);
                Assert.AreEqual(index, e.NewStartingIndex);
			};

            ocr.InsertRange(index, names2);
		}

		[Test()]
		public void TestCaseInsertGrouped()
		{
			var ocr = new ObservableCollectionRanged<IGrouping<int, string>>();

			int index = 0;

			var groupedList1 = names1.GroupBy(s => s.Length);
			ocr.AddRange(groupedList1);

			var groupedList2 = names2.GroupBy(s => s.Length);

			ocr.CollectionChanged += (sender, e) =>
			{
				Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Add);
				Assert.IsNotNull(e.NewItems);
				Assert.IsNull(e.OldItems);
				Assert.IsTrue(e.NewItems.Count == groupedList2.Count());
				Assert.AreEqual(index, e.NewStartingIndex);
			};

			ocr.InsertRange(index, groupedList2);
		}

		[Test()]
		public void TestCaseAssume()
		{
			var ocr = new ObservableCollectionRanged<string>();

			ocr.AddRange(names1);
			ocr.AddRange(names2);

			ocr.CollectionChanged += (sender, e) =>
			{
                Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Replace);
				Assert.IsNotNull(e.NewItems);
				Assert.IsNotNull(e.OldItems);
                Assert.IsTrue(e.OldItems.Count == (names1.Count + names2.Count));
                Assert.IsTrue(e.NewItems.Count == names1.Count);
			};

            ocr.AssumeRange(names1);
		}

		[Test()]
		public void TestCaseAssumeGrouped()
		{
			var ocr = new ObservableCollectionRanged<IGrouping<int, string>>();

			var groupedList1 = names1.GroupBy(s => s.Length);
			ocr.AddRange(groupedList1);

			var groupedList2 = names2.GroupBy(s => s.Length);
			ocr.AddRange(groupedList2);

			ocr.CollectionChanged += (sender, e) =>
			{
				Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Replace);
				Assert.IsNotNull(e.NewItems);
				Assert.IsNotNull(e.OldItems);
				Assert.IsTrue(e.OldItems.Count == (groupedList1.Count() + groupedList2.Count()));
				Assert.IsTrue(e.NewItems.Count == groupedList1.Count());
			};

			ocr.AssumeRange(groupedList1);
		}

		[Test()]
		public void TestCaseAssumeNull()
		{
			var ocr = new ObservableCollectionRanged<string>();

			ocr.AddRange(names1);

			ocr.CollectionChanged += (sender, e) =>
			{
                Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Reset);
				Assert.IsNull(e.NewItems);
				Assert.IsNull(e.OldItems);
			};

			ocr.AssumeRange(null);
		}

		[Test()]
		public void TestCaseAddNull()
		{
			var ocr = new ObservableCollectionRanged<string>();

			ocr.CollectionChanged += (sender, e) =>
			{
				Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Add);
				Assert.IsNull(e.NewItems);
				Assert.IsNull(e.OldItems);
			};

            Assert.Throws<ArgumentNullException>(()=>ocr.AddRange(null));
		}
    }
}
