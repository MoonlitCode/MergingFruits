using Godot;

namespace MergingFruits.Scripts;

public partial class Game : Node {
    [Export] private PackedFruitList _packedFruitList;
    [Export] private Node _fruitBasket;
    
    public override void _Ready() {
        base._Ready();
        FruitMerger.InitFruitMerger(_fruitBasket, _packedFruitList);
        Fruit.OnFruitCollision += Fruit_OnFruitCollision;
    }

    private void Fruit_OnFruitCollision(object sender, FruitPair e) {
        FruitMerger.ProcessMerge(e);
    }
}