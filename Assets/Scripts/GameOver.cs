using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
	
	// Audio Stuff
	public AudioSource m_screamSource;
	public AudioSource m_bloodSource;
	public AudioSource m_backgroundSource;
	public AudioSource m_zombie1Source;
	public AudioSource m_zombie2Source;
	public AudioSource m_zombie3Source;
	public AudioSource m_zombie4Source;
	public AudioSource m_zombie5Source;
	public AudioSource m_zombie6Source;
	public AudioSource m_zombie7Source;
	public AudioClip m_screamSound;
	public AudioClip m_bloodSound;
	public AudioClip m_backgroundSound;
	public AudioClip m_zombie1Sound;
	public AudioClip m_zombie2Sound;
	public AudioClip m_zombie3Sound;
	public AudioClip m_zombie4Sound;
	public AudioClip m_zombie5Sound;
	public AudioClip m_zombie6Sound;
	public AudioClip m_zombie7Sound;
	private float m_zombieSoundTimer = 6.5f;

	// Blood Stuff
	public GameObject m_player;
	public bool m_spawnBlood = true;
	public GameObject m_blood;
	public Vector3 m_spawnPosition;
	public float m_bloodTimer = 0.5f;
	public float m_maxBloodTimer = 0.5f;

	// Score Stuff
	private int m_score;
	private int m_highScore;
	private float m_displayDelay = 6.4f;
	public Text m_scoreText;
	public Text m_highScoreText;

	// Try Again Screen
	public GameObject m_tryAgainScreen;

	void Start() {
		m_score = PlayerPrefs.GetInt("Score");
		m_highScore = PlayerPrefs.GetInt("High Score");

		if(m_score < m_highScore) {
			m_scoreText.text = "Score: " + m_score;
			m_highScoreText.text = "High Score: " + m_highScore;
		} else {
			m_highScoreText.text = "NEW HIGHSCORE!!";
			m_scoreText.text = "Score: " + m_score;
			PlayerPrefs.SetInt("High Score", m_score);
		}
		m_tryAgainScreen.SetActive(false);
		m_screamSource.PlayOneShot(m_screamSound, 2.0f);
		m_bloodSource.PlayOneShot(m_bloodSound, 6.0f);
		m_backgroundSource.PlayOneShot(m_backgroundSound, 0.01f);
		m_zombie1Source.PlayOneShot(m_zombie1Sound);
		m_zombie2Source.PlayOneShot(m_zombie2Sound);
		m_zombie3Source.PlayOneShot(m_zombie3Sound);
		m_zombie4Source.PlayOneShot(m_zombie4Sound);
		m_zombie5Source.PlayOneShot(m_zombie5Sound);
		m_zombie6Source.PlayOneShot(m_zombie6Sound);
		m_zombie7Source.PlayOneShot(m_zombie7Sound);
	}
	
	void Update() {
		HandleTryAgainScreen();
		HandleAudio();
		if(m_spawnBlood) {
			HandleBlood();
		}
	}

	void HandleAudio() {
		m_zombieSoundTimer -= Time.deltaTime;
		if(m_zombieSoundTimer <= 1.5f) {
			m_spawnBlood = false;
		}
		if(m_zombieSoundTimer <= 0) {
			m_zombie1Source.Stop();
			m_zombie2Source.Stop();
			m_zombie3Source.Stop();
			m_zombie4Source.Stop();
			m_zombie5Source.Stop();
			m_zombie6Source.Stop();
			m_zombie7Source.Stop();
		}
	}

	void HandleBlood() {
		m_bloodTimer -= Time.deltaTime;
		if(m_bloodTimer <= 0) {
			SpawnBlood();
			m_bloodTimer = m_maxBloodTimer;
		}
  	}
	
	void SpawnBlood() {	
		Vector3 playerPosition = m_player.transform.position;
		Vector3 random = new Vector3(Random.Range(-1.0f, 1.0f) - 2, Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

		m_spawnPosition = new Vector3(playerPosition.x += random.x, playerPosition.y + 0.5f, playerPosition.z += random.z);

		Instantiate(m_blood, new Vector3(m_spawnPosition.x, m_spawnPosition.y, m_spawnPosition.z), Quaternion.identity);
	}

	void HandleTryAgainScreen() {
		m_displayDelay -= Time.deltaTime;
		if(m_displayDelay <= 0) {
			m_tryAgainScreen.SetActive(true);
		}
	}

	public void TryAgain() {
		SceneManager.LoadScene("Main");
	}

	public void Exit() {
		SceneManager.LoadScene("MainMenu");
	}
}
