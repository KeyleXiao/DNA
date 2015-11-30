﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods {
	
	/**
	 *	Float
	 */

	public static float RoundToDecimal (this float fl, int decimalPlaces) {
		float magnitude = 10 * (float)decimalPlaces;
		return Mathf.Round (fl * (magnitude)) / magnitude;
	}

	public static float Map (this float value, float from1, float to1, float from2, float to2) {
	    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	/**
	 *	Vector3
	 */
	 
	public static Vector3 NullPosition = new Vector3 (-1, -1, -1);

	public static bool Equals (this Vector3 vector3, Vector3 otherVector3) {
		return Mathf.Approximately (vector3.x, otherVector3.x) && Mathf.Approximately (vector3.y, otherVector3.y) && Mathf.Approximately (vector3.z, otherVector3.z);
	}

	/**
	 *	Quaternion
	 */

	public static Quaternion ToQuaternion (this Vector3 vector3) {
		return Quaternion.Euler (vector3.x, vector3.y, vector3.z);
	}

	// TODO: this could be made more generic (specify counter/clockwise, set angle axis)
	public static Quaternion SlerpClockwise (this Quaternion from, Quaternion to, float t) {

		Vector3 a = to * Vector3.forward;
		Vector3 b = from * Vector3.forward;

		float delta = Mathf.DeltaAngle (Mathf.Atan2 (a.x, a.z) * Mathf.Rad2Deg, Mathf.Atan2 (b.x, b.z) * Mathf.Rad2Deg);

		// true if turning clockwise
		if (Mathf.Sign (delta) == -1)
			return Quaternion.Slerp (from, to, t);

		if (Mathf.Approximately (delta, 0) && t < 1f) {
			Vector3 e = to.eulerAngles;
			to = Quaternion.Euler (e.x-0.1f, e.y, e.z-0.1f);
		}

		Quaternion mid = Quaternion.Slerp (from, to, 0.5f) * Quaternion.AngleAxis (180f, Vector3.up);
		return (t < 0.5f)
			? Quaternion.Slerp (from, mid, t * 2f)
			: Quaternion.Slerp (mid, to, (t-0.5f) * 2f);
	}

	/**
	 *	LineRenderer
	 */

	public static void SetVertexPositions (this LineRenderer lineRenderer, Vector3[] positions) {
		lineRenderer.SetVertexCount(positions.Length);
		for (int i = 0; i < positions.Length; i ++) {
			lineRenderer.SetPosition (i, positions[i]);
		}
	}

	public static void SetVertexPositions (this LineRenderer lineRenderer, List<Vector3> positions) {
		lineRenderer.SetVertexCount(positions.Count);
		for (int i = 0; i < positions.Count; i ++) {
			lineRenderer.SetPosition (i, positions[i]);
		}
	}

	/**
	 *	Transform
	 */

	public static T GetScript<T> (this Transform transform) where T : class {
		if (transform == null) return null;
		return transform.GetComponent(typeof (T)) as T;
	}

	public static T GetScript<T> (this GameObject gameObject) where T : class {
		if (gameObject == null) return null;
		return gameObject.GetComponent (typeof (T)) as T;
	}

	public static void SetActiveRecursively (this Transform transform, bool active) {
		foreach (Transform child in transform) {
			child.SetActiveRecursively (active);
		}
		transform.gameObject.SetActive (active);
	}

	public static void SetChildrenActive (this Transform transform, bool active) {
		foreach (Transform child in transform) {
			child.SetActiveRecursively (active);
		}
	}

	public static List<Transform> GetChildren (this Transform transform) {
		List<Transform> children = new List<Transform> ();
		foreach (Transform child in transform) {
			children.Add (child);
		}
		return children;
	}

	// TODO: rename to GetChildrenRecursively
	public static List<Transform> GetAllChildren (this Transform transform) {
		List<Transform> children = new List<Transform> ();
		foreach (Transform child in transform) {
			children.Add (child);
			children.AddRange (child.GetAllChildren ());
		}
		return children;
	}

	public static Transform GetNthParent (this Transform transform, int n) {
		Transform parent = transform.parent;
		if (parent == null) {
			return null;
		}
		if (n == 0) {
			return parent;
		}

		int nCount = 0; // 0 is the first parent
		while (parent.parent != null && nCount < n) {
			parent = parent.parent;
			nCount ++;
		}
		return parent;
	}

	public static T GetParentOfType<T> (this Transform transform) where T : MonoBehaviour {
		Transform parent = transform.parent;
		if (parent == null) {
			return null;
		}
		T t = parent.GetScript<T> ();
		if (t != null) {
			return t;
		} else {
			return parent.GetParentOfType<T> ();
		}
	}

	public static Transform GetFirstParent (this Transform transform) {
		Transform parent = transform.parent;
		if (parent == null) {
			return null;
		}
		while (parent.parent != null) {
			parent = parent.parent;
		}
		return parent;
	}

	public static void Reset (this Transform transform) {
		transform.localPosition = Vector3.zero;
		transform.localEulerAngles = new Vector3 (0, 0, 0);
		transform.localScale = new Vector3 (1, 1, 1);
	}

	// Position
	public static void SetPosition (this Transform transform) {
		transform.position = Vector3.zero;
	}
	
	public static void SetPosition (this Transform transform, Vector3 position) {
		transform.position = position;
	}
	
	public static void SetLocalPosition (this Transform transform, Vector3 position) {
		transform.localPosition = position;
	}

	public static void SetPositionX (this Transform transform, float x) {
		Vector3 p = transform.position;
		p.x = x;
		transform.position = p;
	}

	public static void SetPositionY (this Transform transform, float y) {
		Vector3 p = transform.position;
		p.y = y;
		transform.position = p;
	}

	public static void SetPositionZ (this Transform transform, float z) {
		Vector3 p = transform.position;
		p.z = z;
		transform.position = p;
	}

	public static void SetLocalPositionX (this Transform transform, float x) {
		Vector3 p = transform.localPosition;
		p.x = x;
		transform.localPosition = p;
	}
	
	public static void SetLocalPositionY (this Transform transform, float y) {
		Vector3 p = transform.localPosition;
		p.y = y;
		transform.localPosition = p;
	}
	
	public static void SetLocalPositionZ (this Transform transform, float z) {
		Vector3 p = transform.localPosition;
		p.z = z;
		transform.localPosition = p;
	}

	// Rotation
	public static void SetLocalEulerAnglesX (this Transform transform, float x) {
		Vector3 r = transform.localEulerAngles;
		r.x = x;
		transform.localEulerAngles = r;
	}

	public static void SetLocalEulerAnglesY (this Transform transform, float y) {
		Vector3 r = transform.localEulerAngles;
		r.y = y;
		transform.localEulerAngles = r;
	}

	public static void SetLocalEulerAnglesZ (this Transform transform, float z) {
		Vector3 r = transform.localEulerAngles;
		r.z = z;
		transform.localEulerAngles = r;
	}

	// Scale
	public static void SetLocalScaleX (this Transform transform, float x) {
		Vector3 p = transform.localScale;
		p.x = x;
		transform.localScale = p;
	}

	public static void SetLocalScaleY (this Transform transform, float y) {
		Vector3 p = transform.localScale;
		p.y = y;
		transform.localScale = p;
	}

	public static void SetLocalScaleZ (this Transform transform, float z) {
		Vector3 p = transform.localScale;
		p.z = z;
		transform.localScale = p;
	}

	public static void SetLocalScale (this Transform transform, float scale) {
		transform.localScale = new Vector3 (scale, scale, scale);
	}

	/**
	 *	Colliders
	 */

	public static void SetCenterY (this BoxCollider collider, float size) {
		Vector3 colliderCenter = collider.center;
		colliderCenter.y = size;
		collider.center = colliderCenter;
	}

	public static void SetSizeY (this BoxCollider collider, float size) {
		Vector3 colliderSize = collider.size;
		colliderSize.y = size;
		collider.size = colliderSize;
	}

	/**
	 *	EventSystems
	 */

	public static bool LeftClicked (this PointerEventData e) {
		return e.button == PointerEventData.InputButton.Left;
	}
}
