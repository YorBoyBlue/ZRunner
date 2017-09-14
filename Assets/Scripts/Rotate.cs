using UnityEngine;

public class Rotate : MonoBehaviour {

	private Quaternion m_rotation;
	private Vector3 m_rotationSpeed = new Vector3(0, 150, 0);
	[SerializeField] Rigidbody m_rb;

	void Start() {
		if(m_rb == null) {
			m_rb = GetComponent<Rigidbody>();
		}
	} 
	
	void FixedUpdate () {
		Quaternion deltaRotation = Quaternion.Euler((m_rotationSpeed) * Time.deltaTime);
		m_rb.MoveRotation(m_rb.rotation * deltaRotation);	   
	}
}
