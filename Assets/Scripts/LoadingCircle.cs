using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircle : MonoBehaviour {
	public Image m_loadingCircleGreen;
	public Image m_loadingCircleBlue;
	private float m_fillAmountGreen = 1;
	private float m_fillAmountBlue = 1;

	void Update() {		
		if (m_fillAmountBlue > 0) {		
			m_fillAmountBlue -= Time.deltaTime / 3f;	
			m_loadingCircleBlue.fillAmount = m_fillAmountBlue;
		} else {
			m_fillAmountGreen -= Time.deltaTime / 3f;
			m_loadingCircleGreen.fillAmount = m_fillAmountGreen;
			if(m_fillAmountGreen <= 0) {
				m_fillAmountBlue = 1;
				m_fillAmountGreen = 1;
				m_loadingCircleGreen.fillAmount = m_fillAmountGreen;
				m_loadingCircleBlue.fillAmount = m_fillAmountBlue;
			}
		}
	}	
}
