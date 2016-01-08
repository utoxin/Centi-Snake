using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
	public Slider hueSlider;
	public Slider saturationSlider;
	public Slider brightnesSlider;

	void Start ()
	{
		hueSlider.value = GameData.instance.hue;
		saturationSlider.value = GameData.instance.saturation;
		brightnesSlider.value = GameData.instance.brightness;
	}

	public void HueChanged(float value) {
		GameData.instance.hue = value;
		PlayerPrefs.SetFloat("hue", value);
	}

	public void SaturationChanged(float value) {
		GameData.instance.saturation = value;
		PlayerPrefs.SetFloat("saturation", value);
	}

	public void BrightnessChanged(float value) {
		GameData.instance.brightness = value;
		PlayerPrefs.SetFloat("brightness", value);
	}
}
