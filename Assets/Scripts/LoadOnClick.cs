using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {
	public void LoadScene(int level) {
		// Hack for quit button
		if (level == -1) {
			Application.Quit();
			return;
		}

		if (level == 1) {
			Time.fixedDeltaTime = 0.5f;
		} else {
			Time.fixedDeltaTime = 0.05f;
		}

		Time.timeScale = 1f;
		SceneManager.LoadScene(level);
	}
}