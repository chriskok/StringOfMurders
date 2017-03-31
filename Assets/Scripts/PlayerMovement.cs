using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	private float lookSensitivity = 3f;

	private Rigidbody rb;
	private Vector3 velocity;

	public int speed;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {

		//PLAYER MOVEMENT
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov; //transform.right is (1,0,0)
		Vector3 _movVertical = transform.forward * _zMov; //transform.forward is (0,0,1)
		velocity = (_movHorizontal + _movVertical).normalized * speed;//Final movement vector

		rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime);


		//PLAYER ROTATION
		float _yRot = Input.GetAxisRaw("Mouse X");
		Vector3 _rotation = new Vector3 (0f, _yRot, 0f) * lookSensitivity;
		rb.MoveRotation (rb.rotation * Quaternion.Euler(_rotation));


	}


}
