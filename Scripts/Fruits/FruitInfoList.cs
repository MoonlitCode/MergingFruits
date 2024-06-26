﻿using Godot;
using Godot.Collections;

namespace MergingFruits.Scripts.Fruits;

//note This is a 'PackedScene' instead of a 'Fruit' to allow for a Resource.
//note Otherwise the 'Fruit's parent/'RigidBody' would need to be a different node type and mess up the rest of the hierarchy.
[GlobalClass]
public partial class FruitInfoList : Resource {
	[Export] private Array<FruitInfo> _data;

	/// <summary>
	/// 'Fruit' is the child of the root 'RigidBody2D'
	/// </summary>
	public Array<FruitInfo> Data => _data;
}