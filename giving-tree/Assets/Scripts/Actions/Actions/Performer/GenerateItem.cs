﻿using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class GenerateItem<T> : PerformerAction where T : ItemHolder {

		ItemHolder holder = null;
		ItemHolder Holder {
			get {
				if (holder == null) {
					holder = Inventory.Get<T> ();
				}
				if (holder == null) {
					Debug.LogError ("Inventory does not include " + typeof (T));
				}
				return holder;
			}
		}

		public GenerateItem (float duration) : base (duration, true, true) {}
		
		public override void OnEnd () {
			Holder.Add ();
		}
	}
}