using Godot;

namespace MergingFruits.Scripts;

public static class FruitMerger {
    private static PackedFruitList _packedFruitList;
    private static Node _fruitBasket;
    private static FruitPair _lastPair = new();

    public static void InitFruitMerger(Node fruitBasket, PackedFruitList packedFruitList) {
        _fruitBasket = fruitBasket;
        _packedFruitList = packedFruitList;
    }
    
    public static void ProcessMerge(FruitPair fruitPair) {
        if (CheckIfNewPairExisted(fruitPair)) return;
        _lastPair = fruitPair;
        var midPosition = (fruitPair.Fruit1.GlobalPosition + fruitPair.Fruit2.GlobalPosition) / 2;
        // GD.Print($"Deleted 'FruitPair' | P1:{fruitPair.Fruit1.GlobalPosition} | P2:{fruitPair.Fruit2.GlobalPosition} | NewP:{midPosition}");

        if (_packedFruitList is null) {
            GD.Load("FruitMerger.cs: FruitList is Missing");
            return;
        }
        var nextFruit = fruitPair.Fruit1.FruitTier + 1;
        fruitPair.Fruit1.GetParent().QueueFree();
        fruitPair.Fruit2.GetParent().QueueFree();
        if (nextFruit >= _packedFruitList.Data.Count) return;
        InstantiateFruit(_fruitBasket, _packedFruitList.Data[nextFruit], midPosition);
    }

    private static void InstantiateFruit(Node fruitBasket, PackedScene fruit, Vector2 position) {
        if (fruit is null) return;
        GD.Print("Instantiate()");
        //todo Instantiate specified Fruit
        var fruitNode = GD.Load<PackedScene>(fruit.ResourcePath);
        var fruitInstance = fruitNode.InstantiateOrNull<RigidBody2D>();
        if (fruitInstance is null) return;
        fruitBasket.AddChild(fruitInstance);
        fruitInstance.GlobalPosition = position;
        
        GD.Print($"Parent :) :{fruitInstance.GetParent()}");
    }

    private static bool CheckIfNewPairExisted(FruitPair comparePair) {
        if (_lastPair.Fruit1 is null || _lastPair.Fruit2 is null) return false;
        if (_lastPair == comparePair) return true;
        if (_lastPair.Fruit1 == comparePair.Fruit2 && _lastPair.Fruit2 == comparePair.Fruit1) return true;
        GD.Print("Pair Already Processed.");
        return false;
    }
}