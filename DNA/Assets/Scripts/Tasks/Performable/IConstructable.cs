﻿using UnityEngine;
using System.Collections;
using DNA.Paths;

namespace DNA.Tasks {

	public interface IConstructable {

		bool CanConstruct (PathElement element);
		void Start ();
	}
}