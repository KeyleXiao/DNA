﻿using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class ConsumeItem<T> : InventoryTask<T> where T : ItemHolder {

		public override bool Enabled {
			get { return !Holder.Empty; }
		}

		protected override void OnEnd () {
			base.OnEnd ();
			Holder.Remove ();
		}
	}
}