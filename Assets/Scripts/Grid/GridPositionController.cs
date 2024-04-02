using UnityEngine;
using UnityEngine.Serialization;

public class GridPositionController : MonoBehaviour
{
    [SerializeField] private GameObject center;

    [FormerlySerializedAs("_positionAtLine")] [SerializeField] private int positionAtLine;

    [FormerlySerializedAs("_lineController")] [SerializeField] private GridLineController lineController;

    private void Start()
    {
        lineController = GetComponentInParent<GridLineController>();
    }

    public Vector3 GetCenterPosition()
    {
        return center.transform.position;
    }

    public void SetPositionAtLine(int value)
    {
        positionAtLine = value;
    }

    public Vector2 GetPositionAtGrid()
    {
        return new Vector2(lineController.GetPositionAtGrid(), positionAtLine);
    }
}
