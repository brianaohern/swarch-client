using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class GameProcess : MonoBehaviour {

	//PUBLIC MEMBERS
	public int clientNumber;
	public bool startGame = false;

	public Player1 p1;
	public Player2 p2;

	public GameObject[] players;
	public GameObject[] pellets;

	//PRIVATE MEMBERS
	private Sockets socks;
	private string buffer;
	private string tempBuffer;
	private Login gui;
		
	void Start () 
	{
		socks = new Sockets();

		// Players
		players = new GameObject[2];

		players[0] = GameObject.Find("Player1");
		players[1] = GameObject.Find("Player2");

		p1 = players[0].GetComponent<Player1>();
		p2 = players[1].GetComponent<Player2>();

		// Pellets
		try
		{
			pellets = GameObject.FindGameObjectsWithTag("Pellet");
		}
		catch (Exception ex)
		{
			print ( ex.Message + ": Exception when filling pellet array");
		}

		// GUI
		gui = GameObject.Find("LoginScreen").GetComponent<Login>();
	}
	
	void Update()
	{
		lock(socks.recvBuffer)
		{
			if(socks.recvBuffer.Count > 0)
			{
				buffer = (string)socks.recvBuffer.Peek();
				tempBuffer = buffer.Split('&')[0];
				
				switch (tempBuffer)
				{
				case "connect": //connect
					tempBuffer = buffer.Split('&')[1];
					if (tempBuffer == "player 1")
					{
						clientNumber = 1;
						players[0].renderer.material.color = new Color(255,0,0);
						players[1].renderer.material.color = new Color(0,0,255);
						Debug.Log("Connected as player 1.");
					}
					else if (tempBuffer == "player 2")
					{
						clientNumber = 2;
						players[0].renderer.material.color = new Color(0,0,255);
						players[1].renderer.material.color = new Color(255,0,0);
						Debug.Log("Connected as player 2.");
					}
					else if (tempBuffer == "start")
					{
						startGame = true;
						Debug.Log("Start message received.");
					}
					break;
				case "login": //login
					Debug.Log("Received login message from server.");
					tempBuffer = buffer.Split('&')[1];
					if (tempBuffer == "fail")
					{
						Debug.Log("Passwords didn't match. Login failed.");
						printGui("Login failed. Try again.");
					}
					else
					{
						Debug.Log("User signing in.");
						printGui("Username: " + tempBuffer);
						Login.playing = true;
					}
					break;
				case "player": // player & <client number> & <x position> & <z position> & <size>

					tempBuffer = buffer.Split('&')[1];

					// Set player position
					players[Convert.ToInt32(tempBuffer) - 1].transform.position =
						new Vector3(Convert.ToSingle(buffer.Split('&')[2]),
						            0,
						            Convert.ToSingle(buffer.Split('&')[3]));

					players[Convert.ToInt32(tempBuffer) - 1].transform.localScale =
						new Vector3(Convert.ToSingle(buffer.Split('&')[4]),
						            Convert.ToSingle(buffer.Split('&')[4]),
						            Convert.ToSingle(buffer.Split('&')[4]));

					if (tempBuffer == "1")
					{
						p1.speed = Convert.ToSingle(buffer.Split('&')[5]);
					}
					else if (tempBuffer == "2")
					{
						p2.speed = Convert.ToSingle(buffer.Split('&')[5]);
					}

					break;

				case "score":
					tempBuffer = buffer.Split('&')[1];
					if (tempBuffer == "1")
					{
						tempBuffer = buffer.Split('&')[2];
						p1.points += Convert.ToInt32(tempBuffer);
					}
					else if (tempBuffer == "2")
					{
						tempBuffer = buffer.Split('&')[2];
						p2.points += Convert.ToInt32(tempBuffer);
					}
					break;

				case "winner":
					tempBuffer = buffer.Split('&')[1];
					if (tempBuffer == "1")
					{
						p1.scoreText.text = "WINNER";
						p2.scoreText.text = "LOSER";
					}
					else if (tempBuffer == "2")
					{
						p1.scoreText.text = "LOSER";
						p2.scoreText.text = "WINNER";
					}
					startGame = false;
					break;

				case "pellet":
					tempBuffer = buffer.Split('&')[1];
					pellets[Convert.ToInt32(tempBuffer)].transform.position =
						new Vector3(Convert.ToSingle(buffer.Split('&')[2]),
						            0,
						            Convert.ToSingle(buffer.Split('&')[3]));
					break;
				}
				socks.recvBuffer.Dequeue();
			}
		}
	}
	
	public Sockets returnSocket()
	{
		return socks;
	}
	
	public void printGui(string printStr)
	{
		this.gui.printGui(printStr);
	}

	public void SendLogin(string username, string password) {
		Debug.Log("In SendLogin function");
		try
		{
			socks.SendTCPPacket("logins&" + username + "&" + password);
			Debug.Log("Player login information sent to server");
		}
		catch (Exception ex)
		{
			print ( ex.Message + ": onSendScore");
		}
		Debug.Log("Sent login information to server.");
	}

	public void SendDirectionChange(int direction) {
		Debug.Log("In SendDirectionChange function");
		try
		{
			socks.SendTCPPacket("direction&" + direction);
			Debug.Log("Player direction change information sent to server");
		}
		catch (Exception ex)
		{
			print ( ex.Message + ": onSendDirectionChange");
		}
		Debug.Log("Sent direction change information to server.");
	}
}