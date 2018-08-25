using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour {

	public LayerMask collisionMask;

	public const float skinWidth = 0.02f;
	const float dstBetweenRays = 0.25f;

	[HideInInspector] public int horizontalRayCount;
	[HideInInspector] public int verticalRayCount;

	[HideInInspector] public float horizontalRaySpacing;
	[HideInInspector] public float verticalRaySpacing;

	[HideInInspector] public BoxCollider collider;
	public RaycastPoints raycastOrigins;

	public virtual void Awake() {
		collider = GetComponent<BoxCollider> ();
	}

	public virtual void Start() {
		CalculateRayAlignment ();
	}

	public void UpdateRaycastPoints() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector3 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector3 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector3 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector3 (bounds.max.x, bounds.max.y);
	}

	public void CalculateRayAlignment() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		float boundsWidth = bounds.size.x;
		float boundsHeight = bounds.size.y;

		horizontalRayCount = Mathf.RoundToInt (boundsHeight / dstBetweenRays);
		verticalRayCount = Mathf.RoundToInt (boundsWidth / dstBetweenRays);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	public struct RaycastPoints {
		public Vector3 topLeft, topRight;
		public Vector3 bottomLeft, bottomRight;
	}
}
