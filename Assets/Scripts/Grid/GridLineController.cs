using System.Collections.Generic;
using UnityEngine;

public class GridLineController : MonoBehaviour
{
    [Header("LinePositions")] 
    [SerializeField] private List<GameObject> positions;

    private List<GridPositionController> _positionControllers;

    [SerializeField] private int _positionAtGrid;

    private void Awake()
    {
        _positionControllers = new List<GridPositionController>();
        positions.ForEach(o => _positionControllers.Add(o.GetComponent<GridPositionController>()));
    }

    void Start()
    {
        SetPositionNumbersForGridPosition();
    }

    private void SetPositionNumbersForGridPosition()
    {
        int i = 1;
        foreach (GridPositionController controller in _positionControllers)
        {
            controller.SetPositionAtLine(i++);
        }
    }

    public List<GridPositionController> GetPositionControllers()
    {
        return _positionControllers;
    }
    
    public void SetPositionAtGrid(int value)
    {
        _positionAtGrid = value;
    }

    public int GetPositionAtGrid()
    {
        return _positionAtGrid;
    }
}
