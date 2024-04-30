using Godot;

namespace MergingFruits.Scripts.Fruits;

public partial class FruitKillBounds : Area2D {
    public override void _Ready() {
        base._Ready();
        BodyEntered += OnBodyEntered;
    }

    public override void _ExitTree() {
        base._ExitTree();
        BodyEntered -= OnBodyEntered;
    }
    
    private void OnBodyEntered(Node2D body) {
        if (body.GetNodeOrNull(Fruit.ClassName) is not Fruit collidedFruit) return;
        collidedFruit.GetParent().QueueFree();
    }
}