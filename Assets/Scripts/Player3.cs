﻿using UnityEngine;
using System.Collections;

public class Player3 : MonoBehaviour {

	public int points = 0;
	
	public GameProcess process;
	
	private enum Directions { up, down, left, right }
	public int direction;
	
	public float speed = 3; // 5 per second
	
	public GUIText scoreText;

	public string username = "";
	
	// Use this for initialization
	void Start () {
		scoreText.text = "";
		
		this.renderer.enabled = true;
		process = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		
		direction = (int)Directions.down;
	}
	
	// Update is called once per frame
	void Update () {
		if(process.startGame)
		{
			scoreText.text = username + "\n" + points;
			
			if (process.clientNumber == 3)
			{
				if (Input.GetKeyDown(KeyCode.W))
				{
					direction = (int)Directions.up;
					process.SendDirectionChange(direction);
				}
				else if (Input.GetKeyDown(KeyCode.A))
				{
					direction = (int)Directions.left;
					process.SendDirectionChange(direction);
				}
				else if (Input.GetKeyDown(KeyCode.S))
				{
					direction = (int)Directions.down;
					process.SendDirectionChange(direction);
				}
				else if (Input.GetKeyDown(KeyCode.D))
				{
					direction = (int)Directions.right;
					process.SendDirectionChange(direction);
				}
			}
			
			float distance = speed * Time.deltaTime;
			
			if (direction == (int)Directions.up)
			{
				transform.Translate(new Vector3(0,0,distance));
			}
			else if (direction == (int)Directions.down)
			{
				transform.Translate(new Vector3(0,0,-distance));
			}
			else if (direction == (int)Directions.left)
			{
				transform.Translate(new Vector3(-distance,0,0));
			}
			else if (direction == (int)Directions.right)
			{
				transform.Translate(new Vector3(distance,0,0));
			}
			
			if (transform.position.x < -15 || transform.position.x > 15 || transform.position.z < -15 || transform.position.z > 15)
			{
				renderer.enabled = false;
			}
			else if (!renderer.enabled)
			{
				renderer.enabled = true;
			}
		}
	}
}
