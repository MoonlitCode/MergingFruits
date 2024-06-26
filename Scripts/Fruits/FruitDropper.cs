#nullable enable
using Godot;
using MergingFruits.Scripts.MoonDev;
using MergingFruits.Scripts.UIUX;
using static MergingFruits.Scripts.MoonDev.MoonConstants;

namespace MergingFruits.Scripts.Fruits;

public partial class FruitDropper : Area2D {
	[Export] private Node? _targetParent;
	[Export] private Sprite2D? _dropperSprite;
	[Export] private Node2D? _dropPosition;
	[Export] private TextureProgressBar? _progressBar;
	[Export] private CollisionShape2D? _collisionShape2D;
	[Export] private float _dropperSpeed = 15;

	private RigidBody2D? _fruitInstance;
	private ClassTimer _fadeTimer = new();

	public Vector2 DropGlobalPosition => _dropPosition?.GlobalPosition ?? Vector2.Zero;

	public override void _Ready() {
		base._Ready();
		_fadeTimer.InitializeTimer(UIVariables.FadeTimerMax);
	}

	public override void _Process(double delta) {
		base._Process(delta);
		ProcessFruitPosition();
	}

	/// <summary>
	/// Attempts to immediately reposition dropper. To be used with Mouse control schemes.
	/// </summary>
	/// <param name="globalPosition"></param>
	public void TryRepositionDropper(Vector2 globalPosition) {
		if (!HasAllComponents()) return;
		
		var colliderSize = _collisionShape2D!.Shape.GetRect().Size.X / 2;
		if (!(Mathf.Abs(globalPosition.X) < colliderSize)) return;
		_dropperSprite!.GlobalPosition = new Vector2(globalPosition.X, _dropperSprite.GlobalPosition.Y);
	}

	/// <summary>
	/// Attempts to translate dropper over time. To be used with Button control schemes.
	/// </summary>
	/// <param name="direction"></param>
	public void TryTranslatingDropper(Vector2 direction) {
		if (!HasAllComponents()) return;

		var colliderSize = _collisionShape2D!.Shape.GetRect().Size.X / 2;
		var positionOffset = direction * _dropperSpeed;
		
		var newPosition = new Vector2(_dropperSprite!.GlobalPosition.X + positionOffset.X, _dropperSprite.GlobalPosition.Y);
		
		_dropperSprite.GlobalPosition = newPosition;
		if (newPosition.X < -colliderSize)
			_dropperSprite.GlobalPosition = new Vector2(-colliderSize, newPosition.Y);
		if (newPosition.X > colliderSize)
			_dropperSprite.GlobalPosition = new Vector2(colliderSize, newPosition.Y);
	}

	/// <summary>
	/// Updates the 'FruitDropper' progress wheel.
	/// <br/>Also fades it out.
	/// </summary>
	/// <param name="delta">Utilized to fade out the UI element.</param>
	/// <param name="progressBarValue">Should be an increasing value between 0 - 1.</param>
	public void UpdateProgressBar(float delta, float progressBarValue) {
		if (!HasAllComponents()) return;
		_progressBar!.Value = progressBarValue;
		
		if (progressBarValue < 1 && !_fadeTimer.HasTimerEnded) {
			_progressBar.TintProgress = _progressBar.TintProgress.WithAlpha(1);
			_progressBar.TintUnder = _progressBar.TintUnder.WithAlpha(1);
			return;
		}

		if (progressBarValue >= 1 && _fadeTimer.HasTimerEnded) _fadeTimer.ResetTimer();
		if (_progressBar.TintProgress.A <= 0) return;

		_fadeTimer.DecrementCurrentTimer(delta);
		var timerNormalized = _fadeTimer.TimerNormalizedDecreasing();
		_progressBar.TintProgress = _progressBar.TintProgress.WithAlpha(timerNormalized);
		_progressBar.TintUnder = _progressBar.TintUnder.WithAlpha(timerNormalized);
	}

	private void ProcessFruitPosition() {
		if (_fruitInstance is null) return;
		_fruitInstance.GlobalPosition = DropGlobalPosition;
	}
	
	public void SetFruitInstance(RigidBody2D fruitInstance) {
		if (_fruitInstance is not null) TryReleaseFruit();
		_fruitInstance = fruitInstance;
		_fruitInstance.ProcessMode = ProcessModeEnum.Disabled;
	}

	public void TryReleaseFruit() {
		if (_fruitInstance is null) return;
		_fruitInstance.ProcessMode = ProcessModeEnum.Always;
		_fruitInstance = null;
	}

	public bool HasFruit() => _fruitInstance is not null;
	
	private bool HasAllComponents() {
		if (_dropperSprite is null) {
			GD.PrintErr($"{GetType().Name} {IsMissingString} {nameof(_dropperSprite)}");
			return false;
		}
		if (_targetParent is null) {
			GD.PrintErr($"{GetType().Name} {IsMissingString} {nameof(_targetParent)}");
			return false;
		}
		if (_dropPosition is null) {
			GD.PrintErr($"{GetType().Name} {IsMissingString} {nameof(_dropPosition)}");
			return false;
		}
		if (_progressBar is null) {
			GD.PrintErr($"{GetType().Name} {IsMissingString} {nameof(_progressBar)}");
			return false;
		}
		if (_collisionShape2D is null) {
			GD.PrintErr($"{GetType().Name} {IsMissingString} {nameof(_collisionShape2D)}");
			return false;
		}

		return true;
	}
}
