using UnityEngine;

public class DamageController : MonoBehaviour {
	public int hitpointsTaken;
	private RectTransform myRectTransform;

	void Start() {
		myRectTransform = gameObject.GetComponent<RectTransform>();
	}

	void FixedUpdate() {
		// Move forward one step
		Vector3 newPosition = myRectTransform.anchoredPosition3D + gameObject.transform.up * GameData.instance.movementStep;
		newPosition.x = Mathf.RoundToInt(newPosition.x);
		newPosition.y = Mathf.RoundToInt(newPosition.y);

		GameObject[] searchSpace = GameObject.FindObjectsOfType<GameObject>();
		bool foundHit = false;
		foreach (GameObject check in searchSpace) {
			RectTransform checkTransform = check.GetComponent<RectTransform>();
			if (checkTransform != null && checkTransform.anchoredPosition3D == newPosition) {
				if (!check.GetComponent<PlayerController>() && !check.GetComponent<PlayerFollower>()) {
					if (check.GetComponent<FoodController>()) {
						if (check.GetComponent<EnemyController>()) {
							check.GetComponent<EnemyController>().randomPosition();
						} else if (check.GetComponent<FoodController>()) {
							check.GetComponent<FoodController>().randomPosition();
						} else {
							Destroy(check);
						}
					}

					foundHit = true;
					break;
				}
			}
		}

		if (!foundHit) {
			myRectTransform.anchoredPosition3D = newPosition;
		} else {
			Destroy(gameObject);
		}

		if (Mathf.Abs(myRectTransform.anchoredPosition3D.x) > GameData.instance.maxCoord ||
			Mathf.Abs(myRectTransform.anchoredPosition3D.y) > GameData.instance.maxCoord) {
			Destroy(gameObject);
		}
	}
}