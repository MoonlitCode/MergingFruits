using Godot;
using Godot.Collections;

namespace MergingFruits.Scripts;

[GlobalClass]
public partial class PackedFruitList : Resource {
    [Export] private Array<PackedScene> _data;

    public Array<PackedScene> Data => _data;
}