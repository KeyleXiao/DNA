﻿using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class Wind : Pattern {

		NoisySignal amplitude;
		NoisySignal direction;

		public override float Amplitude {
			get { return 1f; }
		}

		public Wind () {

		}

		public override void Update () {

		}

		public override float ValueAt (float position) {
			return 0f;
		}
	}
}