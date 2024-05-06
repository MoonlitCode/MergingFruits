#nullable enable
using Godot;
using static MergingFruits.Scripts.MoonDev.MoonConstants;

namespace MergingFruits.Scripts.Fruits;

public static class FruitPicker {
	private static FruitInfoList? _fruitPackedWeightedList;

	public static void Initialize(FruitInfoList fruitPackedWeightedList) {
		_fruitPackedWeightedList = fruitPackedWeightedList;
	}

	/// <summary>
	/// Spawns a specified fruit according to assigned 'FruitPackedWeightedList'.
	/// <br/>This method will get the lowest "list index" if the given "index" is too low.
	/// <br/>This method will get the highest "list index" if the given "index" is too high.
	/// </summary>
	/// <param name="index">The desired fruit index to spawn</param>
	public static PackedScene? TryGetFruitScene(int index) {
		if (!HasAllComponents()) return null;
		if (index > _fruitPackedWeightedList.Data.Count) index -= 1;
		if (index < 0) index = 0;
		return _fruitPackedWeightedList.Data[index].PackedScene;
	}

	/// <summary>
	/// Utilizes 'TrySpawnSpecificFruit(int index)'
	/// <br/>Spawns a specified fruit according to assigned 'FruitPackedWeightedList'.
	/// <br/>This method will get the lowest "list index" if the given "index" is too low.
	/// <br/>This method will get the highest "list index" if the given "index" is too high.
	/// </summary>
	public static int GetWeightedIndex() {
		if (!HasAllComponents()) return 0;

		var fruitIndex = _fruitPackedWeightedList.Data.Count - 1;
		for (var i = fruitIndex; i >= 0; i--) {
			var fruitWeight = _fruitPackedWeightedList.Data[i].Weight;
			var randomRoll = GD.Randf();
			if (fruitWeight >= randomRoll) return i;
		}

		return 0;
	}

	private static bool HasAllComponents() {
		if (_fruitPackedWeightedList is null) {
			GD.PrintErr($"FruitPicker.cs {IsMissingString} {nameof(_fruitPackedWeightedList)}");
			return false;
		}

		return true;
	}
}