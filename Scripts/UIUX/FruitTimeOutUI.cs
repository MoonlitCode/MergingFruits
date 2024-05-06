using Godot;
using MergingFruits.Scripts.MoonDev;
using static MergingFruits.Scripts.MoonDev.MoonConstants;

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
			_textureProgressBar.TintProgress = _textureProgressBar.TintProgress.WithAlpha(1);
			_textureProgressBar.TintUnder = _textureProgressBar.TintUnder.WithAlpha(1);
			return;
		}

		if (_fadeTimer.CurrentTimer <= 0) return;
		_fadeTimer.DecrementCurrentTimer(delta);
		var timerNormalized = _fadeTimer.TimerNormalizedDecreasing();
		_textureProgressBar.TintProgress = _textureProgressBar.TintProgress.WithAlpha(timerNormalized);
		_textureProgressBar.TintUnder = _textureProgressBar.TintUnder.WithAlpha(timerNormalized);
	}

	public override void _Ready() {
		base._Ready();
		_fadeTimer.InitializeTimer(UIVariables.FadeTimerMax);
	}

	private bool HasAllComponents() {
		if (_textureProgressBar is null) {
			GD.PrintErr($"{GetType().Name} {IsMissingString} {nameof(_textureProgressBar)}");
			return false;
		}
		return true;
	}
}