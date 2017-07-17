using UnityEngine;
using System.Collections;

public class MurderText : MonoBehaviour
{
	public Camera m_Camera;

	void Update()
	{
		if (m_Camera != null) {
			transform.LookAt (transform.position + m_Camera.transform.rotation * Vector3.forward,
				m_Camera.transform.rotation * Vector3.up);
		}
	}
}