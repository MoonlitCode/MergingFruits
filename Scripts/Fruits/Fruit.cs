#nullable enable
using System;
using Godot;
using static MergingFruits.Scripts.MoonDev.MoonConstants;

namespace MergingFruits.Scripts.Fruits;

[GlobalClass]
public partial class Fruit : Node2D {
	public static event EventHandler<FruitPair>? OnSameFruitTierCollision;
	public const string ClassName = "Fruit";

	[Export] private int _fruitTier;
	[Export] private int _scoreValue;
	private RigidBody2D? _parentRigidBody2D;

	public int FruitTier => _fruitTier;
	public int ScoreValue => _scoreValue;
	public bool IsAttemptingMerge;

	public override void _Ready() {
		base._Ready();
		_parentRigidBody2D = GetParentOrNull<RigidBody2D>();
		if (!HasAllComponents()) return;
		_parentRigidBody2D.BodyEntered += OnBodyEntered;
	}

	public override void _ExitTree() {
		base._ExitTree();
		if (_parentRigidBody2D != null) _parentRigidBody2D.BodyEntered -= OnBodyEntered;
	}

	private void OnBodyEntered(Node body) {
		if (!HasAllComponents()) return;
		if (IsAttemptingMerge) return;
		if (body.GetNodeOrNull($"{ClassName}") is not Fruit collidedFruit) return;
		if (collidedFruit.FruitTier != _fruitTier) return;
		
		var fruitPair = new FruitPair() {
			Fruit1 = this,
			Fruit2 = collidedFruit
		};
		OnSameFruitTierCollision?.Invoke(this, fruitPair);
	}

	private bool HasAllComponents() {
		if (_parentRigidBody2D is null) {
			GD.PrintErr($"{GetType().Name} {IsMissingString} {nameof(_parentRigidBody2D)}");
			return false;
		}

		return true;
	}
}
