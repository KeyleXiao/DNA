﻿using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class AcceptDeliverItem<T> : AcceptInventoryTask<T> where T : ItemHolder {

		public override bool Enabled {
			get { return !Holder.Full; }
		}	
	}
}