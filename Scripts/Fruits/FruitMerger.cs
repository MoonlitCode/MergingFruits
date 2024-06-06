using Godot;

namespace MergingFruits.Scripts.Fruits;

public static class FruitMerger {
	private static FruitInfoList _fruitInfoList;
	private static Node2D _fruitBasket;
	private static FruitPair _lastPair = new();

	public static void Initialize(Node2D fruitBasket, FruitInfoList fruitInfoList) {
		_fruitBasket = fruitBasket;
		_fruitInfoList = fruitInfoList;
	}

	public static void ProcessMerge(FruitPair fruitPair) {
		if (CheckIfNewPairExisted(fruitPair)) return;
		_lastPair = fruitPair;
		var midPosition = (fruitPair.Fruit1.GlobalPosition + fruitPair.Fruit2.GlobalPosition) / 2;
		var midRotation = (fruitPair.Fruit1.GlobalRotation + fruitPair.Fruit2.GlobalRotation) / 2;
		if (_fruitInfoList is null) {
			GD.Load("FruitMerger.cs: FruitList is Missing");
			return;
		}

		var nextFruit = fruitPair.Fruit1.FruitTier + 1;
		fruitPair.Fruit1Root.QueueFree();
		fruitPair.Fruit2Root.QueueFree();
		if (nextFruit >= _fruitInfoList.Data.Count) return;
		FruitSpawner.RB2DInstantiateOrNull(_fruitBasket, _fruitInfoList.Data[nextFruit].PackedScene, midPosition, midRotation);
	}

	private static bool CheckIfNewPairExisted(FruitPair comparePair) {
		if (_lastPair.Fruit1 is null || _lastPair.Fruit2 is null) return false;
		if (_lastPair == comparePair) return true;
		if (_lastPair.Fruit1 == comparePair.Fruit2 && _lastPair.Fruit2 == comparePair.Fruit1) return true;
		return false;
	}
}