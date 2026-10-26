using UnityEngine;

public class HexGrid : MonoBehaviour
{
    #region Serialized
    [SerializeField] Transform tileContent;

    #endregion

    #region Properties
    public Transform TileContent { get { return tileContent; } }
    #endregion

}
