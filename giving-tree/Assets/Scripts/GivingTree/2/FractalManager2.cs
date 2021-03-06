﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FractalManager2 : MonoBehaviour {

	public Transform cameraAnchor;
	public Iteration iteration;
	public Transform givingTree;

	int maxIterations = 4;
	List<Iteration> iterations = new List<Iteration> ();

	Iteration PreviousIteration {
		get { return iterations[iterations.Count-3]; }
	}

	Iteration CurrentIteration {
		get { return iterations[iterations.Count-2]; }
	}

	Iteration NextIteration {
		get { return iterations[iterations.Count-1]; }
	}

	void Awake () {
		AddIteration (CreateInitialTree ());
		for (int i = 0; i < maxIterations-1; i ++) {
			AddIteration (NextIteration.TargetTree);
		}
		//PreviousIteration.DeactivateUntargeted ();
		SetCameraTarget ();
	}
	
	GivingTree2 CreateInitialTree () {
		Transform t = Instantiate (givingTree) as Transform;
		t.SetParent (transform);
		GivingTree2 tree = t.GetScript<GivingTree2> ();
		return tree;
	}

	void AddIteration (GivingTree2 tree, Iteration newIteration=null) {
		if (newIteration == null) {
			newIteration = Instantiate (iteration) as Iteration;
		}
		newIteration.name = string.Format ("Iteration {0}", iterations.Count);
		iterations.Add (newIteration);
		NextIteration.Create (tree);
		NextIteration.ThisTree.name = string.Format ("Tree {0}", iterations.Count);
	}

	void Iterate (GivingTree2 tree) {
		iterations[1].transform.SetParent (transform);
		Iteration next = iterations[0];
		iterations.RemoveAt (0);
		AddIteration (iterations[2].ThisTree, next);
		//iterations.Add (next);
	}

	void SetCameraTarget () {
		cameraAnchor.SetParent (CurrentIteration.TargetTree.transform);
		StartCoroutine (MoveCameraToTarget ());
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Iterate (NextIteration.TargetTree);
			SetCameraTarget ();
			StartCoroutine (ExpandInitialTree ());
		}
	}

	IEnumerator MoveCameraToTarget () {
		
		float time = 1;
		float eTime = 0;
		Vector3 startPosition = cameraAnchor.localPosition;
		Vector3 startScale = cameraAnchor.localScale;
		Vector3 endScale = new Vector3 (10, 10, 10);

		while (eTime < time) {
			eTime += Time.deltaTime;
			cameraAnchor.localPosition = Vector3.Lerp (
				startPosition, 
				Vector3.zero, 
				Mathf.SmoothStep (0, 1, eTime / time)
			);
			cameraAnchor.localScale = Vector3.Lerp (
				startScale,
				endScale,
				Mathf.SmoothStep (0, 1, eTime / time)
			);
			yield return null;
		}
	}

	IEnumerator ExpandInitialTree () {

		float time = 1;
		float eTime = 0;
		Transform treeTransform = iterations[iterations.Count-4].ThisTree.transform;
		Vector3 startScale = treeTransform.localScale;
		Vector3 endScale = startScale * 10;

		while (eTime < time) {
			eTime += Time.deltaTime;
			treeTransform.localScale = Vector3.Lerp (
				startScale,
				endScale,
				Mathf.SmoothStep (0, 1, eTime / time)
			);
			yield return null;
		}
	}
}
