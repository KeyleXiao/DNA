﻿using UnityEngine;
using System.Collections;
using DNA.Paths;

// deprecate

namespace DNA.EventSystem {

	public class ClickContainerEvent : GameEvent {

		public readonly ConnectionContainer Container;
		public readonly Connection Connection;

		public ClickContainerEvent (ConnectionContainer container) {
			Container = container;
			Connection = Container.Connection;
		}
	}
}