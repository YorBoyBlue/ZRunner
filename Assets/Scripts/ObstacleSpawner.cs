using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

	public GameObject[] m_obstacles;
	private Quaternion m_spawnRotation;
	private Vector3 m_spawnPosition;
	private float m_spawnTimerAmount;
	private float m_minSpawnTimer;
	public float m_spawnTimer;

	void Start() {
		m_spawnRotation = Quaternion.Euler(0, -90, 0);
		m_spawnPosition = transform.position;
		m_spawnTimerAmount = 8.0f;
		m_minSpawnTimer = 2.0f;
	}

	void Update() {
		if(m_spawnTimerAmount > m_minSpawnTimer) {
			m_spawnTimerAmount -= 0.01f * Time.deltaTime;
		}
		m_spawnTimer -= Time.deltaTime;
		if(m_spawnTimer <= 0) {
			SpawnObstacle();
			m_spawnTimer = m_spawnTimerAmount;
		}
	}

	void SpawnObstacle() {
		int rand = Random.Range(0, m_obstacles.Length);
		Instantiate(m_obstacles[rand], m_spawnPosition, m_spawnRotation);
		if(m_obstacles[rand].gameObject.tag == null) {
			m_obstacles[rand].gameObject.tag = "Environment";
		}
	}
}
