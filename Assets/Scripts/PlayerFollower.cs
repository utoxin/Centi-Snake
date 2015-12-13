using UnityEngine;
using UnityEngine.UI;

public class PlayerFollower : MonoBehaviour
{
	public int currentHealth;
	public int maxHealth;

	public GameObject player;

	public Sprite[] healthImages;

	public void SetHitpoints(int hitpoints)
	{
		if (hitpoints == 0)
		{
			Destroy(gameObject);
			return;
		}

		currentHealth = Mathf.Clamp(hitpoints, 0, maxHealth);

		int index = Mathf.Clamp((hitpoints/10) - 1, 0, 2);
		gameObject.GetComponent<Image>().sprite = healthImages[index];
	}

	void OnTriggerEnter2D(Collider2D other) {
		DamageController damage = other.gameObject.GetComponent<DamageController>();
		if (damage != null) {
			int healthtaken = damage.hitpointsTaken;

			Destroy(other.gameObject);
			SetHitpoints(currentHealth - healthtaken);

			player.GetComponent<PlayerController>().nextSound = player.GetComponent<PlayerController>().hitSound;
		}
	}
}
