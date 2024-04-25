using System;
using Godot;

namespace MergingFruits.Scripts.Fruits;

public static class FruitSpawner {
    public static event EventHandler OnFruitSpawned;
    
    public static RigidBody2D RB2DInstantiateOrNull(Node2D parentNode, PackedScene fruitPackedScene, Vector2 globalPosition, bool freezePhysics = false) {
        if (fruitPackedScene is null) return null;
        var fruitNode = GD.Load<PackedScene>(fruitPackedScene.ResourcePath);
        var fruitInstance = fruitNode.InstantiateOrNull<RigidBody2D>();
        if (fruitInstance is null) return null;
        fruitInstance.ProcessMode = Node.ProcessModeEnum.Disabled;
        parentNode.AddChild(fruitInstance);
        fruitInstance.GlobalPosition = globalPosition;
        fruitInstance.ProcessMode = Node.ProcessModeEnum.Always;
        //note All of these FREEZE it in position, but not follow the dropper
        
        OnFruitSpawned?.Invoke(null, EventArgs.Empty);
        return fruitInstance;
    }
}