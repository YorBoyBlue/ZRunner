using UnityEngine;

public class ObstacleMovement : MonoBehaviour {

	private bool m_jump;
	private float m_speed;
	private bool m_grounded;
	private float m_jogSpeed;
	private float m_runningSpeed;
	private float m_stamina;
	private GameObject m_player;
	private Rigidbody m_rb;

	void Start() {
		m_rb = GetComponent<Rigidbody>();
		m_player = GameObject.FindGameObjectWithTag("Player");
		m_jogSpeed = -3f;
		m_runningSpeed = -5f;
		m_jump = m_player.GetComponent<PlayerController>().m_jump;
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.D)) { 
			if(m_stamina > 0 && !m_jump) {
				m_speed = m_runningSpeed;
			} else {
				m_speed = m_jogSpeed;
			}		
		}
	}

	void Update() {
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.D)) {
			m_grounded = m_player.GetComponent<PlayerController>().m_grounded;
			if(m_grounded) {
				m_stamina = m_player.GetComponent<PlayerController>().m_stamina;
				if(m_stamina > 0.5f) {
					m_speed = m_runningSpeed;
				} else {
					m_speed = m_jogSpeed;
				}	
			}		
		} else {
			m_speed = m_jogSpeed;
		}
	}

	void FixedUpdate () {
		transform.Translate(0, 0, -m_speed * Time.fixedDeltaTime);
	}

	void OnCollisionEnter(Collision other) {
		if (other.collider.tag == "Ground") {
			m_rb.constraints = RigidbodyConstraints.FreezePositionY;
			m_rb.freezeRotation = true;
		}
		if(other.collider.tag == "Player") {
			m_speed = 0f;
		}
	}
}
