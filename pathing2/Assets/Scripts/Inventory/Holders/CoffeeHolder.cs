﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {
	
	public class CoffeeHolder : ItemHolder<CoffeeItem> {

		public override string Name {
			get { return "Coffee"; }
		}

		public CoffeeHolder (int capacity, int startCount) : base (capacity, startCount) {}
	}
}