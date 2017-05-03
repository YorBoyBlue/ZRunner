using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public AudioSource m_audioSource;
	public AudioSource m_zombieIntroSource;
	public AudioClip m_mainMenuAudio;
	public AudioClip m_zombieIntroAudio;
	public GameObject m_controlsScreen;
	public GameObject m_creditsScreen;
	public Button m_playButton;
	public Button m_controlsButton;
	public Button m_creditsButton;
	public Button m_exitButton;
	public GameObject m_loadingCircle;

	void Start() {
		m_audioSource.PlayOneShot(m_mainMenuAudio, 0.1f);
		m_zombieIntroSource.PlayOneShot(m_zombieIntroAudio, 0.5f);
	}

	public void PlayGame() {
		m_loadingCircle.SetActive(true);
		SceneManager.LoadSceneAsync("Main");
		m_playButton.interactable = false;
		m_controlsButton.interactable = false;
		m_creditsButton.interactable = false;
		m_exitButton.interactable = false;
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
