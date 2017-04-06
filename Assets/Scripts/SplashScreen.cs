using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	private float m_splashTimer = 0.0f;
	private float m_maxSplashTimer = 5.0f;
	private float m_fillAmount;
	public GameObject LoadingBar;

	void Start () {
		LoadingBar.GetComponent<Image>().fillAmount = 0;
	}
	
	void Update () {
		m_splashTimer += Time.deltaTime;
		
		if (m_splashTimer > 0) {			
			float currentFillAmount = m_splashTimer / m_maxSplashTimer;	
			LoadingBar.GetComponent<Image>().fillAmount = currentFillAmount;
		}
		
		if(m_splashTimer >= m_maxSplashTimer) {
			SceneManager.LoadScene("MainMenu");
		}
	}
}
