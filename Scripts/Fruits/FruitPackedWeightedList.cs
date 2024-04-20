using Godot;
using Godot.Collections;

namespace MergingFruits.Scripts.Fruits;

[GlobalClass]
public partial class FruitPackedWeightedList : Resource {
    [Export] private Array<FruitPackedWeighted> _data;

    public Array<FruitPackedWeighted> Data => _data;
}