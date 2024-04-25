using Godot;

namespace MergingFruits.Scripts.Fruits;

[GlobalClass]
public partial class FruitInfo : Resource {
    //todo MAKE THE FRUIT INFOS
    [Export] private Texture _fruitTexture;
    [Export] private PackedScene _packedScene;
    [Export(PropertyHint.Range, "0, 1, 0.01")] private float _weight;

    public Texture Texture => _fruitTexture;
    public PackedScene PackedScene  => _packedScene;
    public float Weight => _weight;
}