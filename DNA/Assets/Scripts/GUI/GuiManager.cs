﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;

public class GuiManager : MonoBehaviour {

	public GameObject display;

	void OnEnable () {
		SelectionHandler.onUpdateSelection += OnUpdateSelection;
	}

	void OnDisable () {
		SelectionHandler.onUpdateSelection -= OnUpdateSelection;
	}

	void OnUpdateSelection (List<ISelectable> selectables) {
		//SetDisplayEnabled (selectables.Count > 0);
	}

	void SetDisplayEnabled (bool enabled) {
		display.gameObject.SetActive (enabled);
	}
}
