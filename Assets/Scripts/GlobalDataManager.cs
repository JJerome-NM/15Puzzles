using UnityEngine;

public class GlobalDataManager : MonoBehaviour
{
    public static float PuzzleMoveSpeed { get; private set; } = 10;
    public static bool IsPuzzleMoving { get; private set; }
    public static bool IsGameStopped { get; private set; }
    
    
    private void Start()
    {
        GlobalEventManager.OnPuzzleMoveStarted.AddListener(() => IsPuzzleMoving = true);
        GlobalEventManager.OnPuzzleMoveFinished.AddListener(() => IsPuzzleMoving = false);
        GlobalEventManager.OnGameStopped.AddListener(() => IsGameStopped = true);
        GlobalEventManager.OnGameStarted.AddListener(() => IsGameStopped = false);
    }
}
