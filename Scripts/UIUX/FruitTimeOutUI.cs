using Godot;
using MergingFruits.Scripts.MoonDev;

namespace MergingFruits.Scripts.UIUX;

public partial class FruitTimeOutUI : Control {
	[Export] private TextureProgressBar _textureProgressBar;

	private ClassTimer _fadeTimer = new();

	/// <summary>
	/// Updates the 'FruitDropper' progress wheel.
	/// <br/>Also fades it out.
	/// </summary>
	/// <param name="delta">Utilized to fade out the UI element.</param>
	/// <param name="progressBarValue">Should be an increasing value between 0 - 1.</param>
	/// <param name="hasFruitOutOfBounds">Used for the "Fade Out" check.</param>
	public void UpdateProgressBar(float delta, float progressBarValue, bool hasFruitOutOfBounds) {
		if (!HasAllComponents()) return;
		_textureProgressBar!.Value = progressBarValue;

		if (hasFruitOutOfBounds) {
			if (_textureProgressBar.TintProgress.A < 1) _fadeTimer.ResetTimer();
			_textureProgressBar.TintProgress = ColorManipulation.SetAlpha(_textureProgressBar.TintProgress, 1);
			_textureProgressBar.TintUnder = ColorManipulation.SetAlpha(_textureProgressBar.TintUnder, 1);
			return;
		}

		if (_fadeTimer.CurrentTimer <= 0) return;
		_fadeTimer.DecrementCurrentTimer(delta);
		var timerNormalized = _fadeTimer.TimerNormalizedDecreasing();
		_textureProgressBar.TintProgress =
			ColorManipulation.SetAlpha(_textureProgressBar.TintProgress, timerNormalized);
		_textureProgressBar.TintUnder = ColorManipulation.SetAlpha(_textureProgressBar.TintUnder, timerNormalized);
	}

	public override void _Ready() {
		base._Ready();
		_fadeTimer.InitializeTimer(UIVariables.FadeTimerMax);
	}

	private bool HasAllComponents() {
		if (_textureProgressBar is null) {
			GD.PrintErr($"FruitTimeOutUI.cs is Missing: _textureProgressBar");
			return false;
		}
		return true;
	}
}