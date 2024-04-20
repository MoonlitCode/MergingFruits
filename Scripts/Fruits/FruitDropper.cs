#nullable enable
using Godot;

namespace MergingFruits.Scripts.Fruits;

public partial class FruitDropper : Area2D {
	[Export] private Node? _targetParent;
	[Export] private Sprite2D? _dropperSprite;
	[Export] private TextureRect? _fruitRect;
	[Export] private CollisionShape2D? _collisionShape2D;
	private FruitPackedWeightedList? _fruitPackedWeightedList;

	public void Initialize(FruitPackedWeightedList fruitPackedWeightedList) {
		_fruitPackedWeightedList = fruitPackedWeightedList;
	}

	//todo External inputs, move dropper, method for spawning fruit, include timer.
	/// <summary>
	/// Attempts to immediately reposition dropper. To be used with KB+M control schemes.
	/// </summary>
	/// <param name="globalPosition"></param>
	public void TryRepositionDropper(Vector2 globalPosition) {
		if (!HasAllComponents()) return;
		
		var colliderSize = _collisionShape2D.Shape.GetRect().Size.X / 2;
		if (!(Mathf.Abs(globalPosition.X) < colliderSize)) return;
		_dropperSprite.GlobalPosition = new Vector2(globalPosition.X, _dropperSprite.GlobalPosition.Y);
	}

	/// <summary>
	/// Attempts to translate dropper over time. To be used with Gamepad control schemes.
	/// </summary>
	/// <param name="direction"></param>
	public void TryTranslatingDropper(Vector2 direction) {
		if (!HasAllComponents()) return;
		
	}

	public void TrySpawnSpecificFruit(int index) {
		if (!HasAllComponents()) return;
		if (index > _fruitPackedWeightedList.Data.Count) index -= 1;
		if (index < 0) index = 0;

		var targetPosition = _fruitRect.GlobalPosition + (_fruitRect.Size / 2);
		FruitSpawner.RB2DInstOrNull(_targetParent, _fruitPackedWeightedList.Data[index].FruitScene, targetPosition);
	}

	public void TrySpawnWeightedFruit() {
		if (!HasAllComponents()) return;
		
		var fruitIndex = _fruitPackedWeightedList.Data.Count - 1;
		
		for (var i = fruitIndex; i >= 0; i--) {
			var fruitWeight = _fruitPackedWeightedList.Data[i].Weight;
			var randomRoll = GD.Randf();
			if (fruitWeight >= randomRoll) {
				GD.Print($"FruitWeight:{fruitWeight} | Roll:{randomRoll}");
				TrySpawnSpecificFruit(i);
				break;
			}
			//note Safety Check In-case Everything Is Rolled Past.
			if (i <= 0) TrySpawnSpecificFruit(0);
		}
	}

	private bool HasAllComponents() {
		if (_dropperSprite is null) {
			GD.PrintErr($"FruitDropper.cs is Missing: _dropperSprite");
			return false;
		}
		if (_targetParent is null) {
			GD.PrintErr($"FruitDropper.cs is Missing: _targetParent");
			return false;
		}
		if (_fruitRect is null) {
			GD.PrintErr($"FruitDropper.cs is Missing: _fruitRect");
			return false;
		}
		if (_collisionShape2D is null) {
			GD.PrintErr($"FruitDropper.cs is Missing: _collisionShape2D");
			return false;
		}

		return true;
	}
}
