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
					}
					else if (tempBuffer == "player 2")
					{
						clientNumber = 2;
					}
					else if (tempBuffer == "start")
					{
						startGame = true;
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
		this.gui.printGui(printStr );
	}
}