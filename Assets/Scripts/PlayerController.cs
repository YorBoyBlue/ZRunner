using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum PLAYER_STATES { 
    JOG = 0,
    RUN = 1,
    JUMP = 2,
	RUNNING_JUMP = 3,
	SHOOT = 4,
	ROLL = 5
}

public class PlayerController : MonoBehaviour {	

	// States
	PLAYER_STATES m_playerStates;

	// Audio TODO: Seperate some into Audio Script
	//public AudioSource m_jumpSource;
	// public AudioSource m_backgroundSource;
	public AudioSource m_gunShotSource;
	public AudioSource m_reloadSource;
	public AudioClip m_gunShotSound;
	public AudioClip m_reloadSound;

	// Scoring
	public float m_score;
	private ScoreTracker m_scoreScript;

	// Shooting Stuff
	public GameObject m_gun;
	public Image m_outOfAmmoMessage;
    public Transform m_bulletSpawn;
	public Text m_ammoText;
    public GameObject m_bullet;
    public bool m_firedBullet = false;
	private float m_maxBulletSpawnDelay = 0.13f;
	public float m_bulletSpawnDelay = 0.13f;
	private int m_ammo = 5;
	public bool m_shoot = false;
	public bool m_readyToShoot = true;

	// Movement Stuff
	public bool m_jogging = true;
	public bool m_running = false;
	public GameObject m_staminaBar;
	public float m_stamina = 100.0f;
	private float m_maxStamina = 100.0f;
	private Vector3 m_playerSpeed;
	private float m_currentPosX;
	private float m_maxPosX = -43.0f;

	// Roll Stuff
	public bool m_roll = false;
	public bool m_adjustRollPosX = false;
	private float m_rollTimer = 0.8f;
	private float m_maxRollTimer = 0.8f;

	// Jumping Stuff
	public bool m_grounded = true;
	public bool m_jump = false;
	private float m_jumpForce;
	private float m_jumpDelayTimer = 0.5f;
	private float m_maxJumpDelayTimer = 0.5f;
	public bool	m_runningJump = false;
	public bool m_canJump = true;

	// Zombies
	[SerializeField] GameObject m_zombies;

	// Animator
	private Animator m_animator;

	// Rigidbody
	private Rigidbody m_rb;

	void Start() {
		m_score = GetComponent<ScoreTracker>().GetScore();
		m_scoreScript = GetComponent<ScoreTracker>();
		m_playerSpeed = new Vector3(0, 0, 1.5f);
		m_outOfAmmoMessage.gameObject.SetActive(false);
		m_gun.gameObject.SetActive(false);
		m_rb = GetComponent<Rigidbody>();
		m_animator = GetComponent<Animator>();
	}

	void Update() {
		PlayerInput();
		HandleJumping();
		//HandleRolling();
		HandleShooting();
		OutputAmmoCount();
		HandleStamina();
		OutputStamina();
	}

	void FixedUpdate() {
		SetAnimations();
		PlayerMovement();
		if((m_canJump && m_jump) || m_canJump && m_runningJump) {
			Jump();	
		}
	}

