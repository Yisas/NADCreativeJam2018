using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = 0.4f;
	float accelerationTimeAirborne = 0.2f;
	float accelerationTimeGrounded = 0.1f;
	public float moveSpeed = 6;

	public Vector3 wallJumpClimb;
	public Vector3 wallJumpOff;
	public Vector3 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = 0.25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	[HideInInspector] public Vector3 velocity;
	float velocityXSmoothing;

	[HideInInspector] public Controller25D controller;

	[HideInInspector] public Vector2 directionalInput;
	[HideInInspector] public bool wallSliding;
	int wallDirX;

	[HideInInspector] public bool isJumping;

	void Start() {
		controller = GetComponent<Controller25D> ();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	void Update() {
		CalculateVelocity ();
		HandleWallSliding ();

		controller.Move (velocity * Time.deltaTime, directionalInput);

		if (controller.collisionInfo.above || controller.collisionInfo.below) {
			if (controller.collisionInfo.slidingDownMaxSlope) {
				velocity.y += controller.collisionInfo.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}

		if (!controller.collisionInfo.below && !controller.collisionInfo.canWallSlide)
			isJumping = true;

		if (isJumping) {
			if (controller.collisionInfo.below )
				isJumping = false;
		}
	}

	public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}

	public void OnJumpInputDown() {

		if (wallSliding) {
			if (wallDirX == directionalInput.x) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if (controller.collisionInfo.below) {
			if (controller.collisionInfo.slidingDownMaxSlope) {
				if (directionalInput.x != -Mathf.Sign (controller.collisionInfo.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisionInfo.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisionInfo.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}


	void HandleWallSliding() {
		wallDirX = (controller.collisionInfo.left) ? -1 : 1;
		wallSliding = false;
		if (controller.collisionInfo.canWallSlide && (controller.collisionInfo.left || controller.collisionInfo.right) && !controller.collisionInfo.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}

	}

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisionInfo.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}
}