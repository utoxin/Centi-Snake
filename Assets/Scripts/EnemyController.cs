using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public GameObject bulletPrefab;
	public int fireInterval;

	private int fireCounter = 0;
	private int bulletRotation;
	private RectTransform myRectTransform;

	void Start() {
		myRectTransform = gameObject.GetComponent<RectTransform>();
		randomPosition();
	}

	void FixedUpdate()
	{
		fireCounter++;

		if (fireCounter >= fireInterval)
		{
			fireCounter = 0;
			fireShot();
		}
	}

	void fireShot()
	{
		GameObject bullet = Instantiate(bulletPrefab);
		RectTransform bulletTransform = bullet.GetComponent<RectTransform>();

		bulletTransform.SetParent(myRectTransform.parent);

		bulletTransform.Rotate(Vector3.forward, bulletRotation);

		bulletTransform.anchoredPosition3D = myRectTransform.anchoredPosition3D;
//		bulletTransform.anchoredPosition3D += bulletTransform.up * GameData.instance.movementStep;

		if (Mathf.Abs(bulletTransform.anchoredPosition3D.x) > GameData.instance.maxCoord || Mathf.Abs(bulletTransform.anchoredPosition3D.y) > GameData.instance.maxCoord) {
			Destroy(bullet);
			bulletRotation = 90 * Random.Range(0, 4);
			fireShot();
		}
	}

	public void randomPosition() {
		Vector3 newPosition;
		GameObject[] searchSpace = GameObject.FindObjectsOfType<GameObject>();
		bool foundHit;

		do {
			foundHit = false;
			newPosition = new Vector3(Random.Range(-15, 17) * 16 - 8, Random.Range(-15, 17) * 16 - 8, -1);

			foreach (GameObject check in searchSpace) {
				RectTransform checkTransform = check.GetComponent<RectTransform>();
				if (checkTransform != null && checkTransform.anchoredPosition3D == newPosition) {
					foundHit = true;
					break;
				}
			}
		} while (foundHit);

		gameObject.GetComponent<RectTransform>().anchoredPosition3D = newPosition;

		bulletRotation = 90*Random.Range(0, 4);
		fireCounter = 0;
	}
}
