using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
[RequireComponent (typeof(Player))]
[RequireComponent (typeof(PlayerInput))]
public class Controller25D : RaycastController {

	public float maxSlopeAngle = 80;

	public CollisionInfo collisionInfo;
	[HideInInspector] public Vector2 playerInput;

	public override void Start() {
		base.Start ();
		collisionInfo.faceDir = 1;

	}

	public void Move(Vector3 moveAmount, bool standingOnPlatform) {
		Move (moveAmount, Vector3.zero, standingOnPlatform);
	}

	public void Move(Vector3 moveAmount, Vector2 input, bool standingOnPlatform = false) {
		UpdateRaycastPoints ();

		collisionInfo.Reset ();
		collisionInfo.moveAmountOld = moveAmount;
		playerInput = input;

		if (moveAmount.y < 0) {
			DescendSlope(ref moveAmount);
		}

		if (moveAmount.x != 0) {
			collisionInfo.faceDir = (int)Mathf.Sign(moveAmount.x);
		}

		HorizontalCollisions (ref moveAmount);
		if (moveAmount.y != 0) {
			VerticalCollisions (ref moveAmount);
		}

		transform.Translate (moveAmount);

		if (standingOnPlatform) {
			collisionInfo.below = true;
		}
	}

	void HorizontalCollisions(ref Vector3 moveAmount) {
		float directionX = collisionInfo.faceDir;
		float rayLength = Mathf.Abs (moveAmount.x) + skinWidth;

		if (Mathf.Abs(moveAmount.x) < skinWidth) {
			rayLength = 2*skinWidth;
		}

		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector3 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector3.up * (horizontalRaySpacing * i);
			RaycastHit hit;
			bool coll = Physics.Raycast(rayOrigin, Vector3.right * directionX, out hit, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector3.right * directionX,Color.red);

			if (coll) {

				if (hit.distance == 0) {
					continue;
				}

				if (hit.collider.tag == "Climbable") {
					collisionInfo.canWallSlide = true;
				}

				float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

				if (i == 0 && slopeAngle <= maxSlopeAngle) {
					if (collisionInfo.descendingSlope) {
						collisionInfo.descendingSlope = false;
						moveAmount = collisionInfo.moveAmountOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisionInfo.slopeAngleOld) {
						distanceToSlopeStart = hit.distance-skinWidth;
						moveAmount.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
					moveAmount.x += distanceToSlopeStart * directionX;
				}

				if (!collisionInfo.climbingSlope || slopeAngle > maxSlopeAngle) {
					moveAmount.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					if (collisionInfo.climbingSlope) {
						moveAmount.y = Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
					}

					collisionInfo.left = directionX == -1;
					collisionInfo.right = directionX == 1;
				}
			}
		}
	}

	void VerticalCollisions(ref Vector3 moveAmount) {
		float directionY = Mathf.Sign (moveAmount.y);
		float rayLength = Mathf.Abs (moveAmount.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i ++) {

			Vector3 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector3.right * (verticalRaySpacing * i + moveAmount.x);
			RaycastHit hit;

			bool coll = Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector3.up * directionY,Color.red);

			if (coll) {
				if (hit.collider.tag == "Through") {
					if (directionY == 1 || hit.distance == 0) {
						continue;
					}
					if (collisionInfo.fallingThroughPlatform) {
						continue;
					}
					if (playerInput.y == -1) {
						collisionInfo.fallingThroughPlatform = true;
						Invoke("ResetFallingThroughPlatform",.5f);
						continue;
					}
				}

				moveAmount.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if (collisionInfo.climbingSlope) {
					moveAmount.x = moveAmount.y / Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
				}

				collisionInfo.below = directionY == -1;
				collisionInfo.above = directionY == 1;
			}
		}

