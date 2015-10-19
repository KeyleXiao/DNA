﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.Units;

namespace DNA {

	public class EnvironmentSetupManager : MonoBehaviour {

		public PointsManager points;
		public ConnectionsManager connections;

		void Awake () {
			
			points.OnLoadPoints += OnLoadPoints;
			connections.OnLoadConnections += OnLoadConnections;
			points.Init ();
			connections.Init ();
			
		}

		void OnLoadPoints () {

			// Create Giving Tree
			points.SetUnitAtIndex<GivingTreeUnit> (0);
		}

		void OnLoadConnections () {
			
			// Create initial roads
			List<Connection> topConnections = points.GetConnectionsAtIndex (0);
			for (int i = 0; i < topConnections.Count; i ++) {
				ConnectionsManager.GetContainer (topConnections[i]).SetObject<Road> ();
			}
		}
	}
}