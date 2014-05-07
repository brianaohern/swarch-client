using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {
//	private string u = "Username";
//	private string p = "Password";
	
	private GameProcess process;
	public GUIText username;
	
	public static bool playing;
	private string usernameString = string.Empty;
	private string passwordString = string.Empty;
	public GUIText username;
	public static bool playing;
	private Hashtable hashy = new Hashtable();  //I think having the hashtable in this file is fine instead of it getting its own file

	private Rect windowRect = new Rect(0, 0, Screen.width, Screen.height);

	// Use this for initialization
	void Start () {
		playing = false;
		username.text = string.Empty;
		process = GameObject.Find("GameProcess").GetComponent<GameProcess>();
	}

	void OnGUI()
	{
		if (!playing) 
		{
			GUI.backgroundColor = new Color(0, 0, 0);
			GUI.Window (0, windowRect, windowFunction, "Login");
		}
		if (playing) 
		{
			if ( GUI.Button( new Rect( 100, 200, 100, 20), "Disconnect"))
			{
				process.returnSocket().Disconnect();
				playing = false;
			}
		}
	}

	void windowFunction(int windowID)
	{
		usernameString = GUI.TextField(new Rect (Screen.width / 3, 2 * Screen.height / 5, Screen.width / 3, Screen.height / 9), usernameString, 10);
		passwordString = GUI.PasswordField(new Rect (Screen.width / 3, 2 * Screen.height / 3, Screen.width / 3, Screen.height / 9), passwordString, '*');

		if(hashy.Contains(usernameString))
		{
			if(hashy.ContainsValue(passwordString))//this might not be the function I'm looking for
			{
				//start game
				if (GUI.Button(new Rect (Screen.width / 2, 4 * Screen.height / 5, Screen.width / 9, Screen.height / 12), "Login")) 
				{
					username.text = "Signing in...";
					if ( process.returnSocket().Connect() )
					{
						playing = true;
						username.text = ("Username: " + usernameString);
					}
					else username.text = "Connect Failed";
				}
			}
			else
			{
				//take back to login screen
			}
		}
		else //add the username/password combo to the hashtable
		{
			hashy.Add(usernameString, passwordString);
			if (GUI.Button(new Rect (Screen.width / 2, 4 * Screen.height / 5, Screen.width / 9, Screen.height / 12), "Login")) 
			{
				username.text = ("Username: " + usernameString);
				playing = true;
			}
		}


		GUI.Label(new Rect(Screen.width/3, 35 * Screen.height/100, Screen.width/5, Screen.height/8), "Username");
		GUI.Label(new Rect(Screen.width/3, 62 * Screen.height/100, Screen.width/8, Screen.height/8), "Password");
	}
	
	public void printGui ( string printStr )
	{
		int wordCount = 0 ;
		string[] words = printStr.Split(' ');
		
		printStr = "";
		
		for ( int i = 0 ; i < words.Length ; ++ i )
		{
			if ( wordCount <= 4 )
			{
				printStr += words[i] + " " ;
				wordCount ++ ;
			}
			else
			{
				printStr += words[i] + "\n" ;
				wordCount = 0;
				
			}	
		}
		username.text = printStr ;
	}
}
