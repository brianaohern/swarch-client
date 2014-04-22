using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPellets : MonoBehaviour {
	
	public GameObject pellet;
	public int pelletCount;
	public List<GameObject> pellets;
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < pelletCount; i++) {
			pellets.Add((GameObject)Instantiate(pellet,new Vector3(Random.Range(-29,30),0,Random.Range(-14,15)),Quaternion.identity));
		}
	}
}