		if (collisionInfo.climbingSlope) {
			float directionX = Mathf.Sign(moveAmount.x);
			rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
			Vector3 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector3.up * moveAmount.y;
			RaycastHit hit;
			bool coll = Physics.Raycast(rayOrigin,Vector3.right * directionX, out hit,rayLength,collisionMask);

			if (coll) {
				float slopeAngle = Vector3.Angle(hit.normal,Vector3.up);
				if (slopeAngle != collisionInfo.slopeAngle) {
					moveAmount.x = (hit.distance - skinWidth) * directionX;
					collisionInfo.slopeAngle = slopeAngle;
					collisionInfo.slopeNormal = hit.normal;
				}
			}
		}
	}

	void ClimbSlope(ref Vector3 moveAmount, float slopeAngle, Vector3 slopeNormal) {
		float moveDistance = Mathf.Abs (moveAmount.x);
		float climbmoveAmountY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (moveAmount.y <= climbmoveAmountY) {
			moveAmount.y = climbmoveAmountY;
			moveAmount.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (moveAmount.x);
			collisionInfo.below = true;
			collisionInfo.climbingSlope = true;
			collisionInfo.slopeAngle = slopeAngle;
			collisionInfo.slopeNormal = slopeNormal;
		}
	}

	void DescendSlope(ref Vector3 moveAmount) {

		RaycastHit maxSlopeHitLeft;
		bool collHitLeft = Physics.Raycast (raycastOrigins.bottomLeft, Vector3.down, out maxSlopeHitLeft, Mathf.Abs (moveAmount.y) + skinWidth, collisionMask);

		RaycastHit maxSlopeHitRight;
		bool collHitRight = Physics.Raycast (raycastOrigins.bottomRight, Vector3.down, out maxSlopeHitRight, Mathf.Abs (moveAmount.y) + skinWidth, collisionMask);

		if (collHitLeft ^ collHitRight) {
			SlideDownMaxSlope (collHitLeft, maxSlopeHitLeft, ref moveAmount);
			SlideDownMaxSlope (collHitRight, maxSlopeHitRight, ref moveAmount);
		}

		if (!collisionInfo.slidingDownMaxSlope) {
			float directionX = Mathf.Sign (moveAmount.x);
			Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
			RaycastHit hit;
			bool coll = Physics.Raycast (rayOrigin, -Vector3.up, out hit, Mathf.Infinity, collisionMask);

			if (coll) {
				float slopeAngle = Vector3.Angle (hit.normal, Vector3.up);
				if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle) {
					if (Mathf.Sign (hit.normal.x) == directionX) {
						if (hit.distance - skinWidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (moveAmount.x)) {
							float moveDistance = Mathf.Abs (moveAmount.x);
							float descendmoveAmountY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
							moveAmount.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (moveAmount.x);
							moveAmount.y -= descendmoveAmountY;

							collisionInfo.slopeAngle = slopeAngle;
							collisionInfo.descendingSlope = true;
							collisionInfo.below = true;
							collisionInfo.slopeNormal = hit.normal;
						}
					}
				}
			}
		}
	}

	void SlideDownMaxSlope(bool col, RaycastHit hit, ref Vector3 moveAmount) {

		if (col) {
			float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
			if (slopeAngle > maxSlopeAngle) {
				moveAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs (moveAmount.y) - hit.distance) / Mathf.Tan (slopeAngle * Mathf.Deg2Rad);

				collisionInfo.slopeAngle = slopeAngle;
				collisionInfo.slidingDownMaxSlope = true;
				collisionInfo.slopeNormal = hit.normal;
			}
		}

	}

	void ResetFallingThroughPlatform() {
		collisionInfo.fallingThroughPlatform = false;
	}

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public bool climbingSlope;
		public bool descendingSlope;
		public bool slidingDownMaxSlope;

		public bool canWallSlide;

		public float slopeAngle, slopeAngleOld;
		public Vector3 slopeNormal;
		public Vector3 moveAmountOld;
		public int faceDir;
		public bool fallingThroughPlatform;


		public void Reset() {
			above = below = false;
			left = right = false;
			climbingSlope = false;
			descendingSlope = false;
			slidingDownMaxSlope = false;
			slopeNormal = Vector3.zero;
			canWallSlide = false;

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}

}