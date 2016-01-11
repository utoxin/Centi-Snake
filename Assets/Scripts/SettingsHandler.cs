using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour {
	public Slider brightnesSlider;
	public Slider hueSlider;
	public Slider saturationSlider;
	public Toggle screenFX;
	public Slider volumeSlider;

	void Start() {
		hueSlider.value = GameData.instance.hue;
		saturationSlider.value = GameData.instance.saturation;
		brightnesSlider.value = GameData.instance.brightness;
		volumeSlider.value = GameData.instance.volume;
		screenFX.isOn = GameData.instance.screenfx;
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

	public void VolumeChanged(float value) {
		GameData.instance.volume = value;
		PlayerPrefs.SetFloat("volume", value);
	}

	public void ScreenFXChanged(bool value) {
		GameData.instance.screenfx = value;
		PlayerPrefs.SetInt("screenfx", value ? 1 : 0);
	}
}