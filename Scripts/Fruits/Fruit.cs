#nullable enable
using System;
using Godot;

namespace MergingFruits.Scripts.Fruits;

[GlobalClass]
public partial class Fruit : Node2D {
	public static event EventHandler<FruitPair>? OnSameFruitTierCollision;
	public const string Class = "Fruit";

	[Export] private int _fruitTier;
	private RigidBody2D? _parentRigidBody2D;

	public int FruitTier => _fruitTier;
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
		if (body.GetNodeOrNull($"{Name}") is not Fruit collidedFruit) return;
		if (collidedFruit.FruitTier != _fruitTier) return;
		
		var fruitPair = new FruitPair() {
			Fruit1 = this,
			Fruit2 = collidedFruit
		};
		OnSameFruitTierCollision?.Invoke(this, fruitPair);
	}

	private bool HasAllComponents() {
		if (_parentRigidBody2D is null) {
			GD.PrintErr($"Game.cs is Missing: _parentRigidBody2D");
			return false;
		}

		return true;
	}
}
