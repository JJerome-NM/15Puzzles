using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [Header("Lines")] 
    [SerializeField] private List<GameObject> lines;

    private List<GridLineController> _linesControllers;

    private void Awake()
    {
        _linesControllers = new List<GridLineController>();
        lines.ForEach(o => _linesControllers.Add(o.GetComponent<GridLineController>()));
    }

    private void Start()
    {
        SetLineNumbers();
    }

    private void SetLineNumbers()
    {
        int i = 1;
        foreach (GridLineController controller in _linesControllers)
        {
            controller.SetPositionAtGrid(i++);
        }
    }

    public List<GridLineController> GetLineControllers()
    {
        return _linesControllers;
    }
}