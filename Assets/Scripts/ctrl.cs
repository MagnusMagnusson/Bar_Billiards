using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ctrl : MonoBehaviour {
	public static ctrl instance;
	public CueBall_Controller balltray;
	public UiController UI;

	private int currentPlayer;
	public int playerCount;
	public bool barTimeInMinutes;
	public float barTime;
	public List<Player> players;
	public List<GameObject> Pins;
	public GameObject endGuard;
	public bool playing;


	private bool valid;
	private bool scored;
	private bool legal;
	public bool endgame;
	// Use this for initialization
	void Start () {
		barTime *= (barTimeInMinutes) ? 60:1;
		barTimeInMinutes = false;
		valid = true;
		scored = false;
		legal = false;
		endgame = false;
		playing = false;

		currentPlayer = 0;
		instance = this;
		players = new List<Player>();	
		for(int i = 0; i < UI.myText.Count; i++)
		{
			UI.myText[i].enabled = false;
		}
		for(int i = 0; i < playerCount; i++)
		{
			players.Add(new Player(i + 1));
			UI.myText[i].enabled = true;
		}
		UI.myText[0].color = Color.green;
		UI.updateScore();
	}

	void Update()
	{
		if (barTime > 0)
		{
			barTime -= Time.deltaTime;	  
			if (barTime <= 0)
			{
				barTime = 0;
				balltray.barUp = false;
			}
			UI.updateTime();
		}
	}
										
	public void potBall(GameObject Ball, int score)
	{							   
		if (valid)
		{
			scored = true;
			players[currentPlayer].addBreak(score * Ball.GetComponent<ballController>().multiplier);
		}
		balltray.potBall(Ball);
		UI.updateScore();
	}
	public void foulBall(GameObject Ball, int score)
	{	
		balltray.foulBall(Ball);
		UI.updateScore();
	}

	public void foul(bool reset)
	{
		//Ends the break
		valid = false;
		scored = false;
		players[currentPlayer].resetBreak();
		if (reset)
		{
			players[currentPlayer].resetScore();
		}
		players[currentPlayer].endBreak();
		UI.updateScore();
	}

	public void startStopTest()
	{
		if (!IsInvoking("stopTest"))
		{
			InvokeRepeating("stopTest", 2, 2);
		}
	}
	private void stopTest()
	{
		if (balltray.shooting)
		{
			return;
		}							   
		var Balls = balltray.ballList();
		bool moving = false;
		foreach (GameObject Ball in Balls)
		{
			if (!Ball.GetComponent<ballController>().isStopped())
			{
				moving = true;
				break;
			}	
		}

		if (!moving)
		{
			CancelInvoke("stopTest");
			if (Balls.Count == 1 && !balltray.barUp && !endgame && balltray.freeRed == 0 && balltray.freeWhite == 0)
			{
				startEndGame();
				return;
			}
			if(Balls.Count == 0 && !balltray.barUp && balltray.freeRed == 0 && balltray.freeWhite == 0)
			{
				if ((valid && legal) || (valid && endgame))
				{
					players[currentPlayer].endBreak();
				}
				else
				{
					players[currentPlayer].resetBreak();
				}
				UI.updateScore();
				game_over();
				return;
			}

			if (valid && (legal || endgame))
			{				  
				if (scored && !endgame)
				{
					scored = false;
					valid = true;
					legal = false;
					Debug.Log("Valid!");
					balltray.nextShot();
					UI.updateScore();
				}
				else
				{
					players[currentPlayer].endBreak();
					nextPlayer();
					scored = false;
					valid = true;
					legal = false;
					Debug.Log("Valid banked!");
					balltray.nextShot();
					UI.updateScore();
				}
			}
			else
			{
				scored = false;
				valid = true;
				legal = false;					 
				players[currentPlayer].resetBreak();
				players[currentPlayer].endBreak();
				nextPlayer();
				reset_pins();
				balltray.nextShot();
				UI.updateScore();
				Debug.Log("Foul!");
			}
		}
	}

	private void nextPlayer()
	{				  
		UI.myText[currentPlayer].color = Color.yellow;
		currentPlayer = (currentPlayer + 1) % playerCount;
		UI.myText[currentPlayer].color = Color.green;
		UI.updateScore();
	}
	public void setLegal()
	{
		legal = true;
	}
	private void reset_pins()
	{
		foreach(GameObject Pin in Pins)
		{
			pegController pinBrain = Pin.GetComponent<pegController>();
			pinBrain.reset();
		}
	}

	public void startEndGame()
	{
		endgame = true;
		endGuard.SetActive(true);
		if (valid && legal)
		{				  
			players[currentPlayer].endBreak();
			scored = false;
			valid = true;
			legal = false;
			Debug.Log("Valid!");
			balltray.nextShot();
			UI.updateScore();
			return;
		}
		else
		{
			scored = false;
			valid = true;
			legal = false;
			players[currentPlayer].resetBreak();
			players[currentPlayer].endBreak();
			nextPlayer(); 
			balltray.nextShot();
			UI.updateScore();
			Debug.Log("Foul!");
		}

		foreach (GameObject Pin in Pins)
		{
			pegController PinBrain = Pin.GetComponent<pegController>();
			if (!PinBrain.PegOfDoom)
			{
				Pin.SetActive(false);
			}
		}

		reset_pins();
	}
	public void game_over()
	{
		currentPlayer = -5;
		foreach(Text T in UI.myText)
		{
			T.color = Color.green;
		}
		UI.updateScore();
		Debug.Log("Game Over");
	}

	public int getCurrentPlayer()
	{
		return currentPlayer;
	}
	public class Player
	{

		private int ID;
		private string name;
		int score;
		int break_score;  
		public string getName()
		{
			return name;
		}	
		public int getID()
		{
			return ID;
		}		  
		public void addBreak(int score)
		{
			break_score += score;
		}	 
		public int getBreak()
		{
			return break_score;
		}	 
		public int getScore()
		{
			return score;
		}	 
		public void resetBreak()
		{
			break_score = 0;
		}		 
		public void endBreak()
		{
			score += break_score;
			resetBreak();
		}
		public void resetScore()
		{
			resetBreak();
			score = 0;
		}
		public Player(int Id, string Name)
		{
			ID = Id;
			score = 0;
			break_score = 0;
			name = Name;
		}
		public Player(int Id,int Score = 0)
		{
			ID = Id;
			score = Score;
			break_score = 0;
			name = "Player" + ID.ToString();
		}
		public Player(int Id, string Name, int Score)
		{
			ID = Id;
			score = Score;
			break_score = 0;
			name = Name;
		}


	}
}
