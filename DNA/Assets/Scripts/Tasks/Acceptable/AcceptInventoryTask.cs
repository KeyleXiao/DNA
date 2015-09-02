﻿using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class AcceptInventoryTask<T> : AcceptorTask where T : ItemHolder {

		Inventory inventory = null;
		public Inventory Inventory {
			get {
				if (inventory == null && Acceptor is IInventoryHolder) {
					IInventoryHolder holder = Acceptor as IInventoryHolder;
					inventory = holder.Inventory;
				}
				if (inventory == null) {
					throw new System.Exception ("TaskAcceptor does not implement IInventoryHolder");
				}
				return inventory;
			}
		}

		T holder = null;
		protected T Holder {
			get {
				if (holder == null) {
					holder = Inventory.Get<T> ();
				}
				if (holder == null) {
					throw new System.Exception ("Inventory does not include " + typeof (T));
				}
				return holder;
			}
		}
	}
}