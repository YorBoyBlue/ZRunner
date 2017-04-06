using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public AudioSource m_audioSource;
	public AudioSource m_zombieIntroSource;
	public AudioClip m_mainMenuAudio;
	public AudioClip m_zombieIntroAudio;
	public GameObject m_controlsScreen;
	public GameObject m_creditsScreen;

	void Start() {
		m_audioSource.PlayOneShot(m_mainMenuAudio, 0.1f);
		m_zombieIntroSource.PlayOneShot(m_zombieIntroAudio, 0.5f);
	}

	public void PlayGame() {
		SceneManager.LoadScene("Main");
	}

	public void DisplayCredits() {
		m_creditsScreen.SetActive(true);
	}

	public void HideCredits() {
		m_creditsScreen.SetActive(false);
	}

	public void DisplayControls() {
		m_controlsScreen.SetActive(true);
	}

	public void HideControls() {
		m_controlsScreen.SetActive(false);
	}

	public void ExitGame() {
		Application.Quit();
	}
}
