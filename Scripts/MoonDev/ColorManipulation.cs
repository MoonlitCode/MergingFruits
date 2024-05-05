using Godot;

namespace MergingFruits.Scripts.MoonDev;

public static class ColorManipulation {
	public static Color SetRed(Color color, float red) {
		var newColor = color;
		newColor.R = red;
		return newColor;
	}

	public static Color SetGreen(Color color, float green) {
		var newColor = color;
		newColor.G = green;
		return newColor;
	}

	public static Color SetBlue(Color color, float blue) {
		var newColor = color;
		newColor.B = blue;
		return newColor;
	}

	public static Color SetAlpha(Color color, float alpha) {
		var newColor = color;
		newColor.A = alpha;
		return newColor;
	}
}