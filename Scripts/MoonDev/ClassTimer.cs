using System;

namespace MergingFruits.Scripts.MoonDev;

public class ClassTimer {
    public event EventHandler<ClassTimer> OnTimerEnded;
    
    public float MaxTimer;
    private float _currentTimer;
    private bool _hasTimerEnded;

    public float CurrentTimer => _currentTimer;
    public bool HasTimerEnded => _hasTimerEnded;

    public float TimerNormalizedDecreasing() {
        if (_currentTimer == 0) return 0;
        return _currentTimer / MaxTimer;
    }

    public float TimerNormalizedIncreasing() {
        var timerValue = 1 - TimerNormalizedDecreasing();
        if (timerValue > 1) return 1;
        return timerValue;
    }

    public void InitializeTimer(float maxTimer) {
        MaxTimer = maxTimer;
    }

    public void ResetTimer() {
        _currentTimer = MaxTimer;
        _hasTimerEnded = false;
    }

    public void IncrementCurrentTimer(float amount) {
        _currentTimer += amount;
        
        if (_currentTimer > MaxTimer) return;
        _currentTimer = MaxTimer;
    }

    public void DecrementCurrentTimer(float amount) {
        _currentTimer -= amount;
        
        if (_currentTimer > 0 || _hasTimerEnded) return;
        _currentTimer = 0;
        _hasTimerEnded = true;
        OnTimerEnded?.Invoke(this, this);
    }
}