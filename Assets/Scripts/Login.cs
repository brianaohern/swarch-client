using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {
//	private string u = "Username";
//	private string p = "Password";

	private string usernameString = string.Empty;
	private string passwordString = string.Empty;
	public GUIText username;
	public static bool playing;

	private Rect windowRect = new Rect(0, 0, Screen.width, Screen.height);

	// Use this for initialization
	void Start () {
		playing = false;
		username.text = string.Empty;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if (!playing) 
		{
			GUI.backgroundColor = new Color(0, 0, 0);
			GUI.Window (0, windowRect, windowFunction, "Login");
		}
	}

	void windowFunction(int windowID)
	{
		usernameString = GUI.TextField(new Rect (Screen.width / 3, 2 * Screen.height / 5, Screen.width / 3, Screen.height / 9), usernameString, 10);
		passwordString = GUI.PasswordField(new Rect (Screen.width / 3, 2 * Screen.height / 3, Screen.width / 3, Screen.height / 9), passwordString, '*');

		if (GUI.Button(new Rect (Screen.width / 2, 4 * Screen.height / 5, Screen.width / 9, Screen.height / 12), "Login")) 
		{
			username.text = ("Username: " + usernameString);
			playing = true;
		}

		GUI.Label(new Rect(Screen.width/3, 35 * Screen.height/100, Screen.width/5, Screen.height/8), "Username");
		GUI.Label(new Rect(Screen.width/3, 62 * Screen.height/100, Screen.width/8, Screen.height/8), "Password");
	}
}
