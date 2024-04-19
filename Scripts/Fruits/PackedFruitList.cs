using Godot;
using Godot.Collections;

namespace MergingFruits.Scripts.Fruits;

[GlobalClass]
public partial class PackedFruitList : Resource {
    [Export] private Array<PackedScene> _data;

    public Array<PackedScene> Data => _data;
}