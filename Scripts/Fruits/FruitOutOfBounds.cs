using Godot;

namespace MergingFruits.Scripts.Fruits;

public partial class FruitOutOfBounds : Area2D {
    //todo This Whole Thing
    public override void _Ready() {
        base._Ready();
        BodyEntered += OnBodyEntered;
    }

    public override void _ExitTree() {
        base._ExitTree();
        BodyEntered -= OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body) {
        if (body.GetNodeOrNull(Fruit.Class) is not Fruit collidedFruit) return;
        GD.Print($"FruitOutOfBounds: {collidedFruit} BodyEntered");
    }
}