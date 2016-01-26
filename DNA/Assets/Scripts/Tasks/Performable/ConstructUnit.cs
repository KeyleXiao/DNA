﻿using UnityEngine;
using System.Linq;
using System.Collections;
using DNA.Paths;
using DNA.Units;
using InventorySystem;

namespace DNA.Tasks {
	
	public class ConstructUnit : CostTask, IConstructable {

		public PathElementContainer ElementContainer { get; set; }

		public ConstructUnit (Inventory inventory=null) : base (inventory) {}

		public virtual bool CanConstruct (PathElement element) {
			if (element is Connection)
				return false;
			return CanAfford && Settings.ConstructionTargets.Any (((GridPoint)element).Unit.Settings.Symbol.Contains) && element.State == DevelopmentState.Undeveloped;
		}
	}

	public class ConstructUnit<T> : ConstructUnit where T : Unit, IPathElementObject {

		protected override void OnEnd () {
			Purchase ();
			try {
				ElementContainer.BeginConstruction<T> ().LaborCost = TotalCost;
			} catch {
				throw new System.Exception ("The path element container has not been set for the task '" + this + ".' Be sure to call SetConstructionPoint (container)");
			}
			base.OnEnd ();
		}
	}
}