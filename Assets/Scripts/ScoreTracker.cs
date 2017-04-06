using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour {

private float m_score;
public Text m_textBox;
	
	void Update () {
		m_score += Time.deltaTime;
		OutputScore();
	}

	public int GetScore() {
		return ((int)Mathf.Floor(m_score));
	}

	public void AddBonusScore(int score) {
		m_score += score;
	}

	private void OutputScore() {
		m_textBox.text = RepeatString("0", GetScore().ToString(), 8);
	}

	private string RepeatString(string repeat, string text, int length) {
        string newString = "";
        for (int i = 0; i < length-text.Length; i++) {
            newString += repeat;
        }
        newString += text;        
        return newString;
    }
}

