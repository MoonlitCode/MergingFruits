using Godot;

namespace MergingFruits.Scripts;

public partial class Game : Node {
    [Export] private PackedFruitList _packedFruitList;
    [Export] private Node _fruitBasket;
    
    public override void _Ready() {
        base._Ready();
        FruitMerger.InitFruitMerger(_fruitBasket, _packedFruitList);
        Fruit.OnSameFruitTierCollision += Fruit_OnSameFruitTierCollision;
    }

    private void Fruit_OnSameFruitTierCollision(object sender, FruitPair e) {
        if (e.Fruit1.IsAttemptingMerge || e.Fruit2.IsAttemptingMerge) return;
        e.Fruit1.IsAttemptingMerge = true;
        e.Fruit2.IsAttemptingMerge = true;
        FruitMerger.ProcessMerge(e);
    }
}