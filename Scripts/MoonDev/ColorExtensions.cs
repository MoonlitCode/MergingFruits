using Godot;

namespace MergingFruits.Scripts.MoonDev;

public static class ColorExtensions {
	public static Color WithRed(this Color color, float red) {
		var newColor = color;
		newColor.R = red;
		return newColor;
	}

	public static Color WithGreen(this Color color, float green) {
		var newColor = color;
		newColor.G = green;
		return newColor;
	}

	public static Color WithBlue(this Color color, float blue) {
		var newColor = color;
		newColor.B = blue;
		return newColor;
	}

	public static Color WithAlpha(this Color color, float alpha) {
		var newColor = color;
		newColor.A = alpha;
		return newColor;
	}
}