using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

	public static DontDestroyOnLoad instance;

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}
}
