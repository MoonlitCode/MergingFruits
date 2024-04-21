#nullable enable
using Godot;
using MergingFruits.Scripts.Fruits;
using MergingFruits.Scripts.MoonDev;
using static MergingFruits.Scripts.StringInputs;

namespace MergingFruits.Scripts;

public partial class Game : Node {
	[Export] private FruitDropper? _fruitDropper;
	[Export] private FruitPackedList? _fruitPackedList;
	[Export] private FruitPackedWeightedList? _fruitPackedWeightedList;
	[Export] private Node? _fruitBasket;
	[Export] private float _secondsBetweenFruitSpawn = 1f;

	private ClassTimer _dropperTimer = new();
	
	public override void _Ready() {
		base._Ready();
		if (!HasAllComponents()) return;

		_dropperTimer.InitializeTimer(_secondsBetweenFruitSpawn);
		_fruitDropper.Initialize(_fruitPackedWeightedList);
		FruitMerger.InitFruitMerger(_fruitBasket, _fruitPackedList);
		Fruit.OnSameFruitTierCollision += Fruit_OnSameFruitTierCollision;
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (!HasAllComponents()) return;
		_dropperTimer.DecrementCurrentTimer((float)delta);
		_fruitDropper.UpdateProgressBar((float)delta, _dropperTimer.TimerNormalizedIncreasing());
		CalculateMouseControls();
		CalculateButtonControls();
	}

	private Vector2 GetUserInputVector2()
		=> Input.GetVector(DirLeft, DirRight, DirUp, DirDown);

	private void CalculateMouseControls() {
		var mousePosition = GetWindow().GetMousePosition();
		var halfWindowSize = (Vector2)(GetWindow().Size / 2);
		var centerOriginPosition = mousePosition - halfWindowSize ;
		
		_fruitDropper.TryRepositionDropper(centerOriginPosition);
		FruitDropper_TrySpawnWeightedFruit();
	}

	private void CalculateButtonControls() {
		_fruitDropper.TryTranslatingDropper(GetUserInputVector2());
		FruitDropper_TrySpawnWeightedFruit();
	}

	private void FruitDropper_TrySpawnWeightedFruit() {
		if (!_dropperTimer.HasTimerEnded) return;
		if (!Input.IsActionJustPressed(DropFruit)) return;
		_dropperTimer.ResetTimer();
		_fruitDropper.TrySpawnWeightedFruit();
	}

	private void Fruit_OnSameFruitTierCollision(object? sender, FruitPair e) {
		if (e.Fruit1.IsAttemptingMerge || e.Fruit2.IsAttemptingMerge) return;
		e.Fruit1.IsAttemptingMerge = true;
		e.Fruit2.IsAttemptingMerge = true;
		FruitMerger.ProcessMerge(e);
	}

	private bool HasAllComponents() {
		if (_fruitDropper is null) {
			GD.PrintErr($"Game.cs is Missing: _fruitDropper");
			return false;
		}
		
		if (_fruitPackedList is null) {
			GD.PrintErr($"Game.cs is Missing: _packedFruitList");
			return false;
		}
		
		if (_fruitBasket is null) {
			GD.PrintErr($"Game.cs is Missing: _fruitBasket");
			return false;
		}

		return true;
	}
}
