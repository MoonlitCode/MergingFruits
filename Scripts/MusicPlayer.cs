using Godot;

namespace MergingFruits.Scenes;

public partial class MusicPlayer : Node {
	[Export] private AudioStreamPlayer _audioStreamPlayer;

	public override void _EnterTree() {
		_audioStreamPlayer.Finished += AudioStreamPlayer_OnFinished;
	}

	private void AudioStreamPlayer_OnFinished() {
		_audioStreamPlayer.Play();
	}
}