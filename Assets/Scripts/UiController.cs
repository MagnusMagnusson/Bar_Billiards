using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

	public List<Text> myText;
	public Text Timer;

	public Canvas MainMenu;
	public Canvas PlayerSetup; 

	public List<Canvas> GameUI;

	void Start()
	{
		Time.timeScale = 0;
		ctrl.instance.playing = false;
	}
	public void updateTime()
	{
		float t = ctrl.instance.barTime;
		Timer.text = t.ToString("00") + " seconds left";
	}
	public void updateScore()
	{
		for (int i = 0; i < ctrl.instance.playerCount; i++)
		{
			string name;
			int score, _break;
			bool playing;
			score = ctrl.instance.players[i].getScore();
			_break = ctrl.instance.players[i].getBreak();
			name = ctrl.instance.players[i].getName();
			playing = ctrl.instance.getCurrentPlayer() == i;
			string txt = "";
			txt = name;
			txt += ": ";
			txt += score.ToString("000");
			if (playing)
			{
				txt += " + " + _break.ToString("000");
			}
			myText[i].text = txt;
		}
	}

	public void Button_PlayGame()
	{
		gameSettings settings = gameSettings.get();
		Debug.Log("Cry");
		Invoke("setPlaying", 1);			  
		Time.timeScale = 1;				 
		ctrl.instance.players.Clear();
		PlayerSetup.enabled = false;
		GameUI[0].enabled = true;
		ctrl.instance.playerCount = settings.playerCount;
		for(int i = 0; i < settings.playerTextBox.Count; i++)
		{
			Text t =settings.playerTextBox[i].GetComponentsInChildren<Text>()[1];
			settings.players[i].name = t.text;																		
		}
		for(int i = 0; i < myText.Count; i++)
		{
			string name = settings.players[i].name;
			if(name == "")
			{
				settings.players[i].name = "Player " + (i + 1);
				name = "Player " + (i + 1);
			}
			ctrl.instance.players.Add(new ctrl.Player(i+1, settings.players[i].name));
			myText[i].enabled = i < ctrl.instance.playerCount;
		}
		updateScore();
	}	  
	private void setPlaying()
	{
		ctrl.instance.playing = true;
	}
	public void Button_Quit()
	{

	}
	public void setUpGame()
	{  

	}

}
