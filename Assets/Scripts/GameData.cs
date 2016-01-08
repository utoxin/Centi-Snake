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

	public float hue;
	public float saturation;
	public float brightness;

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		score = 0;
		StoreHighscore(0);
		LoadPrefs();

		GameObject parent = GameObject.FindGameObjectWithTag("ColorSpace");
		if (parent != null) {
			ColorChildren(parent);
		}

		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().backgroundColor = GetColor(true);
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

		GameObject parent = GameObject.FindGameObjectWithTag("ColorSpace");
		if (parent != null)
		{
			ColorChildren(parent);
		}
	}

	private void ColorChildren(GameObject parent)
	{
		foreach (Transform child in parent.transform)
		{
			Color newColor;
			if (child.gameObject.GetComponent<ColorController>() != null)
			{
				newColor = child.gameObject.GetComponent<ColorController>().GetColor();
			}
			else
			{
				newColor = GetColor(false);
			}

			if (child.gameObject.GetComponent<Button>() == null)
			{
				foreach (Text text in child.gameObject.GetComponents<Text>())
				{
					text.color = newColor;
				}

				foreach (Image image in child.gameObject.GetComponents<Image>())
				{
					image.color = newColor;
				}
			}

			ColorChildren(child.gameObject);
		}
	}

	private void LoadPrefs()
	{
		hue = PlayerPrefs.GetFloat("hue", 113f);
		saturation = PlayerPrefs.GetFloat("saturation", 100);
		brightness = PlayerPrefs.GetFloat("brightness", 100);
	}

	public void StoreHighscore(int newHighscore) {
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);
		if (newHighscore > oldHighscore)
		{
			PlayerPrefs.SetInt("highscore", newHighscore);
		}
	}

	private static Color GetColor(bool shadeForBG) {
		float r, g, b;

		float h = instance.hue;
		float s = instance.saturation / 100f;
		float v = instance.brightness / 100f;

		if (shadeForBG)
		{
			v *= 0.1f;
		}

		if (Mathf.Abs(s) <= 0.001f) {
			// achromatic (grey)
			r = g = b = v;
		} else {
			h /= 60; // sector 0 to 5
			int i = Mathf.FloorToInt(h);
			float f = h - i;
			float p = v * (1 - s);
			float q = v * (1 - s * f);
			float t = v * (1 - s * (1 - f));
			switch (i) {
				case 0:
					r = v;
					g = t;
					b = p;
					break;
				case 1:
					r = q;
					g = v;
					b = p;
					break;
				case 2:
					r = p;
					g = v;
					b = t;
					break;
				case 3:
					r = p;
					g = q;
					b = v;
					break;
				case 4:
					r = t;
					g = p;
					b = v;
					break;
				default: // case 5:
					r = v;
					g = p;
					b = q;
					break;
			}
		}

		return new Color(r, g, b);
	}
}
