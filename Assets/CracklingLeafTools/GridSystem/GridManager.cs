using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Hex Enums
    public enum GridShape
    {
        Rectangular = 0,
        Hexagonal,
    }
    public enum HexOrientation
    {
        PointyTop = 0,
        FlatTop,
    }

    public enum HexSwizzle
    {
        xzy = 0,
        //zxy,
    }
    #endregion



}
