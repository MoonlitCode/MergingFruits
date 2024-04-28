using System;
using Godot;

namespace MergingFruits.Scripts.Fruits;

public static class FruitSpawner {
    public static event EventHandler OnFruitSpawned;
    
    /// <summary>
    /// Static method to spawn a "Fruit" from a 'PackedScene' as a 'RigidBody2D'. Returns 'null' if failed instantiation.
    /// </summary>
    /// <param name="parentNode">The 'Node' for the "Fruit" to be parented to.</param>
    /// <param name="fruitPackedScene">The "Fruit Scene" to spawn.</param>
    /// <param name="globalPosition">The 'Global Position'.</param>
    /// <param name="rotationOffset">Offset is applied in 'Degrees'. Default is +/-20.</param>
    /// <param name="freezePhysics">Whether 'Physics' should be applied to the 'RigidBody'.</param>
    /// <returns></returns>
    public static RigidBody2D RB2DInstantiateOrNull(Node2D parentNode, PackedScene fruitPackedScene, Vector2 globalPosition, float rotationOffset = 20, bool freezePhysics = false) {
        if (fruitPackedScene is null) return null;
        var fruitNode = GD.Load<PackedScene>(fruitPackedScene.ResourcePath);
        var fruitInstance = fruitNode.InstantiateOrNull<RigidBody2D>();
        if (fruitInstance is null) return null;
        fruitInstance.ProcessMode = Node.ProcessModeEnum.Disabled;
        parentNode.AddChild(fruitInstance);
        fruitInstance.GlobalPosition = globalPosition;
        fruitInstance.GlobalRotationDegrees += (float)GD.RandRange(-rotationOffset, rotationOffset);
        fruitInstance.ProcessMode = Node.ProcessModeEnum.Always;
        
        OnFruitSpawned?.Invoke(null, EventArgs.Empty);
        return fruitInstance;
    }
}