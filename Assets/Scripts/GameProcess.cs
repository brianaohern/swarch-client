using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class GameProcess : MonoBehaviour {

	//PUBLIC MEMBERS
	public int clientNumber;
	public bool startGame = false;

	//PRIVATE MEMBERS
	private Sockets socks;
	private string buffer;
	private string tempBuffer;
	private Login gui;
		
	void Start () 
	{
		socks = new Sockets();
		
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
						Debug.Log("Connected as player 1.");
					}
					else if (tempBuffer == "player 2")
					{
						clientNumber = 2;
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
}