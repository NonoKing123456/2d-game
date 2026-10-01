using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }
    [SerializeField] private GameState currentState = GameState.Playing;
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<Vector3> enemySpawnPoints;
    [SerializeField] private GameObject DeathPanel;
    public GameState CurrentState => currentState;
    public PlayerControl CurrentPlayer { get; private set; }
    private void Awake()
    {
        DeathPanel = GameObject.Find("DeathPanel");
        if (!ManageInstance()) return;
        ReloadScene1();
    }

    public void ReloadScene1()
    {
        Debug.Log("Reloading Scene 1");
        CancelInvoke(nameof(GameOver));
        CurrentPlayer = null;
        ClearExistingEnemiesAndPlayer();
        GameObject spawnedPlayer = SpawnPlayer();
        if (spawnedPlayer == null) return;

        CurrentPlayer = spawnedPlayer.GetComponent<PlayerControl>();
        BindSceneToPlayer(spawnedPlayer);
        SpawnEnemies(spawnedPlayer.transform);
        DeathPanel.SetActive(false);
        SetGameState(GameState.Playing);
    }

    private void ClearExistingEnemiesAndPlayer()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.CompareTag("Enemy") || obj.CompareTag("Player"))
            {
                obj.SetActive(false);
                Destroy(obj);
            }
        }
    }

    private GameObject SpawnPlayer()
    {
        if (player == null || spawnPoint == null)
        {
            Debug.LogError("GameManager requires a player prefab and spawn point.", this);
            return null;
        }

        return Instantiate(player, spawnPoint.position, Quaternion.identity);
    }

    private void BindSceneToPlayer(GameObject spawnedPlayer)
    {
        foreach (CameraFollow2D cameraFollow in FindObjectsByType<CameraFollow2D>())
        {
            cameraFollow.SetTarget(spawnedPlayer.transform);
        }

        PlayerLife playerLife = spawnedPlayer.GetComponent<PlayerLife>();
        foreach (PlayerHealthHUD hud in FindObjectsByType<PlayerHealthHUD>())
        {
            hud.SetPlayer(playerLife);
        }
    }

    private void SpawnEnemies(Transform playerTarget)
    {
        if (enemies == null || enemies.Count == 0 || enemySpawnPoints == null) return;

        for (int i = 0; i < enemySpawnPoints.Count; i++)
        {
            GameObject enemyPrefab = enemies[i % enemies.Count];
            if (enemyPrefab == null) continue;

            GameObject spawnedEnemy = Instantiate(enemyPrefab, enemySpawnPoints[i], Quaternion.identity);
            EnemyBrain brain = spawnedEnemy.GetComponent<EnemyBrain>();
            if (brain != null)
            {
                brain.SetPlayer(playerTarget);
            }
        }
    }
    


    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private bool ManageInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return false;
        }
        else
        {
            Instance = this;
            return true;
        }
    }

    
    private void OnDrawGizmos()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint.position, 0.3f);
        }
        if (enemySpawnPoints == null) return;

        Gizmos.color = Color.red;

        foreach (Vector3 point in enemySpawnPoints)
        {
            Gizmos.DrawWireSphere(point, 0.3f);
        }
    }
    public void GameOver()
    {
        SetGameState(GameState.GameOver);
        if (DeathPanel != null)
        {
            DeathPanel.SetActive(true);
        }
    }
    private void SetGameState(GameState newState)
    {
        currentState = newState;
    }
}
