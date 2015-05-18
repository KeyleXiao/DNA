﻿using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptCollectItemEnabledState<T> : EnabledState {

		public override bool Enabled {
			get { return !holder.Empty; }
		}

		string requiredPair = "";
		public override string RequiredPair {
			get { 
				if (requiredPair == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					requiredPair = "Deliver" + typeName;
				}
				return requiredPair;
			}
		}

		ItemHolder holder;

		public AcceptCollectItemEnabledState (ItemHolder holder) {
			this.holder = holder;
		}
	}
}