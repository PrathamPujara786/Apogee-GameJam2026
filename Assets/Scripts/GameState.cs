using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [Range(0f, 1f)]
    public float WorldSterilizationLevel { get; private set; }

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

    /// <summary>
    /// Called by the AoE system each time Sterilization fires.
    /// Amount should be normalized 0–1 (e.g. 0.05 per AoE use).
    /// </summary>
    public void AddSterilization(float amount)
    {
        WorldSterilizationLevel = Mathf.Clamp01(WorldSterilizationLevel + amount);
    }
}