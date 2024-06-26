﻿#nullable enable
using Godot;
using static MergingFruits.Scripts.MoonDev.MoonConstants;

namespace MergingFruits.Scripts.UIUX;

public partial class ScoreUI : Control {
	[Export] private RichTextLabel? _scoreTextLabel;

	public override void _Ready() {
		base._Ready();
		UpdateScore(0);
	}

	public void UpdateScore(string scoreText, bool shouldCenterText = true) {
		if (!HasAllComponents()) return;
		_scoreTextLabel!.Text = shouldCenterText ? $"[center]{scoreText}" : scoreText;
	}

	public void UpdateScore(int scoreInt, bool shouldCenterText = true) =>
		UpdateScore(scoreInt.ToString(), shouldCenterText);

	private bool HasAllComponents() {
		if (_scoreTextLabel is not null) return true;
		GD.PrintErr($"{GetType().Name} {IsMissingString} {nameof(_scoreTextLabel)}");
		return false;
	}
}