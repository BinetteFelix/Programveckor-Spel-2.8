using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [HideInInspector] public static ButtonController Instance;

    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject ControlsPanel;
    [SerializeField] GameObject AudioPanel;
    [SerializeField] GameObject InventoryPanel;

    bool _IsPaused;
    bool _SM_IsActive;
    bool _Cs_IsActive = false;
    bool _Ao_IsActive = true;
    bool _Inv_IsActive = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!_IsPaused && !_Inv_IsActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
        else if (_IsPaused && !_SM_IsActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnpauseGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenInventory();
        }
    }
    public void PauseGame()
    {
        PausePanel.SetActive(true);
        _IsPaused = true;
        Time.timeScale = 0.0f;
        FPSCameraController.Instance.ResetLockstate();
    }
    public void UnpauseGame()
    {
        if (!_SM_IsActive && _IsPaused)
        {
            PausePanel.SetActive(false);
            _IsPaused = false;
            Time.timeScale = 1.0f;
            FPSCameraController.Instance.ResetLockstate();
        }
    }
    public void OpenSettingsMenu()
    {
        SettingsMenu.SetActive(true);
        _SM_IsActive = true;
    }
    public void CloseSettingsMenu()
    {
        SettingsMenu.SetActive(false);
        _SM_IsActive = false;
    }
    public void ControlsSetting_Type()
    {
        if(_Ao_IsActive && !_Cs_IsActive)
        {
            ControlsPanel.SetActive(true);  
            _Cs_IsActive = true;
            AudioPanel.SetActive(false);
            _Ao_IsActive = false;
        }
    }
    public void AudioSetting_Type()
    {
        if(_Cs_IsActive && !_Ao_IsActive)
        {
            AudioPanel.SetActive(true);
            _Ao_IsActive = true;
            ControlsPanel.SetActive(false);
            _Cs_IsActive = false;
        }
    }
    public void OpenInventory()
    {
        if (!_IsPaused && !_Inv_IsActive)
        {
            InventoryManager.Instance.ArrangeItems();
            FPSCameraController.Instance.ResetLockstate();
            InventoryPanel.SetActive(true);
            _Inv_IsActive = true;
        }
    }
    public void CloseInventory()
    {
        if (_Inv_IsActive)
        {
            InventoryManager.Instance.ArrangeItems();
            FPSCameraController.Instance.ResetLockstate();
            InventoryPanel.SetActive(false);
            _Inv_IsActive = false;
        }
    }
}