using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	private enum Directions { up, down, left, right }
	private int direction;
	private int xMove;
	private int zMove;
	
	public static float speed = 5;
	
	// Use this for initialization
	void Start () {
		direction = (int)Directions.down;
	}
	
	// Update is called once per frame
	void Update () {
		if(Login.playing)
		{
			if (Input.GetKeyDown(KeyCode.W))
				direction = (int)Directions.up;
			else if (Input.GetKeyDown(KeyCode.A))
				direction = (int)Directions.left;
			else if (Input.GetKeyDown(KeyCode.S))
				direction = (int)Directions.down;
			else if (Input.GetKeyDown(KeyCode.D))
				direction = (int)Directions.right;
			
			if (direction == (int)Directions.up) {
				xMove = 0;
				zMove = 1;	
			} else if (direction == (int)Directions.down) {
				xMove = 0;
				zMove = -1;	
			} else if (direction == (int)Directions.left) {
				xMove = -1;
				zMove = 0;	
			} else {
				xMove = 1;
				zMove = 0;
			}
			
			transform.Translate(xMove*Time.deltaTime*speed, 0, zMove*Time.deltaTime*speed);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Wall") {
			transform.position = new Vector3 (Random.Range(-29,30), 0, Random.Range(-14,15));
			transform.localScale = new Vector3(1,1,1);
			speed = 5;
		}
	}
}
