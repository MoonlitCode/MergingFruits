#nullable enable
using System;
using Godot;
using MergingFruits.Scripts.Fruits;
using MergingFruits.Scripts.MoonDev;

namespace MergingFruits.Scripts.Game;

public partial class GameMain : Node {
	[Export] private GameControls? _gameControls;
	
	[Export] private FruitDropper? _fruitDropper;
	[Export] private FruitPackedList? _fruitPackedList;
	[Export] private FruitPackedWeightedList? _fruitPackedWeightedList;
	[Export] private Node? _fruitBasket;
	[Export] private float _secondsBetweenFruitSpawn = 1f;

	private PackedScene _currentFruit;
	private PackedScene _nextFruit;

	private ClassTimer _dropTimer = new();
	
	public override void _Ready() {
		base._Ready();
		if (!HasAllComponents()) return;

		_gameControls.OnActionDropFruit += GameControls_OnActionDropFruit;
		Fruit.OnSameFruitTierCollision += Fruit_OnSameFruitTierCollision;
		FruitSpawner.OnFruitSpawned += FruitSpawner_OnFruitSpawned;
		FruitPicker.Initialize(_fruitPackedWeightedList);
		FruitMerger.Initialize(_fruitBasket, _fruitPackedList);

		_dropTimer.InitializeTimer(_secondsBetweenFruitSpawn);
		_gameControls.Initialize(_fruitDropper, _dropTimer);
		_currentFruit = FruitPicker.TryPickWeightedFruit();
		_nextFruit = FruitPicker.TryPickWeightedFruit();
		SetNextFruit();
	}


	public override void _Process(double delta) {
		base._Process(delta);
		if (!HasAllComponents()) return;
		_dropTimer.DecrementCurrentTimer((float)delta);
		_fruitDropper.UpdateProgressBar((float)delta, _dropTimer.TimerNormalizedIncreasing());
		_gameControls.CalculateControls();
	}

	private void GameControls_OnActionDropFruit(object? sender, EventArgs e) {
		FruitSpawner.RB2DInstantiateOrNull(_fruitBasket, _currentFruit,_fruitDropper.DropPosition);
	}
	
	private void Fruit_OnSameFruitTierCollision(object? sender, FruitPair e) {
		if (e.Fruit1.IsAttemptingMerge || e.Fruit2.IsAttemptingMerge) return;
		e.Fruit1.IsAttemptingMerge = true;
		e.Fruit2.IsAttemptingMerge = true;
		FruitMerger.ProcessMerge(e);
	}

	private void FruitSpawner_OnFruitSpawned(object? sender, EventArgs e) {
		SetNextFruit();
	}

	private void SetNextFruit() {
		_currentFruit = _currentFruit is null ? FruitPicker.TryPickWeightedFruit() : _nextFruit;
		_nextFruit = FruitPicker.TryPickWeightedFruit();
	}

	private bool HasAllComponents() {
		if (_gameControls is null) {
			GD.PrintErr($"Game.cs is Missing: _gameControls");
			return false;
		}
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
