using System;
using Godot;
using MergingFruits.Scripts.MoonDev;

namespace MergingFruits.Scripts.Fruits;

public partial class FruitOutOfBounds : Area2D {
    public static event EventHandler OnFruitTimedOut;
    
    private ClassTimer _outOfBoundsTimer = new();
    private bool _hasFruitOutOfBounds;
    
    public override void _Ready() {
        base._Ready();
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _ExitTree() {
        base._ExitTree();
        BodyEntered -= OnBodyEntered;
        BodyExited -= OnBodyExited;
    }

    public override void _Process(double delta) {
        base._Process(delta);
        if (_outOfBoundsTimer.HasTimerEnded) {
            OnFruitTimedOut?.Invoke(this, EventArgs.Empty);
            QueueFree();
        }
        if (_hasFruitOutOfBounds) _outOfBoundsTimer.DecrementCurrentTimer(delta);
        if (!_hasFruitOutOfBounds && _outOfBoundsTimer.CurrentTimer <= _outOfBoundsTimer.MaxTimer) _outOfBoundsTimer.ResetTimer();
    }

    public void Initialize(float outOfBoundsTimerMax) {
        _outOfBoundsTimer.InitializeTimer(outOfBoundsTimerMax);
    }

    private void OnBodyEntered(Node2D body) {
        if (body.GetNodeOrNull(Fruit.ClassName) is not Fruit) return;
        _hasFruitOutOfBounds = true;
    }

    private void OnBodyExited(Node2D body) {
        if (body.GetNodeOrNull(Fruit.ClassName) is not Fruit) return;
        _hasFruitOutOfBounds = false;
    }
}