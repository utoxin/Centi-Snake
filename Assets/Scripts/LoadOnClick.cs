using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {
	public void LoadScene(int level)
	{
		// Hack for quit button
		if (level == -1)
		{
			Application.Quit();
			return;
		}

		SceneManager.LoadScene(level);
	}
}
