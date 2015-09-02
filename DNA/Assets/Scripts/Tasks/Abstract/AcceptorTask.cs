﻿using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public abstract class AcceptorTask : Task {
		public ITaskAcceptor Acceptor { get; set; }
	}
}