#nullable enable
using Godot;
using MergingFruits.Scripts.MoonDev;

namespace MergingFruits.Scripts.Fruits;

public partial class FruitDropper : Area2D {
	[Export] private Node? _targetParent;
	[Export] private Sprite2D? _dropperSprite;
	[Export] private Node2D? _dropPosition;
	[Export] private TextureProgressBar? _progressBar;
	[Export] private CollisionShape2D? _collisionShape2D;
	[Export] private float _dropperSpeed = 15;

	private RigidBody2D _fruitInstance;
	private ClassTimer _progressBarFadeTimer = new();

	public Vector2 DropGlobalPosition => _dropPosition?.GlobalPosition ?? Vector2.Zero;

	public override void _Ready() {
		base._Ready();
		_progressBarFadeTimer.InitializeTimer(0.2f);
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
		
		var colliderSize = _collisionShape2D.Shape.GetRect().Size.X / 2;
		if (!(Mathf.Abs(globalPosition.X) < colliderSize)) return;
		_dropperSprite.GlobalPosition = new Vector2(globalPosition.X, _dropperSprite.GlobalPosition.Y);
	}

	/// <summary>
	/// Attempts to translate dropper over time. To be used with Button control schemes.
	/// </summary>
	/// <param name="direction"></param>
	public void TryTranslatingDropper(Vector2 direction) {
		if (!HasAllComponents()) return;

		var colliderSize = _collisionShape2D.Shape.GetRect().Size.X / 2;
		var positionOffset = direction * _dropperSpeed;
		
		var newPosition = new Vector2(_dropperSprite.GlobalPosition.X + positionOffset.X, _dropperSprite.GlobalPosition.Y);
		
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
		_progressBar.Value = progressBarValue;
		
		var progressColor = _progressBar.TintProgress;
		var underColor = _progressBar.TintUnder;
		var newProgressColor = new Color(progressColor.R, progressColor.G, progressColor.B);
		var newUnderColor = new Color(underColor.R, underColor.G, underColor.B);

		if (progressBarValue < 1 ) {
			_progressBar.TintProgress = newProgressColor;
			_progressBar.TintUnder = newUnderColor;
			return;
		}

		if (progressBarValue >= 1 && _progressBarFadeTimer.HasTimerEnded) _progressBarFadeTimer.ResetTimer();
		if (_progressBar.TintProgress.A <= 0) return;

		_progressBarFadeTimer.DecrementCurrentTimer(delta);
		newProgressColor = new Color(progressColor.R, progressColor.G, progressColor.B,_progressBarFadeTimer.TimerNormalizedDecreasing());
		newUnderColor = new Color(underColor.R, underColor.G, underColor.B,_progressBarFadeTimer.TimerNormalizedDecreasing());
		_progressBar.TintProgress = newProgressColor;
		_progressBar.TintUnder = newUnderColor;
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
			GD.PrintErr($"FruitDropper.cs is Missing: _dropperSprite");
			return false;
		}
		if (_targetParent is null) {
			GD.PrintErr($"FruitDropper.cs is Missing: _targetParent");
			return false;
		}
		if (_dropPosition is null) {
			GD.PrintErr($"FruitDropper.cs is Missing: _dropPosition");
			return false;
		}
		if (_progressBar is null) {
			GD.PrintErr($"FruitDropper.cs is Missing: _progressBar");
			return false;
		}
		if (_collisionShape2D is null) {
			GD.PrintErr($"FruitDropper.cs is Missing: _collisionShape2D");
			return false;
		}

		return true;
	}
}
