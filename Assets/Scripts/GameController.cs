using System.Collections.Generic;
using Puzzel;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static float _maxDistance = 1.25f;

    private static List<Puzzle> _puzzlesPatterns = new()
    {
        { new Puzzle(1, new Color(1f, 0.22f, 0.24f)) },
        { new Puzzle(2, new Color(1f, 0.16f, 0.49f)) },
        { new Puzzle(3, new Color(0.78f, 0.19f, 1f)) },
        { new Puzzle(4, new Color(0.46f, 0.27f, 1f)) },
        { new Puzzle(5, new Color(0.31f, 0.74f, 1f)) },
        { new Puzzle(6, new Color(0.26f, 1f, 0.46f)) },
        { new Puzzle(7, new Color(1f, 0.83f, 0.25f)) },
        { new Puzzle(8, new Color(1f, 0.65f, 0.18f)) },
        { new Puzzle(9, new Color(0.74f, 0.54f, 1f)) },
        { new Puzzle(10, new Color(0.12f, 0.26f, 1f)) },
        { new Puzzle(11, new Color(0.32f, 1f, 0f)) },
        { new Puzzle(12, new Color(0f, 1f, 0.72f)) },
        { new Puzzle(13, new Color(0.53f, 0.69f, 1f)) },
        { new Puzzle(14, new Color(0.88f, 0.95f, 1f)) },
        { new Puzzle(15, new Color(0.97f, 0.67f, 1f)) },
    };

    [Header("Game")] 
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject puzzlePrefab;
    [SerializeField] private GameObject puzzlesParent;
    [SerializeField] private LayerMask whatIsPuzzle;
    
    private GridController _gridController;
    private List<GridPositionController> _gridPositionControllers;
    private List<GameObject> _puzzles = new();
    private List<Vector3> _availableDirections = new() { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    private bool isPuzzleReady = false;
    private int shakingCount = 0;
    
    void Start()
    {
        _gridPositionControllers = new List<GridPositionController>();

        _gridController = grid.GetComponent<GridController>();

        _gridController.GetLineControllers().ForEach(o =>
        {
            _gridPositionControllers.AddRange(o.GetPositionControllers());
        });

        GlobalEventManager.OnGameStarted.AddListener(GeneratePuzzles);
        GlobalEventManager.OnGameStopped.AddListener(ClearGameBoard);
        GlobalEventManager.OnPuzzleMoveFinished.AddListener(CheckPuzzlesInCompletedPosition);
    }


    private void FixedUpdate()
    {
        if (GlobalDataManager.IsPuzzleMoving)
        {
            GlobalEventManager.UpdatePuzzles();
        }

        if (isPuzzleReady && shakingCount < 15)
        {
            ShakePuzzles();
        }
        else
        {
            isPuzzleReady = false;
        }
    }

    private void ClearGameBoard()
    {
        foreach (Transform child in puzzlesParent.transform)
        {
            Destroy(child.gameObject);
        }

        _puzzles.Clear();

        isPuzzleReady = false;
    }

    private void GeneratePuzzles()
    {
        _gridPositionControllers.ForEach(p =>
        {
            if (_puzzlesPatterns.Count > _puzzles.Count)
            {
                GameObject newPuzzle = Instantiate(puzzlePrefab, p.GetCenterPosition(), Quaternion.identity);
                newPuzzle.transform.SetParent(puzzlesParent.transform);

                PuzzleController puzzleController = newPuzzle.GetComponent<PuzzleController>();
                puzzleController.SetPuzzle(_puzzlesPatterns[_puzzles.Count]);

                _puzzles.Add(newPuzzle);
            }
        });
        isPuzzleReady = true;
        shakingCount = 0;
    }

    
    private void ShakePuzzles()
    {
        Vector3 emptyPosition = GetEmptyPosition();

        if (emptyPosition == Vector3.zero) return;
        
        List<Vector3> availableDirections = GetAvailableDirectionsAtPosition(emptyPosition);
        Vector3 targetDirection = availableDirections[Random.Range(0, availableDirections.Count)];

        Physics.Raycast(emptyPosition, targetDirection, out var hit);
        
        if (hit.collider == null || hit.collider.gameObject.GetComponent<PuzzleController>() == null) return;

        PuzzleController puzzle = hit.collider.gameObject.GetComponent<PuzzleController>();

        puzzle.MovePuzzleTo(-targetDirection);

        shakingCount++;
    }

    private Vector3 GetEmptyPosition()
    {
        foreach (GridPositionController controller in _gridPositionControllers)
        {
            if (!Physics.Raycast(controller.GetCenterPosition() + Vector3.up, Vector3.down, _maxDistance, whatIsPuzzle))
            {
                return controller.GetCenterPosition();
            }
        }
        return Vector3.zero;
    }
    
    private List<Vector3> GetAvailableDirectionsAtPosition(Vector3 position)
    {
        List<Vector3> result = new List<Vector3>();

        foreach (Vector3 direction in _availableDirections)
        {
            if (Physics.Raycast(position, direction, _maxDistance, whatIsPuzzle))
            {
                result.Add(direction);
            }
        }

        return result;
    }

    private void CheckPuzzlesInCompletedPosition()
    {
        int index = 0;
        foreach (GameObject puzzle in _puzzles)
        {
            if (puzzle.transform.position != _gridPositionControllers[index++].GetCenterPosition()) return;
        }

        GlobalEventManager.StopGame();
    }
}