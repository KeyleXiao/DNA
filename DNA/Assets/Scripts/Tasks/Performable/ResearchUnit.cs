﻿using UnityEngine;
using System.Collections;
using DNA.Units;

namespace DNA.Tasks {

	public class ResearchUnit<T> : UpgradeTask where T : Unit {

		public override bool Enabled {
			get { return CanAfford && !UpgradeInProgress && !DataManager.GetUnitSettings (typeof (T)).Unlocked; }
		}

		public ResearchUnit () : base () {
			Settings.Duration = TotalCost;
		}

		protected override void OnStart () {
			Purchase ();
			base.OnStart ();
		}

		protected override void OnEnd () {
			DataManager.GetUnitSettings (typeof (T)).Unlocked = true;
			base.OnEnd ();
		}
	}
}