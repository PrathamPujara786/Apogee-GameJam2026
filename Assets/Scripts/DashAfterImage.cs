using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class DashAfterimage : MonoBehaviour
{
    [Header("Afterimage Settings")]
    [SerializeField] private GameObject afterimagePrefab;
    [SerializeField] private float spawnInterval = 0.03f;

    private PlayerMovement _player;
    private SpriteRenderer _playerSprite;
    private float _spawnTimer;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
        _playerSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!_player.IsDashing)
        {
            _spawnTimer = 0f; // reset so first afterimage spawns instantly on next dash
            return;
        }

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer > 0f) return;

        _spawnTimer = spawnInterval;
        SpawnAfterimage();
    }

    private void SpawnAfterimage()
    {
        if (GameState.Instance == null) return;

        float sterilization = GameState.Instance.WorldSterilizationLevel;
        if (sterilization >= 1f) return;

        if (afterimagePrefab == null)
        {
            Debug.LogWarning("DashAfterimage: afterimagePrefab is not assigned.");
            return;
        }

        GameObject img = Instantiate(afterimagePrefab, transform.position, transform.rotation);
        SpriteRenderer sr = img.GetComponent<SpriteRenderer>();

        if (sr != null && _playerSprite != null)
        {
            sr.sprite = _playerSprite.sprite;
            sr.sortingLayerName = _playerSprite.sortingLayerName;
            sr.sortingOrder = _playerSprite.sortingOrder - 1; // renders just behind player

            Color c = sr.color;
            c.a = Mathf.Lerp(0.6f, 0f, sterilization);
            sr.color = c;
        }

        Destroy(img, 0.12f);
    }
}