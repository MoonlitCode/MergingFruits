using Godot;

namespace MergingFruits.Scripts.Fruits;

public static class FruitMerger {
    private static FruitPackedList _fruitPackedList;
    private static Node _fruitBasket;
    private static FruitPair _lastPair = new();

    public static void Initialize(Node fruitBasket, FruitPackedList fruitPackedList) {
        _fruitBasket = fruitBasket;
        _fruitPackedList = fruitPackedList;
    }
    
    public static void ProcessMerge(FruitPair fruitPair) {
        if (CheckIfNewPairExisted(fruitPair)) return;
        _lastPair = fruitPair;
        var midPosition = (fruitPair.Fruit1.GlobalPosition + fruitPair.Fruit2.GlobalPosition) / 2;
        if (_fruitPackedList is null) {
            GD.Load("FruitMerger.cs: FruitList is Missing");
            return;
        }
        var nextFruit = fruitPair.Fruit1.FruitTier + 1;
        fruitPair.Fruit1Root.QueueFree();
        fruitPair.Fruit2Root.QueueFree();
        if (nextFruit >= _fruitPackedList.Data.Count) return;
        FruitSpawner.RB2DInstantiateOrNull(_fruitBasket, _fruitPackedList.Data[nextFruit], midPosition);
    }

    private static bool CheckIfNewPairExisted(FruitPair comparePair) {
        if (_lastPair.Fruit1 is null || _lastPair.Fruit2 is null) return false;
        if (_lastPair == comparePair) return true;
        if (_lastPair.Fruit1 == comparePair.Fruit2 && _lastPair.Fruit2 == comparePair.Fruit1) return true;
        return false;
    }
}