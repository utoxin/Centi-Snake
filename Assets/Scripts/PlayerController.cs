using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public int currentHealth;
	public int maxHealth;

	public AudioSource audioSource;

	public AudioClip tickSound;
	public AudioClip healSound;
	public AudioClip hitSound;

	public AudioClip nextSound;

	private List<GameObject> bodyPieces = new List<GameObject>(); 
	private List<Vector3> previousLocations = new List<Vector3>();

	public GameObject background;
	public GameObject bodyPiecePrefab;

	private RectTransform myRectTransform;

	void Start()
	{
		myRectTransform = gameObject.GetComponent<RectTransform>();
	}

	void FixedUpdate ()
	{
		// Move forward one step
		Vector3 newPosition = myRectTransform.anchoredPosition3D + gameObject.transform.up*GameData.instance.movementStep;
		newPosition.x = Mathf.RoundToInt(newPosition.x);
		newPosition.y = Mathf.RoundToInt(newPosition.y);

		// Search space ahead for collisions that would prevent movement
		GameObject[] searchSpace = GameObject.FindObjectsOfType<GameObject>();
		bool foundHit = false;
		foreach (GameObject check in searchSpace)
		{
			RectTransform checkTransform = check.GetComponent<RectTransform>();
			if (checkTransform != null && checkTransform.anchoredPosition3D == newPosition)
			{
				// Ignore collisions with food and damage sources - We can enter those spaces
				if (!check.GetComponent<FoodController>() && !check.GetComponent<DamageController>())
				{
					// If the collision is a body segment, do damage to it, so we can escape
					PlayerFollower follower = check.GetComponent<PlayerFollower>();
					if (follower != null)
					{
						follower.SetHitpoints(follower.currentHealth - 10);
					}

					foundHit = true;
					break;
				}
			}
		}

		// If there wasn't a blocking collision, move forward after all
		if (!foundHit)
		{
			myRectTransform.anchoredPosition3D = newPosition;
		}

		// Clamp values to stay in arena
		int x = Mathf.RoundToInt(Mathf.Clamp(myRectTransform.anchoredPosition.x, -1 * GameData.instance.maxCoord, GameData.instance.maxCoord));
		int y = Mathf.RoundToInt(Mathf.Clamp(myRectTransform.anchoredPosition.y, -1 * GameData.instance.maxCoord, GameData.instance.maxCoord));
		myRectTransform.anchoredPosition3D = new Vector3(x, y, myRectTransform.anchoredPosition3D.z);

		// Add previous location to list for body piece code
		if (previousLocations.Count == 0 || myRectTransform.anchoredPosition3D != previousLocations[0])
		{
			previousLocations.Insert(0, myRectTransform.anchoredPosition3D);
		}

		// Clean up body piece list to remove dead pieces, and calculate score addition
		bool clearRemaning = false;
		List<GameObject> newPieces = new List<GameObject>();
		int addScore = 0;
		for (int i = 0; i < bodyPieces.Count; i++)
		{
			if (bodyPieces[i] == null || clearRemaning)
			{
				clearRemaning = true;
				if (bodyPieces[i] != null)
				{
					if (Random.Range(0, 2) == 1)
					{
						// Transform pieces into blocks (50% odds)
						RectTransform pieceTransform = bodyPieces[i].GetComponent<RectTransform>();

						GameObject newBlock = Instantiate(GameData.instance.blockPrefab);
						RectTransform blockTransform = newBlock.GetComponent<RectTransform>();
						blockTransform.SetParent(pieceTransform.parent);
						blockTransform.anchoredPosition3D = pieceTransform.anchoredPosition3D;
					}

					Destroy(bodyPieces[i]);
				}
			}
			else
			{
				// Add valid pieces back to the list, and increment score
				newPieces.Add(bodyPieces[i]);
				addScore += bodyPieces[i].GetComponent<PlayerFollower>().currentHealth;
			}
		}
		bodyPieces = newPieces;

		// Add the new score, and update high score
		GameData.instance.score += addScore;
		GameData.instance.StoreHighscore(GameData.instance.score);

		// Pieces will move to their next location
		for (int i = 0; i < bodyPieces.Count && (i + 1) < previousLocations.Count; i++)
		{
			bodyPieces[i].GetComponent<RectTransform>().anchoredPosition3D = previousLocations[i + 1];
		}

		// Moving takes health
		if (bodyPieces.Count > 0)
		{
			PlayerFollower piece = bodyPieces[bodyPieces.Count - 1].GetComponent<PlayerFollower>();
			piece.SetHitpoints(piece.currentHealth - 1);
		}
		else
		{
			currentHealth--;
		}

		// If dead, return to start screen
		if (currentHealth <= 0)
		{
			SceneManager.LoadScene(0);
		}

		// Trim location list to max size required
		if ((bodyPieces.Count + 1) < previousLocations.Count) 
		{
			previousLocations = previousLocations.GetRange(0, bodyPieces.Count + 1);
		}

		audioSource.clip = nextSound;
		audioSource.Play();
		nextSound = tickSound;
	}

	void Update()
	{
		if (Input.GetButtonDown("Turn Left")) {
			gameObject.transform.Rotate(Vector3.forward, 90);
		}

		if (Input.GetButtonDown("Turn Right")) {
			gameObject.transform.Rotate(Vector3.forward, -90);
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene(0);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		FoodController food = other.gameObject.GetComponent<FoodController>();
		if (food != null)
		{
			int healthGranted = food.hitpointsGranted;

			if (currentHealth < maxHealth)
			{
				int missingHealth = maxHealth - currentHealth;
				if (missingHealth > healthGranted)
				{
					currentHealth += healthGranted;
					healthGranted = 0;
				}
				else
				{
					currentHealth = maxHealth;
					healthGranted -= missingHealth;
				}
			}

			foreach (GameObject piece in bodyPieces)
			{
				if (healthGranted == 0) {
					break;
				}

				PlayerFollower data = piece.GetComponent<PlayerFollower>();

				if (data.maxHealth > data.currentHealth)
				{
					int missingHealth = data.maxHealth - data.currentHealth;
					if (healthGranted >= missingHealth)
					{
						healthGranted -= missingHealth;
						data.SetHitpoints(data.maxHealth);
					}
					else
					{
						data.SetHitpoints(data.currentHealth + healthGranted);
						healthGranted = 0;
					}
				}
			}

			if (healthGranted > 0)
			{
				SpawnBodySegment(healthGranted);
			}

			if (other.gameObject.GetComponent<EnemyController>() == null)
			{
				food.randomPosition();
			}
			else
			{
				other.gameObject.GetComponent<EnemyController>().randomPosition();
            }

			nextSound = healSound;
		}

		DamageController damage = other.gameObject.GetComponent<DamageController>();
		if (damage != null) {
			int healthtaken = damage.hitpointsTaken;

			if (currentHealth < healthtaken)
			{
				SceneManager.LoadScene(0);
			}
			else
			{
				currentHealth -= healthtaken;
			}

			Destroy(other.gameObject);

			nextSound = hitSound;
		}
	}

	void SpawnBodySegment(int hitpoints)
	{
		while (hitpoints > 0)
		{
			GameObject newPiece = Instantiate(bodyPiecePrefab);
			RectTransform newTransform = newPiece.GetComponent<RectTransform>();

			newTransform.SetParent(myRectTransform.parent);
			newTransform.anchoredPosition3D = new Vector3(-2000, -2000, myRectTransform.anchoredPosition3D.z);
			newTransform.localScale = myRectTransform.localScale;

			if (hitpoints < 30)
			{
				newPiece.GetComponent<PlayerFollower>().SetHitpoints(hitpoints);
			}
			else
			{
				newPiece.GetComponent<PlayerFollower>().SetHitpoints(30);
			}

			newPiece.GetComponent<PlayerFollower>().player = this.gameObject;
			bodyPieces.Add(newPiece);

			hitpoints -= 30;
		}
	}
}
