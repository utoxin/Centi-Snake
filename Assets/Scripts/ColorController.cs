using UnityEngine;

public class ColorController : MonoBehaviour {
	public float hueShift;
	public bool ignoreSaturation;

	public Color GetColor() {
		float r, g, b;

		float h = GameData.instance.hue;
		float s = GameData.instance.saturation / 100f;
		float v = GameData.instance.brightness / 100f;

		if (ignoreSaturation) {
			s = 1;
		}

		h += hueShift;
		h = h % 360;

		if (Mathf.Abs(s) <= 0.001f) {
			// achromatic (grey)
			r = g = b = v;
		} else {
			h /= 60; // sector 0 to 5
			int i = Mathf.FloorToInt(h);
			float f = h - i;
			float p = v * (1 - s);
			float q = v * (1 - s * f);
			float t = v * (1 - s * (1 - f));
			switch (i) {
				case 0:
					r = v;
					g = t;
					b = p;
					break;
				case 1:
					r = q;
					g = v;
					b = p;
					break;
				case 2:
					r = p;
					g = v;
					b = t;
					break;
				case 3:
					r = p;
					g = q;
					b = v;
					break;
				case 4:
					r = t;
					g = p;
					b = v;
					break;
				default: // case 5:
					r = v;
					g = p;
					b = q;
					break;
			}
		}

		return new Color(r, g, b);
	}
}