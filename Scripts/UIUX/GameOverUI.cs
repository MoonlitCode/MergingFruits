using Godot;
using static MergingFruits.Scripts.StringInputs;

namespace MergingFruits.Scripts.UIUX;

public partial class GameOverUI : Control {
    [Export] private Button _restartButton;

    public override void _Ready() {
        base._Ready();
        _restartButton.Pressed += RestartButton_Pressed;
    }

    public override void _ExitTree() {
        base._ExitTree();
        _restartButton.Pressed -= RestartButton_Pressed;
    }

    public override void _Process(double delta) {
        base._Process(delta);
        if (Input.IsActionJustPressed(UIConfirm)) RestartButton_Pressed();
    }

    private void RestartButton_Pressed() => GetTree().ReloadCurrentScene();

    public void SetUIActiveState(bool state) {
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
}