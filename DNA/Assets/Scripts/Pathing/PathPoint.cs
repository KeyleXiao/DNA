﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameActions;
using GameInput;
using Units;

namespace Pathing {

	public class PathPoint : MBRefs, IPoolable, IDraggable {

		public bool Enabled { 
			get { return StaticUnit.PathPointEnabled; }
		}

		public bool MoveOnDrag { get { return false; } }

		public virtual InputLayer[] IgnoreLayers {
			get { return new InputLayer[] { InputLayer.UI }; }
		}

		StaticUnitTransform staticUnitTransform;
		public StaticUnitTransform StaticUnitTransform {
			get { 
				if (staticUnitTransform == null) {
					staticUnitTransform = staticUnit.StaticTransform;
				}
				return staticUnitTransform; 
			}
		}

		StaticUnit staticUnit;
		public StaticUnit StaticUnit {
			get { return staticUnit; }
			set { 
				staticUnit = value; 
				staticUnitTransform = staticUnit.StaticTransform;
			}
		}

		public IActionAcceptor ActionAcceptor {
			get { return staticUnit as IActionAcceptor; }
		}

		public void OnDragEnter (DragSettings dragSettings) {
			PathManager.Instance.EnterPathPoint (dragSettings, this);
		}

		public void OnDragExit (DragSettings dragSettings) {
			PathManager.Instance.ExitPathPoint (dragSettings, this);
		}

		public void OnDrag (DragSettings dragSettings) {}
		public void OnPoolCreate () {}
		public void OnPoolDestroy () {}
	}
}