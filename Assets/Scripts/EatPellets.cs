using UnityEngine;
using System.Collections;

public class EatPellets : MonoBehaviour {
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Pellet") {
			other.transform.position = new Vector3(Random.Range(-29,30),0,Random.Range(-14,15));
			transform.localScale = new Vector3(transform.localScale.x + 2, transform.localScale.y, transform.localScale.z + 2);
			Movement.speed = (4 * Movement.speed / 5);
		}
	}
}
