#nullable enable
using System;
using Godot;
using MergingFruits.Scripts.Fruits;
using MergingFruits.Scripts.MoonDev;
using MergingFruits.Scripts.UIUX;

namespace MergingFruits.Scripts.Game;

public partial class GameMain : Node {
	[Export] private GameControls? _gameControls;
	[ExportGroup("UI")]
	[Export] private GameUI? _gameUI;
	[Export] private FruitUpcomingUI? _fruitUpcomingUI;
	[Export] private FruitTimeOutUI? _fruitTimeOutUI;
	[Export] private ScoreUI? _scoreUI;
	[Export] private GameOverUI? _gameOverUI;
	[Export] private OverlayUI? _overlayUI;
	[ExportGroup("Components")]
	[Export] private FruitOutOfBounds? _fruitOutOfBounds;
	[Export] private FruitDropper? _fruitDropper;
	[Export] private FruitInfoList? _fruitPackedList;
	[Export] private Node2D? _fruitBasket;
	[ExportGroup("Variables")]
	[Export(PropertyHint.Range, "0, 10, 0.1")] private float _outOfBoundsTimerMax = 2;
	[Export] private float _secondsBetweenFruitSpawn = 1f;
	[Export] private float _fruitSpawnDelay = 0.5f;

	private ClassTimer _dropTimer = new();
	private ClassTimer _dropDelayTimer = new();
	private ClassTimer _outOfBoundsTimer = new();
	private int _currentFruitIndex;
	private int _nextFruitIndex;
	private int _currentScore;
	private bool _isGameOver;
	
	public override void _Ready() {
		base._Ready();
		if (!HasAllComponents()) return;

		Fruit.OnSameFruitTierCollision += Fruit_OnSameFruitTierCollision;
		FruitOutOfBounds.OnFruitTimedOut += FruitOutOfBounds_OnFruitTimedOut;
		FruitPicker.Initialize(_fruitPackedList!);
		FruitMerger.Initialize(_fruitBasket, _fruitPackedList);

		// UI
		_gameControls!.OnActionDropFruit += GameControls_OnActionDropFruit;
		_fruitUpcomingUI!.Initialize(_fruitPackedList);
		_gameOverUI!.SetUIActiveState(false);
		_overlayUI!.SetUIActiveState(false);
		// Components
		_gameControls.Initialize(_gameOverUI, _fruitDropper, _dropTimer);
		_fruitOutOfBounds!.Initialize(_outOfBoundsTimer);
		// Variables
		_dropTimer.InitializeTimer(_secondsBetweenFruitSpawn);
		_dropDelayTimer.InitializeTimer(_fruitSpawnDelay);
		_outOfBoundsTimer.InitializeTimer(_outOfBoundsTimerMax);
		_currentFruitIndex = FruitPicker.GetWeightedIndex();
		_nextFruitIndex = FruitPicker.GetWeightedIndex();
		
		SetNextFruit(true);
	}

	public override void _ExitTree() {
		base._ExitTree();
		_gameControls!.OnActionDropFruit -= GameControls_OnActionDropFruit;
		Fruit.OnSameFruitTierCollision -= Fruit_OnSameFruitTierCollision;
		FruitOutOfBounds.OnFruitTimedOut -= FruitOutOfBounds_OnFruitTimedOut;
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (_isGameOver) return;
		
		if (!HasAllComponents()) return;
		if (Input.IsActionJustPressed(StringInputs.UIToggleVisibility)) _overlayUI!.SetUIActiveState(!_overlayUI.Visible);
		_dropTimer.DecrementCurrentTimer((float)delta);
		_dropDelayTimer.DecrementCurrentTimer((float)delta);
		_fruitDropper!.UpdateProgressBar((float)delta, _dropTimer.TimerNormalizedIncreasing());
		_fruitTimeOutUI!.UpdateProgressBar((float)delta, _outOfBoundsTimer.TimerNormalizedIncreasing(), _fruitOutOfBounds.HasFruitOutOfBounds);
		_gameControls!.CalculateControls();
		
		if (_dropDelayTimer.HasTimerEnded && !_fruitDropper.HasFruit()) TrySpawnFruit();
	}

	private void TrySpawnFruit() {
		if (!HasAllComponents()) return;
		_fruitDropper!.TryReleaseFruit();
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
		_fruitDropper!.TryReleaseFruit();
	}
	
	private void Fruit_OnSameFruitTierCollision(object? sender, FruitPair e) {
		if (!HasAllComponents()) return;
		if (e.Fruit1.IsAttemptingMerge || e.Fruit2.IsAttemptingMerge) return;
		e.Fruit1.IsAttemptingMerge = true;
		e.Fruit2.IsAttemptingMerge = true;
		_currentScore += e.Fruit1.ScoreValue;
		_scoreUI!.UpdateScore(_currentScore);
		FruitMerger.ProcessMerge(e);
	}

	private void FruitOutOfBounds_OnFruitTimedOut(object? sender, EventArgs e) {
		_isGameOver = true;
		_fruitDropper!.QueueFree();
		_fruitDropper = null;
		_overlayUI!.SetUIActiveState(false);
		_gameOverUI!.SetUIActiveState(true);
	}

	private void SetNextFruit(bool isFirstRoll = false) {
		if (!HasAllComponents()) return;
		_currentFruitIndex = isFirstRoll ? FruitPicker.GetWeightedIndex() : _nextFruitIndex;
		_nextFruitIndex = FruitPicker.GetWeightedIndex();
		_fruitUpcomingUI!.UpdateUpcomingImage(_nextFruitIndex);
	}

	private bool HasAllComponents() {
#if TOOLS
		if (_isGameOver) return true;
		if (_gameControls is null) {
			GD.PrintErr($"Game.cs is Missing: _gameControls");
			return false;
		}
		if (_gameUI is null) {
			GD.PrintErr($"Game.cs is Missing: _gameUI");
			return false;
		}
		if (_gameOverUI is null) {
			GD.PrintErr($"Game.cs is Missing: _gameOverUI");
			return false;
		}
		if (_overlayUI is null) {
			GD.PrintErr($"Game.cs is Missing: _overlayUI");
			return false;
		}
		if (_fruitUpcomingUI is null) {
			GD.PrintErr($"Game.cs is Missing: _fruitUpcomingUI");
			return false;
		}
		if (_scoreUI is null) {
			GD.PrintErr($"Game.cs is Missing: _scoreUI");
			return false;
		}
		if (_fruitOutOfBounds is null) {
			GD.PrintErr($"Game.cs is Missing: _fruitOutOfBounds");
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
#endif
		return true;
	}
}
