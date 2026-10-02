using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance { get; private set; }
    [SerializeField] private GameObject optionsMenu;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        optionsMenu.SetActive(false);
    }
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseOptions();
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }
    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }
}
