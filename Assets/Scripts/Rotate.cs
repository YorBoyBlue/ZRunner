using UnityEngine;

public class Rotate : MonoBehaviour {

	private Quaternion m_rotation;
	private Vector3 m_rotationSpeed;
	private Rigidbody m_rb;

	void Start() {
		m_rb = GetComponent<Rigidbody>();
		m_rotationSpeed = new Vector3(0, 150, 0);
	} 
	
	void FixedUpdate () {
		Quaternion deltaRotation = Quaternion.Euler((m_rotationSpeed) * Time.deltaTime);
		m_rb.MoveRotation(m_rb.rotation * deltaRotation);	   
	}
}
