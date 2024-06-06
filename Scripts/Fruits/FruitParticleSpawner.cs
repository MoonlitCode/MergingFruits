using Godot;

namespace MergingFruits.Scripts.Fruits;

public partial class FruitParticleSpawner : Node {
	[Export] private PackedScene _spawnParticles;
	[Export] private Node _particleBasket;

	public override void _Ready() {
		if (_spawnParticles is null || _particleBasket is null) return;
		FruitMerger.FruitMerged += FruitMerger_OnFruitMerged;
	}

	public override void _ExitTree() {
		if (_spawnParticles is null || _particleBasket is null) return;
		FruitMerger.FruitMerged -= FruitMerger_OnFruitMerged;
	}

	private void FruitMerger_OnFruitMerged(object sender, Vector2 globalPosition) {
		var particleInstance = _spawnParticles.InstantiateOrNull<GpuParticles2D>();
		if (particleInstance is null) return;

		particleInstance.ProcessMode = ProcessModeEnum.Disabled;
		_particleBasket.AddChild(particleInstance);
		particleInstance.GlobalPosition = globalPosition;
		particleInstance.ProcessMode = ProcessModeEnum.Always;
		particleInstance.Emitting = true;
		GD.Print("FruitSpawned: Spawn Particles");
	}
}