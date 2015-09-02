﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Rename to "ItemGroup"

namespace GameInventory {

	public delegate void HolderUpdated (); // TODO: rename to OnHolderUpdate
	public delegate void HolderFilled ();  // TODO: rename to OnHolderFull
	public delegate void HolderEmptied (); // TODO: rename to OnHolderEmpty

	[System.Serializable]
	public abstract class ItemHolder : System.Object, INameable {
		
		public abstract HolderUpdated HolderUpdated { get; set; }
		public abstract HolderFilled HolderFilled { get; set; }
		public abstract HolderEmptied HolderEmptied { get; set; }

		public abstract string Name { get; }
		public abstract ItemHolderDisplaySettings DisplaySettings { get; set; }
		public abstract Inventory Inventory { get; set; }

		protected List<Item> items;
		public List<Item> Items {
			get { return items; }
		}

		int capacity;
		public int Capacity {
			get { return capacity; }
			set { capacity = value; }
		}

		public abstract int Count { get; }
		public abstract bool Full { get; }
		public abstract bool Empty { get; }
		public abstract float PercentFilled { get; }

		public List<Item> EmptyList {
			get { return new List<Item> (0); }
		}

		public abstract void Initialize (int count=-1);
		public abstract Item Get (ItemHasAttribute contains);
		public abstract bool Has (ItemHasAttribute contains);
		public abstract List<Item> Add ();
		public abstract List<Item> Add (int amount);
		public abstract List<Item> Add (Item item);
		public abstract List<Item> Add (List<Item> newItems);
		public abstract List<Item> Remove ();
		public abstract List<Item> Remove (int amount);
		public abstract List<Item> Remove (int amount, ItemHasAttribute transferable);
		public abstract void Remove<Item> (Item item);
		public abstract void Clear ();
		public abstract void Transfer (ItemHolder holder, int amount, ItemHasAttribute transferable);
		public abstract void OnTransfer ();
		public abstract void OnHolderUpdated ();
		public abstract void Print ();
	}

	[System.Serializable]
	public class ItemHolder<T> : ItemHolder where T : Item {
		
		public override HolderUpdated HolderUpdated { get; set; }
		public override HolderFilled HolderFilled { get; set; }
		public override HolderEmptied HolderEmptied { get; set; }

		public override string Name {
			get { return ""; }
		}

		ItemHolderDisplaySettings displaySettings = new ItemHolderDisplaySettings (false, true);
		public override ItemHolderDisplaySettings DisplaySettings {
			get { return displaySettings; }
			set { displaySettings = value; }
		}

		public override Inventory Inventory { get; set; }

		new protected List<T> items = new List<T> ();
		new public List<T> Items {
			get { return items; }
		}

		public override int Count {
			get { return items.Count; }
		}

		public override bool Full {
			get { return Count == Capacity; }
		}

		public override bool Empty {
			get { return Count == 0; }
		}

		public override float PercentFilled {
			get { return (float)Count / (float)Capacity; }
		}

		public ItemHolder (int capacity, int startCount) {
			Capacity = capacity;
			Initialize (startCount);
		}

		public override void Initialize (int count=-1) {
			if (count == -1) {
				count = Capacity;
			}
			if (count == 0) {
				Clear ();
			} else {
				for (int i = 0; i < count; i ++) {
					Add ();
				}
			}
		}

		public override Item Get (ItemHasAttribute contains) {
			foreach (Item item in items) {
				if (contains (item)) {
					return item as T;
				}
			}
			return null;
		}

		public override bool Has (ItemHasAttribute contains) {
			foreach (Item item in items) {
				if (contains (item)) {
					return true;
				}
			}
			return false;
		}

		public override List<Item> Add () {
			return Add (new Item () as T);
		}

		public override List<Item> Add (int amount) {
			List<Item> temp = new List<Item> ();
			for (int i = 0; i < amount; i ++) {
				temp.Add (new Item () as T);
			}
			return Add (temp);
		}
		
		public override List<Item> Add (Item item) {
			List<Item> temp = new List<Item> ();
			temp.Add (item);
			return Add (temp);
		}

		public override List<Item> Add (List<Item> newItems) {
			if (newItems.Count > 0) {
				while (Count < Capacity && newItems.Count > 0) {
					Item newItem = newItems[0];
					items.Add (newItem as T);
					if (newItem != null) {
						newItem.Holder = this;
						newItem.OnAdd ();
					}
					newItems.RemoveAt (0);
				}
				NotifyHolderUpdated ();
				if (Full) NotifyHolderFilled ();
			}
			if (newItems.Count > 0) {

				// return items that couldn't be added
				return newItems; 
			} else {
				return EmptyList;
			}
		}

		public override List<Item> Remove () {
			List<Item> removed = Remove (1);
			return removed;
		}

		public override List<Item> Remove (int amount) {
			List<Item> temp = new List<Item> (0);
			while (Count > 0 && amount > 0) {
				temp.Add (items[0]);
				items.RemoveAt (0);
				amount --;
			}
			NotifyHolderUpdated ();
			// return items that were removed
			return temp; 
		}

		public override void Remove<Item> (Item item) {
			items.Remove (item as T);
			NotifyHolderUpdated ();
		}

		public override List<Item> Remove (int amount, ItemHasAttribute transferable) {
			
			if (transferable == null) {
				return Remove (amount);
			}

			List<Item> temp = new List<Item> (0);
			while (Count > 0 && amount > 0) {
				if (transferable (items[0])) {
					temp.Add (items[0]);
				}
				items.RemoveAt (0);
				amount --;
			}

			NotifyHolderUpdated ();
			return temp;
		}

		public override void Clear () {
			items.Clear ();
		}

		public override void Transfer (ItemHolder senderHolder, int amount=-1, ItemHasAttribute transferable=null) {
			if (senderHolder is ItemHolder<T>) {
				if (amount == -1) amount = Capacity;
				ItemHolder<T> sender = senderHolder as ItemHolder<T>;
				List<Item> items = sender.Remove (amount, transferable);
				List<Item> overflow = Add (items);
				sender.Add (overflow);
				OnTransfer ();
			}
		}

		public override void OnTransfer () {}

		protected List<Item> ToItemsList<Y> (List<Y> childItems) where Y : Item {
			List<Item> temp = new List<Item> ();
			foreach (Y item in childItems) {
				temp.Add (item as Item);
			}
			return temp;
		}

		void NotifyHolderUpdated () {
			OnHolderUpdated ();
			if (HolderUpdated != null) {
				HolderUpdated ();
			}
			if (Empty) NotifyHolderEmptied ();
		}

		void NotifyHolderFilled () {
			if (HolderFilled != null) {
				HolderFilled ();
			}
		}

		void NotifyHolderEmptied () {
			if (HolderEmptied != null) {
				HolderEmptied ();
			}
		}

		public override void OnHolderUpdated () {}

		/**
		 *	Debugging
		 */

		public override void Print () {
			foreach (T item in items) {
				Debug.Log (item);
			}
		}
	}
}