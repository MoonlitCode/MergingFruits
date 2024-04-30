using System;

namespace MergingFruits.Scripts.MoonDev;

public class ClassTimer {
    public event EventHandler<ClassTimer> OnTimerEnded;
    
    private float _maxTimer;
    private float _currentTimer;
    private bool _hasTimerEnded;

    public float MaxTimer => _maxTimer;
    public float CurrentTimer => _currentTimer;
    public bool HasTimerEnded => _hasTimerEnded;

    /// <summary>
    /// Used to display on a "Progress bar".
    /// </summary>
    /// <returns>Value goes from 1 to 0</returns>
    public float TimerNormalizedDecreasing() {
        if (_currentTimer == 0) return 0;
        return _currentTimer / MaxTimer;
    }

    /// <summary>
    /// Used to display on a "Progress bar".
    /// </summary>
    /// <returns>Value goes from 0 to 1</returns>
    public float TimerNormalizedIncreasing() {
        var timerValue = 1 - TimerNormalizedDecreasing();
        if (timerValue > 1) return 1;
        return timerValue;
    }

    /// <summary>
    /// Sets the 'MaxTimer'
    /// </summary>
    /// <param name="maxTimer"></param>
    public void InitializeTimer(float maxTimer) {
        _maxTimer = maxTimer;
    }

    /// <summary>
    /// Sets the "Current timer" to 'MaxTimer'.
    /// </summary>
    public void ResetTimer() {
        _currentTimer = MaxTimer;
        _hasTimerEnded = false;
    }

    /// <summary>
    /// Stops the "Current Timer"
    /// </summary>
    public void StopTimer() {
        _currentTimer = 0;
        _hasTimerEnded = true;
    }
    
    /// <summary>
    /// Increments the "Current timer".
    /// </summary>
    /// <param name="amount">Uses "Delta" time typically.</param>
    public void IncrementCurrentTimer(float amount) {
        _currentTimer += amount;
        
        if (_currentTimer > MaxTimer) return;
        _currentTimer = MaxTimer;
    }

    /// <summary>
    /// Decrements the "Current timer".
    /// </summary>
    /// <param name="amount">Uses "Delta" time typically.</param>
    public void DecrementCurrentTimer(float amount) {
        _currentTimer -= amount;
        
        if (_currentTimer > 0 || _hasTimerEnded) return;
        _currentTimer = 0;
        _hasTimerEnded = true;
        OnTimerEnded?.Invoke(this, this);
    }
    
    /// <summary>
    /// Decrements the "Current timer".
    /// </summary>
    /// <param name="amount">Uses "Delta" time typically.</param>
    public void DecrementCurrentTimer(double amount) => DecrementCurrentTimer((float)amount);
}