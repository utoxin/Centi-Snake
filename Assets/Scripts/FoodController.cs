using UnityEngine;
using UnityEngine.UI;

public class FoodController : MonoBehaviour
{
	public int hitpointsGranted;
	public Sprite[] fruitSprites;

	void Start()
	{
		if (gameObject.GetComponent<EnemyController>() == null)
		{
			randomPosition();
		}
	}

	public void randomPosition()
	{
		Vector3 newPosition;
		GameObject[] searchSpace = GameObject.FindObjectsOfType<GameObject>();
		bool foundHit;

		do
		{
			foundHit = false;
			newPosition = new Vector3(Random.Range(-15, 17) * 16 - 8, Random.Range(-15, 17) * 16 - 8, -1);

			foreach (GameObject check in searchSpace)
			{
				RectTransform checkTransform = check.GetComponent<RectTransform>();
				if (checkTransform != null && checkTransform.anchoredPosition3D == newPosition)
				{
					foundHit = true;
					break;
				}
			}
		} while (foundHit);

		gameObject.GetComponent<RectTransform>().anchoredPosition3D = newPosition;

		int fruitIndex = Random.Range(0, Mathf.Min(fruitSprites.Length, GameData.instance.gamePhase));
		Debug.Log(fruitIndex);
        gameObject.GetComponent<Image>().sprite = fruitSprites[fruitIndex];
		hitpointsGranted = (fruitIndex + 1)*10;

		if (fruitIndex >= 4 && gameObject.GetComponent<ColorController>() != null)
		{
			gameObject.GetComponent<ColorController>().hueShift = 55;
			gameObject.GetComponent<ColorController>().ignoreSaturation = true;
		}
		else
		{
			gameObject.GetComponent<ColorController>().hueShift = 0;
			gameObject.GetComponent<ColorController>().ignoreSaturation = false;
		}

	}
}
