﻿using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class DrillablePlotRenderer : UnitRenderer {
		public override Vector3 Offset { get { return new Vector3 (0, 0.05f, 0); } }
		void Awake () { SetColors (Color.white); }
	}
}