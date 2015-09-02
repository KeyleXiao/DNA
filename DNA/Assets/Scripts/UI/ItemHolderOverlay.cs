﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemHolderOverlay : MonoBehaviour, IPoolable {

	public Text itemHolder;

	public string Text {
		set { itemHolder.text = value; }
	}

	public void OnPoolCreate () {}
	public void OnPoolDestroy () {}
}