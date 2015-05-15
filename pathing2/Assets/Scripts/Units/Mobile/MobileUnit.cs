﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathing;
using GameActions;
using GameInput;

namespace Units {

	public class MobileUnit : Unit, IActionPerformer, IBinder {

		MobileUnitTransform mobileTransform;
		public MobileUnitTransform MobileTransform {
			get {
				if (mobileTransform == null) {
					mobileTransform = UnitTransform as MobileUnitTransform;
				}
				return mobileTransform;
			}
		}

		MobileUnitClickable mobileClickable;
		public MobileUnitClickable MobileClickable {
			get {
				if (mobileClickable == null) {
					mobileClickable = UnitClickable as MobileUnitClickable;
				}
				return mobileClickable;
			}
		}

		Path path = null;
		public Path Path {
			get { 
				if (path == null) {
					IPathable pathable = MobileTransform as IPathable; 
					path = pathable.Path;
				}
				return path;
			}
		}

		public PerformableActions PerformableActions { get; protected set; }
		public IActionAcceptor BoundAcceptor { get; private set; }

		public virtual void OnRelease () {
			if (StartMovingOnPath ()) return;
			BindToCollider ();
		}

		protected void BindToCollider () {
			UnitClickable clickable = MobileClickable.Colliding (1 << (int)InputLayer.StaticUnits);
			if (clickable != null) {
				OnBindActionable (clickable.StaticUnit as IActionAcceptor);
				OnBind ();
			} else {
				OnUnbind ();
			}
		}

		protected virtual void OnBind () {}
		protected virtual void OnUnbind () {}

		public virtual void OnBindActionable (IActionAcceptor acceptor) {
			BoundAcceptor = acceptor;
			ActionHandler.instance.Bind (this);
		}

		public virtual void OnEndActions () {
			StartMovingOnPath ();
		}

		bool StartMovingOnPath () {
			PerformableActions.PairActionsBetweenAcceptors (
				Path.Points.Points.ConvertAll (x => x.StaticUnit as IActionAcceptor));
			return MobileTransform.StartMovingOnPath ();
		}

		public virtual void OnDragRelease (Unit unit) {}
	}
}