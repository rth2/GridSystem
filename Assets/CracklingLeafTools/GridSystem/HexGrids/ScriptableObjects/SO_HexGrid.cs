using UnityEngine;

[CreateAssetMenu(fileName = "HexGrid", menuName = "Scriptable Objects/CracklingLeafTools/GridSystem/HexGrid")]
public class SO_HexGrid : ScriptableObject
{
    #region references
    [Header("Every Grid")]
    [SerializeField] GridManager.GridShape gridShape;
    [SerializeField] GridManager.HexOrientation hexOrientation;

    [Header("Hexagonal Specific")]
    [Tooltip("Layer depth is like a ring radiating from the center. 1 = 1 tile; 2 = 7 tiles; 3 = 19; etc...")]
    [SerializeField] int layerDepth = 1;

    [Header("Rectangular Specific")]
    [SerializeField] int rectRows;
    [SerializeField] int rectColumns;
    
    [Tooltip("True offsets indices 0 and even, False offsets odds")]
    [SerializeField] bool offsetEven;

    #endregion

    #region Properties
    public GridManager.GridShape GridShape { get { return gridShape; } }
    public GridManager.HexOrientation HexOrientation { get { return hexOrientation; } }
    public int LayerDepth { get { return layerDepth; } set { layerDepth = value; } }
    public int RectRows { get { return rectRows; } set { rectRows = value; } }
    public int RectColumns {  get { return rectColumns; } set { rectColumns = value; } }
    public bool OffsetEven { get { return offsetEven; } }

    #endregion
}
