using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject mainUiPanel;
    
    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button stopButton;
    
    
    void Start()
    {
        winText.SetActive(false);
        mainUiPanel.SetActive(true);
        stopButton.gameObject.SetActive(false);

        GlobalEventManager.OnGameStopped.AddListener(OnGameStopped);
        GlobalEventManager.OnGameStarted.AddListener(OnGameStarted);
        
        restartButton.onClick.AddListener(RestartGame);
        stopButton.onClick.AddListener(GlobalEventManager.StopGame);
    }

    private void OnGameStopped()
    {
        winText.SetActive(true);
        mainUiPanel.SetActive(true);
        stopButton.gameObject.SetActive(false);
    }

    private void OnGameStarted()
    {
        stopButton.gameObject.SetActive(true);
        mainUiPanel.SetActive(false);
    }

    public void RestartGame()
    {
        GlobalEventManager.StopGame();
        GlobalEventManager.StartGame();
    }
}
