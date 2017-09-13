using UnityEngine;

public class PlatformMovement : MonoBehaviour {

	private bool m_jump;
	private bool m_runningJump;
	private float m_speed;
	private bool m_grounded;
	private float m_jogSpeed;
	private float m_runningSpeed;
	private GameObject m_player;
	private float m_stamina;

	void Start () {
		m_player = GameObject.FindGameObjectWithTag("Player");
		m_jogSpeed = -3f;
		m_runningSpeed = -5f;
		m_jump = m_player.GetComponent<PlayerController>().m_jump;
		m_runningJump = m_player.GetComponent<PlayerController>().m_runningJump;
		m_stamina = m_player.GetComponent<PlayerController>().m_stamina;
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.D)) {
			if(m_stamina > 0 && m_runningJump) {
				m_speed = m_runningSpeed;
			} else if(m_stamina > 0 && m_jump) {
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
		transform.Translate(m_speed * Time.fixedDeltaTime, 0, 0);
	}
}
