﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Models;
using DNA.Paths;
using DNA.Units;
using InventorySystem;

namespace DNA.Tasks {

	public class ConstructRoad : CostTask, IConstructable {

		public PathElementContainer ElementContainer { get; set; }

		protected override void OnEnd () {
			Purchase ();
			ConnectionContainer c = (ConnectionContainer)ElementContainer;
			ConstructionSite site = c.BeginConstruction<Road> ();
			if (site != null) {
				site.LaborCost = TotalCost;
				site.RoadPlan = new RoadPlan (c.Connection);
			}
			base.OnEnd ();
		}

		public bool CanConstruct (PathElement element) {
			Connection c = (Connection)element;
			return CanAfford 
				&& c.State == DevelopmentState.Undeveloped 
				&& c.Cost > 0 
				&& System.Array.Find (c.Points, x => x.HasRoad) != null;
				// eventually allow this - but need to scrap RoadPlan and have laborers search for valid construction on their own
				// && System.Array.Find (c.Points, x => x.HasRoad || x.HasRoadConstruction) != null;
		}
	}
}