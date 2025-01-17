using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject AudioPanel;
    [SerializeField] GameObject ControlsPanel;
    [SerializeField] GameObject Buttons;

    bool _SM_IsActive;
    bool _Cs_IsActive = false;
    bool _Ao_IsActive = true;
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    public void OpenSettingsMenu()
    {
        SettingsMenu.SetActive(true);
        Buttons.SetActive(false);
        _SM_IsActive = true;
    }
    public void CloseSettingsMenu()
    {
        SettingsMenu.SetActive(false);
        Buttons.SetActive(true);
        _SM_IsActive = false;
    }
    public void ControlsSetting_Type()
    {
        if (_Ao_IsActive && !_Cs_IsActive)
        {
            ControlsPanel.SetActive(true);
            _Cs_IsActive = true;
            AudioPanel.SetActive(false);
            _Ao_IsActive = false;
        }
    }
    public void AudioSetting_Type()
    {
        if (_Cs_IsActive && !_Ao_IsActive)
        {
            AudioPanel.SetActive(true);
            _Ao_IsActive = true;
            ControlsPanel.SetActive(false);
            _Cs_IsActive = false;
        }
    }

}
