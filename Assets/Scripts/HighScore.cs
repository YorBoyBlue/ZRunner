using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

	public int m_highscore;
	public Text m_highscoreText;

	void Start () {
		m_highscore = PlayerPrefs.GetInt("High Score");
		m_highscoreText.text = "High Score: " + m_highscore;
	}   
}
