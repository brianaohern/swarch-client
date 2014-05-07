using UnityEngine;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System;
using System.Diagnostics;
using System.Threading;

public class Sockets : MonoBehaviour {

	const string SERVER_LOCATION = "127.0.0.1"; const int SERVER_PORT = 4645; //FILL THESE OUT FOR YOUR OWN SERVER
	
	public TcpClient client;

	public NetworkStream nws;
	public StreamWriter sw;
	public int clientNumber;
	public bool connected;
	
	public DateTime dt;
	
	public Thread t = null;
		
	protected static bool threadState = false;
	
	public Queue recvBuffer;

	GameProcess process;

	public Sockets()
	{
		connected = false;
		recvBuffer = new Queue();
	}
	
	public bool Connect (string username, string password)
	{
		process = GameObject.Find("GameProcess").GetComponent<GameProcess>();

		UnityEngine.Debug.Log("Connecting to server.");
		UnityEngine.Debug.Log("Trying");
		try
		{
			client = new TcpClient(SERVER_LOCATION, SERVER_PORT);
			UnityEngine.Debug.Log("Created client");
			if (client.Connected)
			{
				UnityEngine.Debug.Log("Connected");
				nws = client.GetStream();
				sw = new StreamWriter(nws);
				ThreadSock tsock = new ThreadSock(nws, this);
				t = new Thread(new ThreadStart(tsock.Service));
				t.IsBackground = true;
				t.Start();
				threadState = true;
				sw.AutoFlush = true;

				UnityEngine.Debug.Log("Connected to server. Sending login info.");
				process.SendLogin(username, password);
			}
		}
		catch ( Exception ex )
		{
			UnityEngine.Debug.Log ( ex.Message + " : OnConnect");
		}
		
		if ( client == null ) return false;
		return client.Connected;
	}
	
	public bool Disconnect ()
	{
		try
		{
			nws.Close();
			client.Close();
			endThread();
		}
		catch ( Exception ex )
		{
			UnityEngine.Debug.Log ( ex.Message + " : OnDisconnect");
			return false;
			
		}
		return true;
	}
	
	public void SendTCPPacket ( string toSend )
	{
		try
		{	
			sw.WriteLine(toSend);
		}
		catch ( Exception ex )
		{
			UnityEngine.Debug.Log ( ex.Message + ": OnTCPPacket" );
		}	
	}
	
	public void endThread(){
		threadState = false;
	}
}