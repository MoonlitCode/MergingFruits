using System;
using Godot;
using MergingFruits.Scripts.MoonDev;

namespace MergingFruits.Scripts.Fruits;

public partial class FruitOutOfBounds : Area2D {
    public static event EventHandler OnFruitTimedOut;
    
    private ClassTimer _fadeTimer;
    private bool _hasFruitOutOfBounds;

    public bool HasFruitOutOfBounds => _hasFruitOutOfBounds;
    
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
        if (_fadeTimer.HasTimerEnded) {
            OnFruitTimedOut?.Invoke(this, EventArgs.Empty);
            QueueFree();
        }
        if (_hasFruitOutOfBounds) _fadeTimer.DecrementCurrentTimer(delta);
        if (!_hasFruitOutOfBounds && _fadeTimer.CurrentTimer <= _fadeTimer.MaxTimer) _fadeTimer.ResetTimer();
    }

    public void Initialize(ClassTimer outOfBoundsTimer) => _fadeTimer = outOfBoundsTimer;

    private void OnBodyEntered(Node2D body) {
        if (body.GetNodeOrNull(Fruit.ClassName) is not Fruit) return;
        _hasFruitOutOfBounds = true;
    }

    private void OnBodyExited(Node2D body) {
        if (body.GetNodeOrNull(Fruit.ClassName) is not Fruit) return;
        _hasFruitOutOfBounds = false;
    }
}