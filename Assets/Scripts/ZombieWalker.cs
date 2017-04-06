using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieWalker : MonoBehaviour {

	private Animator m_animator;
	public bool m_isAlive = true;

	void Start() {
		m_animator = GetComponent<Animator>();
	}

	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == "Player") {
			if(m_isAlive) {
				PlayerPrefs.SetInt("Score", (int)other.gameObject.GetComponent<ScoreTracker>().GetScore()); 
				SceneManager.LoadScene("GameOver");
			}
		}
	}

	void Update() {
		if(!m_isAlive) {
			m_animator.SetBool("Death", true);
			GetComponent<Collider>().enabled = false;
			GetComponent<Rigidbody>().isKinematic = true;
		}
	}
}
