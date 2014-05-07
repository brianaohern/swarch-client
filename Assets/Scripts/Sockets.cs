using UnityEngine;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System;
using System.Diagnostics;
using System.Threading;

public class Sockets : MonoBehaviour {

	const string SERVER_LOCATION = "255.255.255.255"; const int SERVER_PORT = 4645; //FILL THESE OUT FOR YOUR OWN SERVER
	
	public TcpClient client;

	public NetworkStream nws;
	public StreamWriter sw;
	public int clientNumber;
	public bool connected;
	
	public DateTime dt;
	
	public Thread t = null;
		
	protected static bool threadState = false;
	
	public Queue recvBuffer;
	
	public Sockets()
	{
		connected = false;
		recvBuffer = new Queue();
	}
	
	public bool Connect ()
	{
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
			}
		}
		catch ( Exception ex )
		{
			print ( ex.Message + " : OnConnect");
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
			print ( ex.Message + " : OnDisconnect" );
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
			print ( ex.Message + ": OnTCPPacket" );
		}	
	}
	
	public void endThread(){
		threadState = false;
	}
}