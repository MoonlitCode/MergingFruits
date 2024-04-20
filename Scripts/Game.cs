#nullable enable
using Godot;
using MergingFruits.Scripts.Fruits;

namespace MergingFruits.Scripts;

public partial class Game : Node {
	[Export] private FruitDropper? _fruitDropper;
    
    [Export] private FruitPackedList? _fruitPackedList;
    [Export] private FruitPackedWeightedList? _fruitPackedWeightedList;
    [Export] private Node? _fruitBasket;
    
    public override void _Ready() {
        base._Ready();
        if (!HasAllComponents()) return;

        _fruitDropper.Initialize(_fruitPackedWeightedList);
        FruitMerger.InitFruitMerger(_fruitBasket, _fruitPackedList);
        Fruit.OnSameFruitTierCollision += Fruit_OnSameFruitTierCollision;
    }

    public override void _Process(double delta) {
        base._Process(delta);
        if (!HasAllComponents()) return;

        if (Input.IsActionJustPressed(StringInputs.DropFruit)) {
            _fruitDropper.TrySpawnWeightedFruit();
        }
    }

    private void Fruit_OnSameFruitTierCollision(object? sender, FruitPair e) {
        if (e.Fruit1.IsAttemptingMerge || e.Fruit2.IsAttemptingMerge) return;
        e.Fruit1.IsAttemptingMerge = true;
        e.Fruit2.IsAttemptingMerge = true;
        FruitMerger.ProcessMerge(e);
    }

    private bool HasAllComponents() {
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

        return true;
    }
}