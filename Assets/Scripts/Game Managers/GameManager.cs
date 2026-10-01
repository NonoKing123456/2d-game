using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }
    [SerializeField] private GameState currentState = GameState.Playing;
    private PlayerInput playerInput;
    private InputAction escInput;
    public static GameManager instance { get; private set; }
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<Vector3> enemySpawnPoints;
    [SerializeField] private GameObject DeathPanel;
    [SerializeField] private GameObject OptionsPanel;
    public GameState CurrentState => currentState;
    public PlayerControl CurrentPlayer { get; private set; }
    private void Awake()
    {
        OptionsPanel = GameObject.Find("Options Panel");
        // Only the spawned player should pair devices and own the Player action map.
        PlayerInput managerInput = GetComponent<PlayerInput>();
        if (managerInput != null) managerInput.enabled = false;
        DeathPanel = GameObject.Find("DeathPanel");
        if (!ManageInstance()) return;
        ReloadScene1();
    }

    public void ReloadScene1()
    {
        Debug.Log("Reloading Scene 1");
        CancelInvoke(nameof(GameOver));
        CurrentPlayer = null;
        escInput = null;
        ClearExistingEnemiesAndPlayer();
        GameObject spawnedPlayer = SpawnPlayer();
        if (spawnedPlayer == null) return;

        CurrentPlayer = spawnedPlayer.GetComponent<PlayerControl>();
        playerInput = spawnedPlayer.GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Player");
        OptionsPanel.SetActive(false);
        escInput = playerInput.actions.FindAction("Esc", true);
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
    public void ContinueGame()
    {
        SetGameState(GameState.Playing);
    }


    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (escInput != null && escInput.WasPressedThisFrame())
        {
            if (currentState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
            }
            else if (currentState == GameState.Paused)
            {
                ContinueGame();
            }
        }
        
    }
    private bool ManageInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return false;
        }
        else
        {
            instance = this;
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
        if (newState == GameState.Paused)
        {
            Time.timeScale = 0;
            OptionsPanel.SetActive(true);
        }
        if (newState == GameState.Playing)
        {
            Time.timeScale = 1;
            OptionsPanel.SetActive(false);
        }
    }
}
