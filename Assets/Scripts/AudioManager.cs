using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager m_singleton = null;
	public AudioClip m_backgroundSound;
	public AudioClip m_gunShotSound;
	public AudioClip m_reloadSound;
	public AudioClip m_zombie1Sound;
	public AudioClip m_zombie2Sound;
	public AudioClip m_zombie3Sound;
	public AudioClip m_zombie4Sound;

	[SerializeField] AudioSource m_audioSource;

	void Awake() {
		if(m_singleton == null) {
			m_singleton = this;
		} else {
			Destroy(this.gameObject);
		}
	}

	void Start() {
		m_audioSource.clip = m_backgroundSound;
		m_audioSource.Play();
		m_audioSource.clip = m_zombie1Sound;
		m_audioSource.Play();
		// m_audioSource.PlayOneShot(m_backgroundSound, 0.5f);
		// m_audioSource.PlayOneShot(m_zombie1Sound);
		m_audioSource.PlayOneShot(m_zombie2Sound);
		m_audioSource.PlayOneShot(m_zombie3Sound);
		m_audioSource.PlayOneShot(m_zombie4Sound);
	}
}
