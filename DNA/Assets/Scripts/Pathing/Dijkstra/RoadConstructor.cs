﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.EventSystem;
using DNA.Tasks;

// deprecate

namespace DNA.Paths {

	[RequireComponent (typeof (LineDrawer))]
	public class RoadConstructor : MBRefs {

		// TODO: track user's actions instead of making this a singleton (which only calls the Build method)

		static RoadConstructor instance = null;
		static public RoadConstructor Instance {
			get {
				if (instance == null) {
					instance = Object.FindObjectOfType (typeof (RoadConstructor)) as RoadConstructor;
					if (instance == null) {
						GameObject go = new GameObject ("RoadConstructor");
						instance = go.AddComponent<RoadConstructor>();
					}
				}
				return instance;
			}
		}

		public int NewSegmentCount {
			get {
				
				if (Connections == null || Connections.Count == 0)
					return 0;
				
				int count = 0;
				foreach (Connection c in Connections)
					if (c.Cost > 0) count ++;

				return count;
			}
		}

		public int PointCount {
			get { return points.Count; }
		}

		LineDrawer drawer = null;
		LineDrawer Drawer {
			get {
				if (drawer == null) {
					drawer = GetComponent<LineDrawer> ();
				}
				return drawer;
			}
		}

		public List<Connection> Connections { get; private set; }

		public ConstructPrompt prompt;
		readonly List<GridPoint> points = new List<GridPoint> ();
		readonly List<GridPoint> path = new List<GridPoint> ();

		public void AddPoint (GridPoint newPoint) {

			// TODO: this could obv be cleaned up

			if (points.Count == 0) {
				if (!newPoint.HasRoad)
					return;
				points.Add (newPoint);
				ObjectPool.Instantiate ("ConstructionStartIndicator", newPoint.Position);
				return;
			} else if (points.Count == 1) {
				
				if (newPoint == points[0]) {
					Clear ();
					return;
				}

				ObjectPool.Instantiate ("ConstructionEndIndicator", newPoint.Position);
				points.Add (newPoint);
				

			} else if (points.Count == 2) {
				if (newPoint == points[0]) {
					Clear ();
					return;
				}
				if (newPoint == points[1]) {
					points.RemoveAt (1);
					path.Clear ();
					Drawer.Clear ();
					ObjectPool.Destroy ("ConstructionEndIndicator");
					return;
				} else {
					points.RemoveAt (1);
					points.Add (newPoint);
					ObjectPool.Destroy ("ConstructionEndIndicator");
					ObjectPool.Instantiate ("ConstructionEndIndicator", newPoint.Position);
				}
			}

			GenerateShortestPath ();

			// If both points have roads, find shortest route
			// If second point does not have a road, find cheapest route
			/*if (newPoint.HasRoad)
				GenerateShortestPath ();
			else
				GenerateCheapestPath ();*/
		}

		void GenerateShortestPath () {
			path.Clear ();
			for (int i = 0; i < points.Count-1; i ++)
				path.AddRange (Pathfinder.GetPathNoOverlap (points[i], points[i+1]));
			Connections = Pathfinder.PointsToConnections (path);
			Drawer.UpdatePositions (path.ConvertAll (x => x.Position));
		}

		void GenerateCheapestPath () {
			path.Clear ();
			for (int i = 0; i < points.Count-1; i ++)
				path.AddRange (Pathfinder.GetCheapestPath (points[i], points[i+1]));
			Connections = Pathfinder.PointsToConnections (path);
			Drawer.UpdatePositions (path.ConvertAll (x => x.Position));
		}

		public void Clear () {
			path.Clear ();
			points.Clear ();
			Drawer.Clear ();
			ObjectPool.Destroy ("ConstructionStartIndicator");
			ObjectPool.Destroy ("ConstructionEndIndicator");
		}
	}
}