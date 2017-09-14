using UnityEngine;

public class PlatformSpawner : MonoBehaviour {
	
	public int m_scene = 1;
	public int m_maxScenes = 3;
	public GameObject[] m_sewerPlatforms;
	public GameObject[] m_brickPlatforms;
	public GameObject[] m_rockPlatforms;
	public int m_platformThemeLimit = 5;
	private int m_maxPlatformThemeLimit = 5;
	private Quaternion m_spawnRotation = Quaternion.identity;
	private Vector3 m_spawnPosition = Vector3.zero;
	public bool m_hasSpawned = false;

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Platform") {
			m_spawnPosition = other.gameObject.transform.position;
			float spawnPosX = m_spawnPosition.x + 39.8f;
			m_spawnPosition.x = spawnPosX;
			m_hasSpawned = false;
			SpawnPlatform(m_spawnPosition, m_scene);
			m_platformThemeLimit -= 1;
			if(m_platformThemeLimit <= 0) {
				m_scene = Random.Range(1, (m_maxScenes + 1));
				m_platformThemeLimit = m_maxPlatformThemeLimit;
			}
		}
	}

	public void SpawnPlatform(Vector3 spawnPos, int scene) {
		int rand = Random.Range(0, m_sewerPlatforms.Length); // TODO: This works if all platform arrays have same amount. Will improve
		switch(scene) {
			case 1:
				if(!m_hasSpawned) {
					Instantiate(m_sewerPlatforms[rand], spawnPos, m_spawnRotation);
					m_hasSpawned = true;
				}		
				break;

			case 2:
				if(!m_hasSpawned) {
					Instantiate(m_brickPlatforms[rand], spawnPos, m_spawnRotation);
					m_hasSpawned = true;
				}
				break;

			case 3:
				if(!m_hasSpawned) {
					Instantiate(m_rockPlatforms[rand], spawnPos, m_spawnRotation);
					m_hasSpawned = true;
				}
				break;

			default:
				break;
		}
	}
}
