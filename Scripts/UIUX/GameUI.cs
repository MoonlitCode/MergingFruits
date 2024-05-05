using Godot;

namespace MergingFruits.Scripts.UIUX;

public partial class GameUI : Control {
	private Vector2 _lastScreenSize;

	public void UpdateCanvasSize() {
		var currentScreenSize = DisplayServer.WindowGetSize();
		if (currentScreenSize == _lastScreenSize) return;
		_lastScreenSize = currentScreenSize;
		Size = currentScreenSize;
		Position = -currentScreenSize / 2;
	}
}