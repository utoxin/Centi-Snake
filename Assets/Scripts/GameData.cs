using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour {
	public static GameData instance;
	public Text highScoreText;
	public Text player1Text;
	public int score;
	public GameObject blockPrefab;

	public int movementStep = 16;
	public int maxCoord = 248;
	public int gamePhase = 1;

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
		score = 0;
		StoreHighscore(0);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (GameObject.FindGameObjectWithTag("HighScoreText") != null) {
			highScoreText = GameObject.FindGameObjectWithTag("HighScoreText").GetComponent<Text>();
			Time.timeScale = 1;
			highScoreText.text = "Hi Score: " + PlayerPrefs.GetInt("highscore").ToString("D9");
		}

		if (GameObject.FindGameObjectWithTag("InGameHiScore") != null) {
			highScoreText = GameObject.FindGameObjectWithTag("InGameHiScore").GetComponent<Text>();
			highScoreText.text = "Hi Score\n" + PlayerPrefs.GetInt("highscore").ToString("D9");
		}

		if (GameObject.FindGameObjectWithTag("Player1Score") != null) {
			player1Text = GameObject.FindGameObjectWithTag("Player1Score").GetComponent<Text>();
			player1Text.text = "Player1\n" + instance.score.ToString("D9");
		}
	}

	public void StoreHighscore(int newHighscore) {
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);
		if (newHighscore > oldHighscore)
		{
			PlayerPrefs.SetInt("highscore", newHighscore);
		}
	}
}
