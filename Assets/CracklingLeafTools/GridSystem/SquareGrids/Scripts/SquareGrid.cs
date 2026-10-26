using UnityEngine;

public class SquareGrid : MonoBehaviour
{
    #region Serialized
    [SerializeField] Transform tileContent;

    #endregion

    #region Properties
    public Transform TileContent { get { return tileContent; } }
    #endregion


}
