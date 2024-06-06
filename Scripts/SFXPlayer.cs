using Godot;
using Godot.Collections;

namespace MergingFruits.Scripts;

public partial class SFXPlayer : Node {
	public static SFXPlayer Instance;
	
	[Export] private Array<AudioStreamPlayer> _audioStreamPlayers;

	public override void _EnterTree() => Instance ??= this;

	public override void _ExitTree() {
		if (Instance == this) Instance = null;
	}

	public void PlaySound(AudioStream audioStream) {
		if (_audioStreamPlayers is null || _audioStreamPlayers.Count == 0) return;

		foreach (var audioStreamPlayer in _audioStreamPlayers) {
			if (audioStreamPlayer.Playing) continue;
			if (audioStreamPlayer.Stream != audioStream) audioStreamPlayer.Stream = audioStream;
			audioStreamPlayer.Play();
			return;
		}

		_audioStreamPlayers[^1].Stream = audioStream;
		_audioStreamPlayers[^1].Play();
	}
}