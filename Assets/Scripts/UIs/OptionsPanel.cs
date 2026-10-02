using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private Scrollbar masterVolumeSlider;
    [SerializeField] private Scrollbar musicVolumeSlider;
    [SerializeField] private Scrollbar sfxVolumeSlider;
    [SerializeField] private Toggle masterToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;
    

    private void OnEnable()
    {
        RefreshSettings();
    }
        private void RefreshSettings()
    {
        if (GameSession.Instance != null)
        {
            masterVolumeSlider.value = GameSession.Instance.GetMasterVolume();
            musicVolumeSlider.value = GameSession.Instance.GetMusicVolume();
            sfxVolumeSlider.value = GameSession.Instance.GetSfxVolume();
            masterToggle.isOn = GameSession.Instance.GetIsMasterEnabled();
            musicToggle.isOn = GameSession.Instance.GetIsMusicEnabled();
            sfxToggle.isOn = GameSession.Instance.GetIsSfxEnabled();
        }
    }
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
    public void QuitGame(){
        GameManager.instance.BackToMainMenu();
    }
    public void RestartGame(){
        GameManager.instance.RestartGame();
    }
}
