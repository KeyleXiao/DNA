﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;

namespace DNA.Paths {

	public class Road : MBRefs, IPoolable {

		Connection connection;
		public Connection Connection {
			get { return connection; }
			set {
				connection = value;
				connection.Road = this;
				SetPoints (connection.Positions[0], connection.Positions[1]);
			}
		}

		Transform roadRender;
		Transform RoadRender {
			get {
				if (roadRender == null) {
					roadRender = MyTransform.GetChild (0);
				}
				return roadRender;
			}
		}

		Renderer roadRenderer = null;
		Renderer RoadRenderer {
			get {
				if (roadRenderer == null) {
					roadRenderer = RoadRender.GetComponent<Renderer> ();
				}
				return roadRenderer;
			}
		}

		bool CanHighlight {
			get { return (!built && SelectionManager.NoneSelected); }
		}

		bool built = false;

		void OnEnable () {
			SetVisible (false);
		}

		public void SetPoints (Vector3 a, Vector3 b) {
			Position = a;
			MyTransform.LookAt (b);
			float distance = Vector3.Distance (a, b);
			RoadRender.SetLocalScaleZ (distance);
			RoadRender.SetLocalPositionZ (distance*0.5f);

			//built = true;
			//SetVisible (true);
		}

		public void OnHoverEnter () {
			if (CanHighlight)
				SetVisible (true);
		}

		public void OnHoverExit () {
			if (CanHighlight)
				SetVisible (false);
		}

		public void OnClick () {
			if (CanHighlight && Player.Instance.Milkshakes.Count >= 5) {
				//Player.Instance.Milkshakes.Remove (5);
				Build ();
			}
		}

		public void Build () {
			built = true;
			SetVisible (true);
			Connection.SetFree ();
		}

		void SetVisible (bool enabled) {
			RoadRenderer.enabled = enabled;	
		}

		public void OnPoolCreate () {}
		public void OnPoolDestroy () {}
	}
}