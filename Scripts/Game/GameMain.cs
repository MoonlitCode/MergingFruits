#nullable enable
using System;
using Godot;
using MergingFruits.Scripts.Fruits;
using MergingFruits.Scripts.MoonDev;
using MergingFruits.Scripts.UIUX;

namespace MergingFruits.Scripts.Game;

public partial class GameMain : Node {
	[Export] private GameControls? _gameControls;
	[Export] private FruitUpcomingUI? _fruitUpcomingUI;
	[Export] private FruitDropper? _fruitDropper;
	[Export] private FruitInfoList? _fruitPackedList;
	[Export] private Node2D? _fruitBasket;
	[Export] private float _secondsBetweenFruitSpawn = 1f;
	[Export] private float _fruitSpawnDelay = 0.5f;

	private int _currentFruitIndex;
	private int _nextFruitIndex;

	private ClassTimer _dropTimer = new();
	private ClassTimer _dropDelayTimer = new();
	
	public override void _Ready() {
		base._Ready();
		if (!HasAllComponents()) return;

		_gameControls.OnActionDropFruit += GameControls_OnActionDropFruit;
		Fruit.OnSameFruitTierCollision += Fruit_OnSameFruitTierCollision;
		FruitPicker.Initialize(_fruitPackedList);
		FruitMerger.Initialize(_fruitBasket, _fruitPackedList);

		_dropTimer.InitializeTimer(_secondsBetweenFruitSpawn);
		_dropDelayTimer.InitializeTimer(_fruitSpawnDelay);
		_currentFruitIndex = FruitPicker.GetWeightedIndex();
		_nextFruitIndex = FruitPicker.GetWeightedIndex();
		_gameControls.Initialize(_fruitDropper, _dropTimer);
		_fruitUpcomingUI.Initialize(_fruitPackedList);
		SetNextFruit(true);
	}

	public override void _ExitTree() {
		base._ExitTree();
		_gameControls.OnActionDropFruit -= GameControls_OnActionDropFruit;
		Fruit.OnSameFruitTierCollision -= Fruit_OnSameFruitTierCollision;
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (!HasAllComponents()) return;
		_dropTimer.DecrementCurrentTimer((float)delta);
		_dropDelayTimer.DecrementCurrentTimer((float)delta);
		_fruitDropper.UpdateProgressBar((float)delta, _dropTimer.TimerNormalizedIncreasing());
		_gameControls.CalculateControls();
		
		if (_dropDelayTimer.HasTimerEnded && !_fruitDropper.HasFruit()) TrySpawnFruit();
	}

	private void TrySpawnFruit() {
		if (!HasAllComponents()) return;
		_fruitDropper.TryReleaseFruit();
		SetNextFruit();
		var fruitInst = FruitSpawner.RB2DInstantiateOrNull(_fruitDropper, FruitPicker.TryGetFruitScene(_currentFruitIndex), _fruitDropper.DropGlobalPosition);
		if (fruitInst is null) return;
		_fruitDropper.SetFruitInstance(fruitInst);
		SetNextFruit();
	}

	private void GameControls_OnActionDropFruit(object? sender, EventArgs e) {
		if (!HasAllComponents()) return;
		_dropTimer.ResetTimer();
		_dropDelayTimer.ResetTimer();
		_fruitDropper.TryReleaseFruit();
	}
	
	private void Fruit_OnSameFruitTierCollision(object? sender, FruitPair e) {
		if (e.Fruit1.IsAttemptingMerge || e.Fruit2.IsAttemptingMerge) return;
		e.Fruit1.IsAttemptingMerge = true;
		e.Fruit2.IsAttemptingMerge = true;
		FruitMerger.ProcessMerge(e);
	}

	private void SetNextFruit(bool isFirstRoll = false) {
		if (!HasAllComponents()) return;
		_currentFruitIndex = isFirstRoll ? FruitPicker.GetWeightedIndex() : _nextFruitIndex;
		_nextFruitIndex = FruitPicker.GetWeightedIndex();
		_fruitUpcomingUI.UpdateUpcomingImage(_nextFruitIndex);
	}

	private bool HasAllComponents() {
		if (_gameControls is null) {
			GD.PrintErr($"Game.cs is Missing: _gameControls");
			return false;
		}
		if (_fruitUpcomingUI is null) {
			GD.PrintErr($"Game.cs is Missing: _fruitUpcomingUI");
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
