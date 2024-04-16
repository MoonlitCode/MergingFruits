using Godot;

namespace MergingFruits.Scripts.UIUX;

public partial class GameUI : Control {
    private Vector2 _lastScreenSize;

    public override void _Ready() {
        base._Ready();
        var currentScreenSize = DisplayServer.WindowGetSize();
        UpdateCanvasSize(currentScreenSize);
    }

    public override void _Process(double delta) {
        base._Process(delta);
        var currentScreenSize = DisplayServer.WindowGetSize();
        if (currentScreenSize == _lastScreenSize) return;
        GD.Print($"GameUI: ScreenSizeChanged: Last:{_lastScreenSize} | Current:{currentScreenSize}");
        UpdateCanvasSize(currentScreenSize);
    }

    private void UpdateCanvasSize(Vector2 screenSize) {
        _lastScreenSize = screenSize;
        Size = screenSize;
        Position = -screenSize / 2;
    }
}