using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private float inputDirection;             // X value of our MoveVector
	private float verticalVelocity;           // Y vaule of our MoveVector

	private float speed = 5.0f;
	private float gravity = 30.0f;
	private float jumpForce = 10.0f;
	private bool secondJumpAvail = false;

	private Vector3 moveVector;
	private Vector3 lastMotion;
	private CharacterController controller;

	void Start () {
		controller = GetComponent<CharacterController> ();

	}

	void Update () {
		IsControllerGrounded ();
		moveVector = Vector3.zero;
		inputDirection = Input.GetAxis("Horizontal") * speed;



		if (IsControllerGrounded()) 
		{
			verticalVelocity = 0;

			if (Input.GetKeyDown (KeyCode.W)) 
			{
				//player jump
				verticalVelocity = jumpForce;
				secondJumpAvail = true;
			}

			moveVector.x = inputDirection;
		} 
		else 
		{
			if (Input.GetKeyDown (KeyCode.W)) 
			{
				if (secondJumpAvail) 
				{
					verticalVelocity = jumpForce;
					secondJumpAvail = false;
				}
			}

			verticalVelocity -= gravity * Time.deltaTime;
			moveVector.x = lastMotion.x;
		}
			
		moveVector.y = verticalVelocity;

		controller.Move (moveVector * Time.deltaTime);
		lastMotion = moveVector;
	}

	private bool IsControllerGrounded()
	{
		Vector3 leftRayStart;
		Vector3 rightRayStart;

		leftRayStart = controller.bounds.center;
		rightRayStart = controller.bounds.center;	

		leftRayStart.x -= controller.bounds.extents.x;
		rightRayStart.x += controller.bounds.extents.x;

		Debug.DrawRay (leftRayStart,Vector3.down,Color.red);
		Debug.DrawRay (rightRayStart,Vector3.down,Color.green);

		if (Physics.Raycast (leftRayStart, Vector3.down, (controller.height / 2) + 0.1f))
			return true;
		if (Physics.Raycast (rightRayStart, Vector3.down, (controller.height / 2) + 0.1f))
			return true;

		return false;
	}


	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (controller.collisionFlags == CollisionFlags.Sides) 
		{
			if (Input.GetKeyDown (KeyCode.W)) 
			{
				Debug.DrawRay (hit.point, hit.normal, Color.red, 2.0f);
				moveVector = hit.normal * speed;
				verticalVelocity = jumpForce;
				secondJumpAvail = true;
			}
				
		}

		// Collectables
		switch (hit.gameObject.tag) 
		{
		case "Coin":
			LevelManager.Instance.CollectCoin ();
			Destroy (hit.gameObject);
			break;
		case "JumpPad":
			verticalVelocity = jumpForce * 2;
			break;
		case "Teleport":
			transform.position = hit.transform.GetChild (0).position;
			break;
		case "WinBox":
			LevelManager.Instance.Win ();
			Destroy (hit.gameObject);
			break;
		default:
			break;
		}
    }
}
