using UnityEngine;
using System;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [Header("Sterilization")]
    [SerializeField, Range(0f, 1f)]
    private float _worldSterilizationLevel = 0f;

    public float WorldSterilizationLevel => _worldSterilizationLevel;

    // Other systems subscribe to this to react when sterilization changes
    public event Action<float> OnSterilizationChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddSterilization(float amount)
    {
        float previous = _worldSterilizationLevel;
        _worldSterilizationLevel = Mathf.Clamp01(_worldSterilizationLevel + amount);

        if (!Mathf.Approximately(previous, _worldSterilizationLevel))
            OnSterilizationChanged?.Invoke(_worldSterilizationLevel);
    }

    // Useful for checking AI phase thresholds cleanly
    public SterilizationPhase CurrentPhase => _worldSterilizationLevel switch
    {
        < 0.34f => SterilizationPhase.Aggressive,
        < 0.67f => SterilizationPhase.Fleeing,
        _ => SterilizationPhase.Cowering
    };
}

public enum SterilizationPhase
{
    Aggressive,
    Fleeing,
    Cowering
}