	void PlayerMovement() {
		m_currentPosX = transform.position.x;
		if(m_jogging || m_stamina <= 0 || m_jump) {
			transform.Translate((-m_playerSpeed * Time.fixedDeltaTime) * 0.6f);
		} else if (m_running && m_stamina > 0 || m_runningJump && m_stamina > 0) {
			if(m_currentPosX < m_maxPosX) {
				transform.Translate(m_playerSpeed * Time.fixedDeltaTime);
			}
		} 
		if(m_shoot) {
			transform.Translate(0, 0, -0.1f * Time.fixedDeltaTime);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		// Death
		if(other.tag == "Zombie") {
			PlayerPrefs.SetInt("Score", (int)m_scoreScript.GetScore());
			SceneManager.LoadScene("GameOver");
		}
		// Pick-Ups
		if(other.tag == "Ammo") {
			m_scoreScript.AddBonusScore(10);
			m_reloadSource.PlayOneShot(m_reloadSound, 14.0f);
			m_ammo += 3;
			Destroy(other.gameObject);
		}
	}

	void OnCollisionEnter(Collision other) {
		// Death
		if(other.collider.tag == "Zombie") {
			PlayerPrefs.SetInt("Score", (int)m_scoreScript.GetScore());
			SceneManager.LoadScene("GameOver");
		}
		if(other.collider.tag == "Ground" || other.collider.tag == "Environment") {
			m_grounded = true;
			m_runningJump = false;
			m_jump = false;
			m_readyToShoot = false;
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.D)) {
				m_running = true;
				m_playerStates = PLAYER_STATES.RUN;
			} else {
				m_jogging = true;
				m_playerStates = PLAYER_STATES.JOG;
			}
			m_canJump = true;
		}
	}

	void HandleStamina() {
		if(m_running || m_runningJump && !m_jogging || Input.GetKey(KeyCode.LeftShift) && m_stamina <= 0 || Input.GetKey(KeyCode.D) && m_stamina <= 0) {
			m_stamina -= 18f * Time.deltaTime;
		} else if (m_jogging && m_stamina < m_maxStamina || m_jump && m_stamina < m_maxStamina) {
			m_stamina += 14f * Time.deltaTime;
		}
		if(m_stamina < 0) {
			m_stamina = 0;
		} else if(m_stamina > m_maxStamina){
			m_stamina = m_maxStamina;
		}
	}

	void HandleShooting() {
		if(!m_readyToShoot) {
			if(m_bulletSpawnDelay >= 0) {
				m_bulletSpawnDelay -= Time.deltaTime;
			} else {
				m_readyToShoot = true;
				m_shoot = false;
				m_bulletSpawnDelay = m_maxBulletSpawnDelay;
			}
		}
		if(m_ammo > 0) {
			m_outOfAmmoMessage.gameObject.SetActive(false);
			if(m_shoot && m_readyToShoot) {
				if(m_bulletSpawnDelay >= 0) {
					m_bulletSpawnDelay -= Time.deltaTime;
				} else {
					Shoot();
					m_readyToShoot = false;
					m_bulletSpawnDelay = m_maxBulletSpawnDelay;
				}
				if(Input.GetKey(KeyCode.LeftShift)) {
					m_running = true;
				} else {
					m_jogging = true;
				}	
			}
		} else if (m_ammo <= 0) {
			m_outOfAmmoMessage.gameObject.SetActive(true);
			m_ammo = 0;
			m_shoot = false;
			m_readyToShoot = false;
		}
	}

	public void ActivateGun() {
		m_gun.gameObject.SetActive(true);
	}

	public void DeactivateGun() {
		m_gun.gameObject.SetActive(false);
	}

	void HandleJumping() {
		if(m_jump || m_runningJump) {
			m_shoot = false;
			m_roll = false;
			m_jumpDelayTimer -= Time.deltaTime;
			if(m_jumpDelayTimer <= 0) {
				if(m_grounded){
					if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.D)) {
						m_running = true;
						m_playerStates = PLAYER_STATES.RUNNING_JUMP;
					} else {
						m_jogging = true;
						m_playerStates = PLAYER_STATES.JUMP;
					}
				}				
				m_jumpDelayTimer = m_maxJumpDelayTimer;
			}
		}
		if(m_runningJump && m_stamina <= 0 || m_jump && m_stamina <= 0) {
			m_runningJump = false;
			m_jump = false;
		}
	}

	//TODO: WIP
	// void HandleRolling() {
	// 	if(m_roll) {
	// 		//m_rollTimer -= Time.deltaTime;
	// 		if(m_rollTimer <= 0) {
	// 			m_adjustRollPosX = false;
	// 			m_roll = false;
	// 			m_rollTimer = m_maxRollTimer;
	// 		}
	// 	}
	// }

	void OutputAmmoCount() {
		m_ammoText.text = ("AMMO  " + m_ammo);
	}

	void OutputStamina() {
		float currentFillAmount = m_stamina / 100.0f;
		m_staminaBar.GetComponent<Image>().fillAmount = currentFillAmount;
	}

	public void Jump() {
		if(m_jogging) {
			if(!m_roll && !m_shoot && !m_jump && !m_runningJump || m_stamina < 0.1 && !m_jump && !m_runningJump) {
				m_jumpForce = 7f;
				m_runningJump = false;
				m_running = false;
				m_jogging = false;
				m_shoot = false;
				m_jump = true;
			}				
		} else if(m_running) {
			if(!m_roll && !m_shoot && !m_jump && !m_runningJump) {
				m_jumpForce = 7.5f;
				m_jump = false;
				m_running = false;
				m_jogging = false;
				m_shoot = false;
				m_runningJump = true;
			}
		}
		if((m_canJump && m_jump) || m_canJump && m_runningJump) {
			m_animator.SetBool("jump", true);
			m_stamina -= 5f;
			m_grounded = false;
			m_readyToShoot = false;
			m_shoot = false;
			m_canJump = false;	
			m_rb.velocity = new Vector3(0, m_jumpForce, 0);
		}
	}

	public void Shoot() {
		if(!m_running) {
			if(m_readyToShoot && m_grounded && m_ammo > 0) {
				m_gunShotSource.PlayOneShot(m_gunShotSound, 4.0f);
				m_jogging = false;
				m_running = false;
				m_jump = false;
				m_runningJump = false;
				m_shoot = true;
				Instantiate(m_bullet, m_bulletSpawn.position, m_bulletSpawn.rotation);
				m_animator.SetBool("shoot", true);
				m_ammo --;
				m_shoot = false;
			}
		}
	}

	public void Run() {
		if(m_stamina > 0 && m_grounded) {
			m_playerStates = PLAYER_STATES.RUN;
			m_jogging = false;
			m_running = true;
		} else if (m_stamina < 0.1f) {
			m_playerStates = PLAYER_STATES.JOG;
			m_running = false;
			m_jogging = true;
		}
	}

	public void Jog() {
		m_playerStates = PLAYER_STATES.JOG;
		m_running = false;
		m_jogging = true;
	}

	void PlayerInput() {
		// Running
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.D)) {
			if(m_stamina > 0 && m_grounded) {
				m_jogging = false;
				m_running = true;
			}
		} else if (Input.GetKeyUp(KeyCode.LeftShift)  || Input.GetKeyUp(KeyCode.D) || m_stamina < 0.1f) {
			m_running = false;
			m_jogging = true;
		}
		// Jumping
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) {
			if(m_jogging) {
				if(!m_roll && !m_shoot && !m_jump && !m_runningJump || m_stamina < 0.1 && !m_jump && !m_runningJump) {
					m_jumpForce = 7f;
					m_runningJump = false;
					m_running = false;
					m_jogging = false;
					m_shoot = false;
					m_jump = true;
				}				
			} else if(m_running) {
				if(!m_roll && !m_shoot && !m_jump && !m_runningJump) {
					m_jumpForce = 7.5f;
					m_jump = false;
					m_running = false;
					m_jogging = false;
					m_shoot = false;
					m_runningJump = true;
				}
			}
		}
		// Shooting
		if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) {
			if(!m_running) {
				if(m_readyToShoot && m_grounded && m_ammo > 0) {
					m_jogging = false;
					m_running = false;
					m_jump = false;
					m_runningJump = false;
					m_shoot = true;
				}
			}
		}
		//Rolling   // TODO: WIP
		// if(Input.GetKeyDown(KeyCode.S)) {
		// 	m_roll = true;
		// 	m_adjustRollPosX = true;
		// }
	}

	void SetAnimations() {
		// Set the enum PLAYER_STATES
		if(m_jogging && !m_jump && !m_roll && !m_runningJump && !m_shoot && !m_running && m_grounded) {
			m_playerStates = PLAYER_STATES.JOG;
		} else if (m_jogging && m_jump || m_jump && m_grounded) {
			m_playerStates = PLAYER_STATES.JUMP;
		} else if(m_running && !m_runningJump && !m_roll && !m_jump && !m_shoot && !m_jogging) {
			if(m_stamina > 0 && !m_jump || m_stamina > 0 && !m_runningJump && m_grounded) {
				m_playerStates = PLAYER_STATES.RUN;
			} else if (m_stamina <= 0 && !m_jump || m_stamina <= 0 && !m_runningJump && m_grounded) {
				m_playerStates = PLAYER_STATES.JOG;
			}
		} else if(m_running && m_runningJump || m_runningJump || m_runningJump && m_jogging) {
			m_playerStates = PLAYER_STATES.RUNNING_JUMP;
		} else if(m_shoot) {
			m_playerStates = PLAYER_STATES.SHOOT;
		} else if(m_roll && m_jogging || m_roll && m_running) {
			m_playerStates = PLAYER_STATES.ROLL;
		} else if(m_stamina <= 0 && m_jump) {
			m_playerStates = PLAYER_STATES.JUMP;
		} else if(m_stamina <= 0 && m_runningJump) {
			m_playerStates = PLAYER_STATES.RUNNING_JUMP;
		} else if(m_jogging && m_runningJump) {
			m_playerStates = PLAYER_STATES.RUNNING_JUMP;
		}

		// Set the Parameters in the Animator based on the enum PLAYER_STATES
		switch(m_playerStates) {
			case PLAYER_STATES.JOG:
				m_animator.SetBool("jogging", true);
				m_animator.SetBool("running", false);
				m_animator.SetBool("runningJump", false);
				m_animator.SetBool("jump", false);
			break;

			case PLAYER_STATES.RUN:
				m_animator.SetBool("running", true);
				m_animator.SetBool("jump", false);
				m_animator.SetBool("runningJump", false);
				m_animator.SetBool("jogging", false);
			break;

			case PLAYER_STATES.JUMP:
				m_animator.SetBool("jump", true);
				m_animator.SetBool("runningJump", false);
				m_animator.SetBool("running", false);
				m_animator.SetBool("jogging", false);
			break;

			case PLAYER_STATES.RUNNING_JUMP:
				m_animator.SetBool("runningJump", true);
				m_animator.SetBool("jump", false);
				m_animator.SetBool("jogging", false);
				m_animator.SetBool("running", false);
			break;

			case PLAYER_STATES.ROLL:
				m_animator.SetBool("roll", true);
			break;

			case PLAYER_STATES.SHOOT:
				m_animator.SetBool("shoot", true);
				m_animator.SetBool("running", false);
				m_animator.SetBool("jump", false);
				m_animator.SetBool("runningJump", false);
				m_animator.SetBool("jogging", false);
			break;
		}
		if(!m_shoot) {
			m_animator.SetBool("shoot", false);
		}
		if(!m_roll) {
			m_animator.SetBool("roll", false);
		}
	}
}
