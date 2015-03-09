﻿using UnityEngine;
using System.Collections;

public class ObjectBank : MonoBehaviour {

	public Transform[] objects;
		
	static ObjectBank instance = null;
	public static ObjectBank Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType (typeof (ObjectBank)) as ObjectBank;
			}
			return instance;
		}
	}

	public Transform GetObject (string name) {
		for (int i = 0; i < objects.Length; i ++) {
			Debug.Log (objects[i].name);
			if (objects[i].name == name)
				return objects[i];
		}
		return null;
	}
}
