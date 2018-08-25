using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

	public Animator Anim;

	Player player;
	Transform trans;

	void Start () {
		player = GetComponent<Player> ();
		trans = GetComponent<Transform> ();
	}

	void Update() {

		trans.localScale = new Vector3 (player.controller.collisionInfo.faceDir, 1, 1);

		if(!player.wallSliding && !player.controller.collisionInfo.left && !player.controller.collisionInfo.right)
			Anim.SetFloat ("Speed", Mathf.Abs(player.directionalInput.x));
		else
			Anim.SetFloat ("Speed", 0);

		Anim.SetBool ("Jump", player.isJumping);

		Anim.SetBool ("Wall Slide", player.wallSliding);
	}
}


