using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;
    
    bool _IsPaused;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_IsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
            
        }
        else if (_IsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnpauseGame();
            }
            
        }
    }
    public void PauseGame()
    {
        PausePanel.SetActive(true);
        _IsPaused = true;
        Time.timeScale = 0.0f;
        FirstPersonController.Instance.ResetLockstate();
    }

    public void UnpauseGame()
    {
        PausePanel.SetActive(false);
        _IsPaused = false;
        Time.timeScale = 1.0f;
        FirstPersonController.Instance.ResetLockstate();
    }


}
