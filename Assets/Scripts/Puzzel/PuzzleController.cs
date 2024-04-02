using System;
using System.Collections.Generic;
using Puzzel;
using TMPro;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    private static float _maxDistance = 1.25f;

    [SerializeField] private TextMeshPro text;
    [SerializeField] private GameObject body;
    [SerializeField] private LayerMask whatIsBounds;

    private Puzzle _puzzle;
    private Vector3 _moveDirection;
    private List<Vector3> _availableDirections = new() { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    private GridPositionController _gridPositionController;
    private Vector3 _targetPosition = Vector3.zero;

    private void UpdatePuzzlePosition()
    {
        Vector3 currentPosition = transform.position;

        if (currentPosition != _targetPosition && _targetPosition != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(currentPosition, _targetPosition,
                GlobalDataManager.PuzzleMoveSpeed * Time.deltaTime);
        }
        else if (GlobalDataManager.IsPuzzleMoving)
        {
            GlobalEventManager.FinishPuzzlesMoving();
            GlobalEventManager.OnPuzzlesUpdate.RemoveListener(UpdatePuzzlePosition);
        }
    }

    public void OnMouseDownEvent()
    {
        MovePuzzleToTargetDirection(GetAvailableDirection());
    }

    public void MovePuzzleTo(Vector3 targetDirection, bool useMovingAnimation = false)
    {
        if (GetAvailableDirection() != targetDirection)
        {
            throw new Exception("Movement in an inaccessible direction");
        }
        
        if (useMovingAnimation)
        {
            MovePuzzleToTargetDirection(targetDirection);
            return;
        }
        MovePuzzleToTargetPositionWithoutAnimation(targetDirection);
    }

    private void MovePuzzleToTargetPositionWithoutAnimation(Vector3 targetDirection)
    {
        if (GlobalDataManager.IsPuzzleMoving || GlobalDataManager.IsGameStopped) return;

        Physics.Raycast(transform.position, targetDirection, out var hit, _maxDistance);

        if (hit.collider == null || hit.collider.gameObject.GetComponent<GridPositionController>() == null) return;

        transform.position = hit.collider.gameObject.GetComponent<GridPositionController>().GetCenterPosition();
    }
    
    private void MovePuzzleToTargetDirection(Vector3 targetDirection)
    {
        if (GlobalDataManager.IsPuzzleMoving || GlobalDataManager.IsGameStopped) return;

        Physics.Raycast(transform.position, targetDirection, out var hit, _maxDistance);

        if (hit.collider == null
            || hit.collider.gameObject.GetComponent<GridPositionController>() == null)
        {
            _targetPosition = Vector3.zero;
            return;
        }

        _gridPositionController = hit.collider.gameObject.GetComponent<GridPositionController>();

        _targetPosition = _gridPositionController.GetCenterPosition();

        GlobalEventManager.StartPuzzleMoving();
        GlobalEventManager.OnPuzzlesUpdate.AddListener(UpdatePuzzlePosition);
    }
    
    private Vector3 GetAvailableDirection()
    {
        foreach (Vector3 direction in _availableDirections)
        {
            if (!Physics.Raycast(transform.position, direction, _maxDistance, whatIsBounds))
            {
                return direction;
            }
        }
        return Vector3.zero;
    }

    public void SetPuzzle(Puzzle puzzle)
    {
        _puzzle = puzzle;

        SetNumber(_puzzle.number);
        SetColor(_puzzle.color);
    }

    private void SetNumber(int number)
    {
        text.SetText(number.ToString());
    }

    private void SetColor(Color color)
    {
        body.GetComponent<Renderer>().material.SetColor("_Color", color);
    }
}