using Godot;

namespace MergingFruits.Scripts.Fruits;

public class FruitPair {
    public Fruit Fruit1;
    public Fruit Fruit2;

    public Node Fruit1Root => Fruit1.GetParent();
    public Node Fruit2Root => Fruit2.GetParent();
}