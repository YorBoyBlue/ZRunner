using UnityEngine;

public class PlatformDestroyer : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Platform") {
			Destroy(other.gameObject);
		}
	}
}
