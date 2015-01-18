﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

namespace GameActions {

	public class MakeMilkshake : Action {

		IceCreamHolder iceCreamHolder;
		MilkHolder milkHolder;
		MilkshakeHolder milkshakeHolder;

		public MakeMilkshake (IceCreamHolder iceCreamHolder, MilkHolder milkHolder, MilkshakeHolder milkshakeHolder, float duration=5) : base ("Make Milkshakes", duration, true, true) {
			this.iceCreamHolder = iceCreamHolder;
			this.milkHolder = milkHolder;
			this.milkshakeHolder = milkshakeHolder;
		}

		public override void End () {
			if (!iceCreamHolder.Empty && !milkHolder.Empty && !milkshakeHolder.Full) {
				iceCreamHolder.Remove ();
				milkHolder.Remove ();
				milkshakeHolder.Add (new MilkshakeItem ());
			}
			base.End ();
		}
	}
}
