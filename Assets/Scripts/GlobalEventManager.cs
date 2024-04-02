using UnityEngine.Events;

public class GlobalEventManager
{
    public static readonly UnityEvent OnPuzzleMoveFinished = new ();
    public static readonly UnityEvent OnPuzzleMoveStarted = new();
    public static readonly UnityEvent OnPuzzlesUpdate = new();
    public static readonly UnityEvent OnGameStopped = new();
    public static readonly UnityEvent OnGameStarted = new();
    
    public static void FinishPuzzlesMoving()
    {
        OnPuzzleMoveFinished.Invoke();
    }

    public static void StartPuzzleMoving()
    {
        OnPuzzleMoveStarted.Invoke();
    }

    public static void UpdatePuzzles()
    {
        OnPuzzlesUpdate.Invoke();
    }

    public static void StartGame()
    {
        OnGameStarted.Invoke();
    }

    public static void StopGame()
    {
        OnGameStopped.Invoke();
    }
}