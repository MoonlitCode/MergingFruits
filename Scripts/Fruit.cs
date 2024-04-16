using System;
using Godot;

namespace MergingFruits.Scripts;

[GlobalClass]
public partial class Fruit : Node2D {
	public static event EventHandler<FruitPair> OnFruitCollision;

	[Export] private int _fruitTier;
	private RigidBody2D _parentRigidBody2D;
	
	public int FruitTier => _fruitTier;

	public override void _Ready() {
		base._Ready();
		_parentRigidBody2D = GetParent<RigidBody2D>();
		_parentRigidBody2D.BodyEntered += OnBodyEntered;
	}

	public override void _ExitTree() {
		base._ExitTree();
		_parentRigidBody2D.BodyEntered -= OnBodyEntered;
	}

	private void OnBodyEntered(Node body) {
		if (body.GetNodeOrNull($"{Name}") is not Fruit collidedFruit) return;
		if (collidedFruit.FruitTier != _fruitTier) return;
		var fruitPair = new FruitPair() {
			Fruit1 = this,
			Fruit2 = collidedFruit
		};
		OnFruitCollision?.Invoke(this, fruitPair);
		//todo Fire event with both fruits. Somehow fuse only once getting both Vector2.Positions, and get the in between, spawn one tier higher
	}
}
