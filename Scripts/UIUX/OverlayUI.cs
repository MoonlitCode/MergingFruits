using Godot;

namespace MergingFruits.Scripts.UIUX;

public partial class OverlayUI : Control {
	[Export] private Button _quitButton;
	[Export] private Button _restartButton;

	public override void _Ready() {
		base._Ready();
		if (!HasAllComponents()) return;
		_quitButton.Pressed += QuitButton_Pressed;
		_restartButton.Pressed += RestartButton_Pressed;
	}

	public override void _ExitTree() {
		base._ExitTree();
		if (!HasAllComponents()) return;
		_quitButton.Pressed -= QuitButton_Pressed;
		_restartButton.Pressed -= RestartButton_Pressed;
	}

	private void QuitButton_Pressed() => GetTree().Quit();

	private void RestartButton_Pressed() => GetTree().ReloadCurrentScene();

	public void SetUIActiveState(bool state) {
		if (!HasAllComponents()) return;
		switch (state) {
			case true:
				Visible = true;
				ProcessMode = ProcessModeEnum.Always;
				_restartButton.GrabFocus();
				break;
			case false:
				Visible = false;
				ProcessMode = ProcessModeEnum.Disabled;
				break;
		}
	}

	private bool HasAllComponents() {
		if (_quitButton is null) {
			GD.PrintErr($"OverlayUI.cs is Missing: _quitButton");
			return false;
		}
		if (_restartButton is null) {
			GD.PrintErr($"OverlayUI.cs is Missing: _restartButton");
			return false;
		}
		return true;
	}
}