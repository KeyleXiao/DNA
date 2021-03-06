﻿using UnityEngine;
using System.Collections;

public static class CustomMesh {

	public static Mesh hexagon = null;

	public static Mesh CreateMesh (Vector3[] vertices) {
		int[] triangles = new int[vertices.Length];
		for (int i = 0; i < triangles.Length; i ++) {
			triangles[i] = i;
		}
		return CustomMesh.CreateMesh (vertices, triangles);
	}

	public static Mesh CreateMesh (Vector3[] vertices, int[] triangles) {

		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		Vector2[] uvs = new Vector2[vertices.Length];
		for (int i = 0; i < uvs.Length; i ++) {
			uvs[i] = new Vector2 (mesh.vertices[i].x, mesh.vertices[i].z);
		}
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();
		
		return mesh;
	}

	public static Mesh Hexagon (float[] heights) {

		Vector3 center = new Vector3 (0, 0, 0);
		int sideCount = 6;
		float length = 1f;
		Vector2[] points = new Vector2[sideCount];
		float deg = 360f / (float)sideCount;
		for (int i = 0; i < points.Length; i ++) {
			float radians = (float)i * deg * Mathf.Deg2Rad;
			float x = length * Mathf.Sin (radians);
			float y = length * Mathf.Cos (radians);
			points[i] = new Vector2 (x, y);
		}
		
		Mesh hex = CustomMesh.CreateMesh (
			new Vector3[] {
			
			center,
			new Vector3 (points[0].x, heights[0], points[0].y),
			new Vector3 (points[1].x, heights[1], points[1].y),
			
			center,
			new Vector3 (points[1].x, heights[1], points[1].y),
			new Vector3 (points[2].x, heights[2], points[2].y),
			
			center,
			new Vector3 (points[2].x, heights[2], points[2].y),
			new Vector3 (points[3].x, heights[3], points[3].y),
			
			center,
			new Vector3 (points[3].x, heights[3], points[3].y),
			new Vector3 (points[4].x, heights[4], points[4].y),
			
			center,
			new Vector3 (points[4].x, heights[4], points[4].y),
			new Vector3 (points[5].x, heights[5], points[5].y),
			
			center,
			new Vector3 (points[5].x, heights[5], points[5].y),
			new Vector3 (points[0].x, heights[0], points[0].y)
				
		},
		new int[] { 
			0, 1, 2,
			3, 4, 5,
			6, 7, 8,
			9, 10, 11,
			12, 13, 14,
			15, 16, 17
		}
		);
		return hex;
	}

	public static Mesh Hexagon () {

		if (hexagon != null) return hexagon;
		hexagon = CustomMesh.Hexagon (new float[6] {0, 0, 0, 0, 0, 0});
		return hexagon;
	}
	
	public static Mesh Step (float width) {

		float halfWidth = width * 0.5f;
		float length = 1f;

		return CustomMesh.CreateMesh (
			new Vector3[] {
				new Vector3(0, 0, 0),
				new Vector3(halfWidth, 0, length),
				new Vector3(-halfWidth, 0, length)	
			},
			new int[3] { 2, 1, 0 }
		);
	}

	public static Mesh Oil () {
		float half = 0.5f;
		return CustomMesh.CreateMesh (
			new Vector3[] {
				new Vector3(0, 0, half),
				new Vector3(-1, 0, -1),
				new Vector3(1, 0, -1)
			},
			new int[3] { 2, 1, 0 }
		);
	}
}


