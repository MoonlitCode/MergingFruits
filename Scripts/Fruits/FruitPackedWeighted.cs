using Godot;

namespace MergingFruits.Scripts.Fruits;

[GlobalClass]
public partial class FruitPackedWeighted : Resource {
    [Export] private PackedScene _packedScene;
    [Export(PropertyHint.Range, "0, 1, 0.01")] private float _weight;

    public PackedScene FruitScene => _packedScene;
    public float Weight => _weight;
}