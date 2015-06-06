﻿using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Jacuzzi : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Jacuzzi"; }
		}

		public override bool PathPointEnabled {
			get { return false; }
		}
		
		public PerformableActions PerformableActions { get; private set; }

		HappinessHolder happinessHolder = new HappinessHolder (50, 50);
		HappinessIndicator indicator;

		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (happinessHolder);

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptCollectItem<HappinessHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new GenerateItem<HappinessHolder> ());
		}

		public override void OnPoolCreate () {
			happinessHolder.HolderUpdated += OnHappinessUpdate;
			indicator = ObjectCreator.Instance.Create<HappinessIndicator> ().GetScript<HappinessIndicator> ();
			indicator.Initialize (Transform, 1.5f);
		}

		public override void OnPoolDestroy () {
			happinessHolder.HolderUpdated -= OnHappinessUpdate;
			ObjectCreator.Instance.Destroy<HappinessIndicator> (indicator.MyTransform);
		}

		void OnHappinessUpdate () {
			//TODO: should set indicator as listener on init (and set position & parent) --- basically move all this out of the unit
			indicator.Fill = happinessHolder.PercentFilled;
		}
	}
}