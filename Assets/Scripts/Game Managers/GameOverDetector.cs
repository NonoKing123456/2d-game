using UnityEngine;

public class GameOverDetector : MonoBehaviour
{
    private Collider2D gameOverCollider;
    void Awake()
    {
        gameOverCollider = GetComponent<Collider2D>();
    }


    void OnTriggerExit2D(Collider2D other)
    {
        GameManager gameManager = GameManager.instance;
        if (gameManager == null || gameManager.CurrentState != GameManager.GameState.Playing) return;

        PlayerControl activePlayer = gameManager.CurrentPlayer;
        if (activePlayer == null || !activePlayer.isActiveAndEnabled) return;
        if (other.GetComponentInParent<PlayerControl>() != activePlayer) return;

        gameManager.GameOver();
    }
}
