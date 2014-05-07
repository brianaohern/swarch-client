using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;

public class ThreadSock : MonoBehaviour 
{
	private NetworkStream nws;
	private StreamReader sr;
	private string buffer;
	private Sockets socks;
	
	//********* COMPLETE THE FOLLOWING CODE
	public ThreadSock (NetworkStream nwsIn, Sockets inSocket)
	{
		nws = nwsIn;
		sr = new StreamReader(nws);
		socks = inSocket;
	}
	//********* COMPLETE THE FOLLOWING CODE
	//********* READ THE STREAM, ADD TO QUEUE, BE THREAD SAFE
	public void Service ()
	{	
		try
		{
			while(true)
			{
				if ((buffer = sr.ReadLine()) != "")
				{
					lock(socks.recvBuffer)
					{
						socks.recvBuffer.Enqueue(buffer);
					}
				}
			}
		}
		catch ( Exception ex )
		{
			print ( ex.Message + " : Thread loop" );
			
		}
		
	}
	
}