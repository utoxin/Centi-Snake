using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
	public GameObject[] enemyPrefabs;
	public GameObject gameArea;
	public GameObject foodPrefab;
	public int spawnInterval;

	private long lastTicks;
	private int wave;

	public AudioSource audioSource;

	public AudioClip tickSound;
	public AudioClip healSound;
	public AudioClip hitSound;

	void Start()
	{
		lastTicks = DateTime.Now.Ticks;
		wave = 1;
		GameData.instance.gamePhase = 1;
		spawnFruit(4);
		audioSource.clip = tickSound;
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		long elapsedTicks = DateTime.Now.Ticks - lastTicks;
		TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

		if (elapsedSpan.TotalSeconds > spawnInterval)
		{
			wave++;
			lastTicks = DateTime.Now.Ticks;

			int enemyIndex = Random.Range(0, enemyPrefabs.Length);
			GameObject enemy = Instantiate(enemyPrefabs[enemyIndex]);
			enemy.GetComponent<RectTransform>().SetParent(gameArea.GetComponent<RectTransform>());
			enemy.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);

			Time.timeScale += 0.05f;

			if (wave%10 == 0)
			{
				GameData.instance.gamePhase++;
				spawnFruit(1);
			}
		}

		audioSource.Play();
	}

	void spawnFruit(int count)
	{
		for (int i = 0; i < count; i++)
		{
			GameObject food = Instantiate(foodPrefab);
			food.GetComponent<RectTransform>().SetParent(gameArea.GetComponent<RectTransform>());
			food.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }
	}
}
