﻿using UnityEngine;
using System.Collections;

public class CustomMeshObject : MonoBehaviour {

	MeshFilter mf;
	MeshRenderer mr;
	MeshCollider mc = null;
	
	public Transform myTransform;
	
	public void Init (Mesh mesh, Color color, bool collider = false) {
		if (mc != null) {
			mf.mesh = mesh;
			mc.sharedMesh = mesh;
			SetMaterial(color);
			return;
		}
		myTransform = transform;
		mf = gameObject.AddComponent<MeshFilter>();
		mr = gameObject.AddComponent<MeshRenderer>();
		mf.mesh = mesh;
		if (collider) {
			mc = gameObject.AddComponent<MeshCollider>();
			mc.sharedMesh = mesh;
		}
		SetMaterial(color);
	}

	public void SetMaterial (Color color) {
		mr.SetColor(color);
	}
	
	public void Deactivate () {
		gameObject.SetActive (false);
	}
}
