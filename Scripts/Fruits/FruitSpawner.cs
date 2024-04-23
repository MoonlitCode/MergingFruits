using System;
using Godot;

namespace MergingFruits.Scripts.Fruits;

public static class FruitSpawner {
    public static event EventHandler OnFruitSpawned;
    
    public static RigidBody2D RB2DInstantiateOrNull(Node parentNode, PackedScene fruitPackedScene, Vector2 position) {
        if (fruitPackedScene is null) return null;
        var fruitNode = GD.Load<PackedScene>(fruitPackedScene.ResourcePath);
        var fruitInstance = fruitNode.InstantiateOrNull<RigidBody2D>();
        if (fruitInstance is null) return null;
        fruitInstance.ProcessMode = Node.ProcessModeEnum.Disabled;
        fruitInstance.GlobalPosition = position;
        parentNode.AddChild(fruitInstance);
        fruitInstance.ProcessMode = Node.ProcessModeEnum.Always;
        OnFruitSpawned?.Invoke(null, EventArgs.Empty);
        return fruitInstance;
    }
}