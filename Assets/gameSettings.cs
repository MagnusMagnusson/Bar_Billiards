using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 

public class gameSettings : MonoBehaviour {

	private static gameSettings instance;
	public int playerCount;
	int gameTime;
	public List<PlayerInfo> players;
	public List<GameObject> playerTextBox;
	public GameObject playerCountDropdown;

	// Use this for initialization
	void Start () {
		if (playerTextBox.Count > 0) {
			instance = this;
		}						   
		gameTime = 10;					
		players = new List<PlayerInfo>();
		for(int i = 0; i < 8; i++)
		{
			players.Add(new PlayerInfo("Player " + (1 + i), 0, 0));
		}
		for (int i = 0; i < playerTextBox.Count; i++)
		{
			if (i < 3)
			{
				gameSettings.instance.playerTextBox[i].SetActive(true);
			}
			else
			{
				gameSettings.instance.playerTextBox[i].SetActive(false);
			}
		}
	}

	public void change_player_count(int nn)
	{
		int newPlayerCount = 1 + get().playerCountDropdown.GetComponent<Dropdown>().value;
		if (newPlayerCount <= 0 || newPlayerCount > 8)
		{
			return;
		}
		get().playerCount = newPlayerCount;
		for(int i = 0; i < get().playerTextBox.Count; i++)
		{
			if(i < newPlayerCount)
			{
				get().playerTextBox[i].SetActive(true);
			}
			else
			{
				get().playerTextBox[i].SetActive(false);
			}
		}
	}
	
	public void change_player_name(string IncomeNumber)
	{
		int i = int.Parse(IncomeNumber);																		
		Text t= gameSettings.get().playerTextBox[i].GetComponentsInChildren<Text>()[1];
		get().players[i].name = t.text;
		Debug.Log(i + " wants to get the name " + t.text + " and ended up with name " + get().players[i].name);
	}
	public static gameSettings get()
	{
		return instance;
	}
	public class PlayerInfo
	{
		public string name { get;  set; }
		int difficulty;
		/*
			0 = human
			1 = easy
			2 = medium
			3 = hard
			4 = champion
			5 = randomized
		*/
		int handicap;
		public PlayerInfo()
		{
			name = "Player";
			difficulty = 0;
			handicap = 0;
		}
		public PlayerInfo(string Name, int Type)
		{
			this.name = Name;
			this.difficulty = Type;
			this.handicap = 0;
		}
		public PlayerInfo(string Name, int Type, int Handicap)
		{
			this.name = Name;
			this.difficulty = Type;
			this.handicap = Handicap;
		}
	}
}
