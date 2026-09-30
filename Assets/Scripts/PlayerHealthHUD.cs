using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthHUD : MonoBehaviour
{
    [SerializeField] private PlayerLife playerLife;
    [SerializeField] private Image heartPrefab;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Vector2 heartInitialPosition = new Vector2(108f, -24f);
    [SerializeField] private float heartSpacing = 48f;

    private Image[] hearts;
    private int displayedHealth = -1;

    public void SetPlayer(PlayerLife newPlayerLife)
    {
        playerLife = newPlayerLife;
    }

    private void Start()
    {
        if (playerLife == null || heartPrefab == null || fullHeart == null || emptyHeart == null)
        {
            Debug.LogError("PlayerHealthHUD is missing a reference.", this);
            return;
        }

        int heartCount = Mathf.Max(0, playerLife.MaxHealth);
        hearts = new Image[heartCount];

        for (int i = 0; i < heartCount; i++)
        {
            Image heart = Instantiate(heartPrefab, transform);
            heart.name = "Heart_" + (i + 1);
            heart.rectTransform.anchoredPosition = heartInitialPosition + new Vector2(i * heartSpacing, 0f);
            heart.sprite = fullHeart;
            hearts[i] = heart;
        }
    }

    private void Update()
    {
        if (playerLife == null || hearts == null) return;

        int health = Mathf.Clamp(playerLife.CurrentHealth, 0, hearts.Length);
        if (health == displayedHealth) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < health ? fullHeart : emptyHeart;
        }

        displayedHealth = health;
    }
}
