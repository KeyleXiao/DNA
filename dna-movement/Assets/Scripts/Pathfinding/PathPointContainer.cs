﻿using UnityEngine;
using System.Collections;

public class PathPointContainer : StaticUnit {

	public Building building;
	public Hospital hospital;

	public DevelopmentPoint devPoint;	// The "blank" PathPoint
	PathPoint newPoint = null;			// The PathPoint that this will develop into after construction
	PathPoint activePoint;				// The current PathPoint (either devPoint or newPoint)

	bool activated = false;
	public bool Activated {
		get { return activated; }
	}

	void Start () {
		activePoint = devPoint;
	}

	public void ArriveAtPoint (MovableUnit u) {
		activePoint.ArriveAtPoint (u);
	}

	public void SetNewPointBuilding () {
		SetNewPoint (building);
	}

	public void SetNewPointHospital () {
		SetNewPoint (hospital);
	}

	public void SetNewPoint (Object obj) {
		if (newPoint != null)
			return;
		newPoint = GameObject.Instantiate (obj, StartPosition, Quaternion.identity) as PathPoint;
		newPoint.MyTransform.SetParent (MyTransform);
		newPoint.Activated = false;
		activated = true;
	}

	public void ActivateDevelopInto () {
		devPoint.Activated = false;
		activePoint = newPoint;
		activePoint.Activated = true;
	}

	protected override void OnSelect () {
		activePoint.Select ();
	}

	protected override void OnUnselect () {
		activePoint.Unselect ();
	}
}