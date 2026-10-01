using UnityEngine;

public class OptionsPanel : MonoBehaviour
{
    public void CloseTab()
    {
        gameObject.SetActive(false);
        if (GameManager.instance != null)
        {
            GameManager.instance.ContinueGame();
        }
    }
    public void SetMasterVolume(float volume)
    {
        GameSession.Instance.SetMasterVolume(volume);
    }
    public void SetMusicVolume(float volume)
    {
        GameSession.Instance.SetMusicVolume(volume);    
    }
    public void SetSfxVolume(float volume)
    {
        GameSession.Instance.SetSfxVolume(volume);
    }
    public void SetIsMasterEnabled(bool isEnabled)
    {
        GameSession.Instance.SetIsMasterEnabled(isEnabled);
    }
    public void SetIsMusicEnabled(bool isEnabled)
    {
        GameSession.Instance.SetIsMusicEnabled(isEnabled);
    }
    public void SetIsSfxEnabled(bool isEnabled)
    {
        GameSession.Instance.SetIsSfxEnabled(isEnabled);
    }
}
