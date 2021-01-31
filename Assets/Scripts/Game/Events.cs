using UnityEngine;

public class Events : MonoBehaviour
{
    [Header("Game Raised Events")]
    public GameEvent onGameStart;
    public GameEvent onLevelCompleted;
    public BoolEvent changeDeselectVisibility;

    [Header("UI Raised Events")]
    public GameEvent onUIDeselectRequest;

    // [Header("Gameplay events")]
    // public CoinDropEvent onCoinDropEnemyEvent;
    
    public static Events singleton { get; set; }

    private void Awake()
    {
        if (Events.singleton != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

        Events.singleton = this;
    }
}
