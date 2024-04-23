using System;
using Godot;
using MergingFruits.Scripts.Fruits;
using MergingFruits.Scripts.MoonDev;
using static MergingFruits.Scripts.StringInputs;

namespace MergingFruits.Scripts.Game;

[GlobalClass]
public partial class GameControls : Node {
    public event EventHandler OnActionDropFruit;
    
    private FruitDropper _fruitDropper;
    
    private ClassTimer _timer;

    public void Initialize(FruitDropper fruitDropper, ClassTimer timer) {
        _fruitDropper = fruitDropper;
        _timer = timer;
    }

    private Vector2 GetUserInputVector2()
        => Input.GetVector(DirLeft, DirRight, DirUp, DirDown);

    public void CalculateControls() {
        CalculateMouseControls();
        CalculateButtonControls();
    }
    
    private void CalculateMouseControls() {
        var mousePosition = GetWindow().GetMousePosition();
        var halfWindowSize = (Vector2)(GetWindow().Size / 2);
        var centerOriginPosition = mousePosition - halfWindowSize ;
		
        _fruitDropper.TryRepositionDropper(centerOriginPosition);
        FruitDropper_TrySpawnFruit();
    }

    private void CalculateButtonControls() {
        _fruitDropper.TryTranslatingDropper(GetUserInputVector2());
        FruitDropper_TrySpawnFruit();
    }

    private void FruitDropper_TrySpawnFruit() {
        if (!_timer.HasTimerEnded) return;
        if (!Input.IsActionJustPressed(DropFruit)) return;
        OnActionDropFruit?.Invoke(this, EventArgs.Empty);
        _timer.ResetTimer();
    }
}