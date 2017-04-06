using UnityEngine;

public class KillVolume : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Platform") {
			Destroy(other.gameObject);
		} else {
			Destroy(other.gameObject);			
		}
	}

	void OnCollisionEnter(Collision other) {
		Destroy(other.gameObject);		
	}
}
