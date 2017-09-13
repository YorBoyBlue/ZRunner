using UnityEngine;

public class Bullet : MonoBehaviour {
    private float m_bulletStreamDelay;
	public GameObject m_blood;
    public GameObject m_bulletStream;
    private ScoreTracker m_scoreTracker;

    void Start() {
        m_bulletStreamDelay = 0.05f;
        m_scoreTracker = GameObject.FindGameObjectWithTag("Player").GetComponent<ScoreTracker>();
    }

    void OnCollisionEnter(Collision other) {
       if(other.collider.tag == "Walker") {
           GameObject tmpBlood = Instantiate(m_blood, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
           Destroy(tmpBlood.gameObject, 5f);
           other.gameObject.GetComponent<ZombieWalker>().m_isAlive = false;
           Destroy(this.gameObject);
           m_scoreTracker.AddBonusScore(20);
       }
    }

    void OnTriggerEnter(Collider other) {
       if(other.tag == "Range") {
           Destroy(this.gameObject);
       }
    }

    void Update() {
        if(m_bulletStreamDelay >= 0) {
            m_bulletStreamDelay -= Time.deltaTime;
        } else if(m_bulletStream.activeSelf == false) {
           m_bulletStream.SetActive(true);
        }
    }

    void FixedUpdate() {
        transform.Translate(0, 0, 1);
    }
}