﻿using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class CollectionCenterRenderer : UnitRenderer {
		public override Vector3 Offset { get { return new Vector3 (0, 0.5f, 0); } }
		void Awake () { SetColors (Palette.Tan); }
	}
}