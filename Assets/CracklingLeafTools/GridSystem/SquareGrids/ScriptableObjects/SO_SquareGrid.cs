using UnityEngine;


[CreateAssetMenu(fileName = "SquareGrid", menuName = "Scriptable Objects/CracklingLeafTools/GridSystem/SquareGrid")]
public class SO_SquareGrid : ScriptableObject
{
    [SerializeField] GameObject squarePrefab;
    [SerializeField] int rows;
    [SerializeField] int columns;


    #region properties
    public int Rows { get => rows; }
    public int Columns { get => columns; }
    public GameObject SquarePrefab { get => squarePrefab; }
    #endregion

}
