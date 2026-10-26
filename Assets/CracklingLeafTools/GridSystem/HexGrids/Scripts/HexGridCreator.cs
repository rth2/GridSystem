using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class HexGridCreator : MonoBehaviour
{
    #region Serialized
    [SerializeField] GameObject hexPrefab;
    [SerializeField] GameObject gridPrefab;
    [SerializeField] Transform gridContent;
    [SerializeField] List<SO_HexGrid>  hexGrids;
    [SerializeField] bool showLabels;
    #endregion

    #region Properties
    //public Transform GridContent { get { return gridContent; } }
    #endregion

    #region Hex Tile Knowns
    const float TileRotationX = -90f;
    const float TilePointyTopZ = 0f;
    const float TileFlatTopZ = 30f;
    const float EdgeToEdge = 1.732051f;
    const float EdgeToEdgeHalf = 0.8660255f;
    const float Height = 0.1999601f;
    const float PointToPoint = 1.5f;
    #endregion


    public void CreateHexGrid(SO_HexGrid hexSO, Vector3 startPos)
    {
        switch (hexSO.GridShape)
        {
            case GridManager.GridShape.Rectangular:
                CreateRectangularGrid(hexSO, startPos);
                break;
            case GridManager.GridShape.Hexagonal:
                CreateHexagonalGrid(hexSO, startPos);
                break;
        }
    }

    #region Grid Creators

    void CreateRectangularGrid(SO_HexGrid hexSO, Vector3 startPos)
    {
        GameObject grid = Instantiate(gridPrefab, startPos, Quaternion.identity, gridContent);

        if (grid.TryGetComponent<HexGrid>(out var hexGrid))
        {
            int rows = hexSO.RectRows;
            int cols = hexSO.RectColumns;
            bool offsetEven = hexSO.OffsetEven;

            Quaternion spawnRotation = Quaternion.Euler(TileRotationX, 0, hexSO.HexOrientation == GridManager.HexOrientation.PointyTop ? TilePointyTopZ : TileFlatTopZ);

            for (int row = 0; row < hexSO.RectRows; row++)
            {
                for (int col = 0; col < hexSO.RectColumns; col++)
                {
                    //setup position placement
                    Vector3 spawnPos;
                    float x, y, z;
                    y = startPos.y;
                    //Pointy top offsets 'left/right'; flat offsets 'up/down'
                    if (hexSO.HexOrientation == GridManager.HexOrientation.PointyTop)
                    {
                        z = startPos.z - PointToPoint * row;
                        x = startPos.x;

                        if (row % 2 == 0 && hexSO.OffsetEven) { x -= EdgeToEdgeHalf; }
                        else if (row % 2 != 0 && !hexSO.OffsetEven) { x -= EdgeToEdgeHalf; }

                        x += EdgeToEdge * col;

                        spawnPos = new(x, y, z);
                    }
                    else
                    {
                        x = startPos.x + PointToPoint * col;
                        z = startPos.z;

                        if (col % 2 == 0 && hexSO.OffsetEven) { z -= EdgeToEdgeHalf; }
                        else if (col % 2 != 0 && !hexSO.OffsetEven) { z -= EdgeToEdgeHalf; }

                        z -= EdgeToEdge * row;
                    }

                    spawnPos = new(x, y, z);

                    GameObject tile = Instantiate(hexPrefab, spawnPos, spawnRotation, hexGrid.TileContent);

                    //for debugging the hex tiles and when they are placed
                    if (tile.TryGetComponent<HexTile>(out HexTile hexTile))
                    {
                        int tilePlaced = row * hexSO.RectRows + col;
                        hexTile.LabelText = showLabels ? $"{tilePlaced:N0}" : "";
                    }
                }
            }
        }

    }

    void CreateHexagonalGrid(SO_HexGrid hexSO, Vector3 startPos)
    {
        GameObject grid = Instantiate(gridPrefab, startPos, Quaternion.identity, gridContent);

        if (grid.TryGetComponent<HexGrid>(out var hexGrid))
        {
            Quaternion spawnRotation = Quaternion.Euler(TileRotationX, 0, hexSO.HexOrientation == GridManager.HexOrientation.PointyTop ? TilePointyTopZ : TileFlatTopZ);
            float x = startPos.x;
            float y = startPos.y;
            float z = startPos.z;
            Vector3 spawnPos;

            int numberTilePlaced = 0;
            int tilesInRow = hexSO.LayerDepth + (hexSO.LayerDepth - 1); //this is the starting row (middle row) tile count. Goes down by 1 each depth.
            int startHalfTiles = (int)MathF.Floor((float)tilesInRow / 2f);
            int depthLayersTraversed = 0;    //basically how many depth layers we have gone through

            if (hexSO.HexOrientation == GridManager.HexOrientation.PointyTop)
            {
                //pointy top
                x = startPos.x - (EdgeToEdge * startHalfTiles);
                for (int i = 0; i < hexSO.LayerDepth; i++)
                {
                    if (i == 0)
                    {
                        //makes the middle row
                        for (int j = 0; j < tilesInRow; j++)
                        {
                            spawnPos = new(x, y, z);
                            GameObject tile = Instantiate(hexPrefab, spawnPos, spawnRotation, hexGrid.TileContent);

                            //for debugging the hex tiles and when they are placed
                            if (tile.TryGetComponent<HexTile>(out HexTile hexTile))
                            {
                                hexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            x += EdgeToEdge;
                        }
                    }
                    else
                    {
                        tilesInRow--;
                        startHalfTiles = (int)MathF.Floor((float)tilesInRow / 2f);
                        x = startPos.x - (EdgeToEdge * startHalfTiles);
                        if (depthLayersTraversed % 2 == 1) { x += EdgeToEdgeHalf; }
                        float topZ = startPos.z + (PointToPoint * depthLayersTraversed);
                        float botZ = startPos.z - (PointToPoint * depthLayersTraversed);

                        for (int j = 0; j < tilesInRow; j++)
                        {
                            spawnPos = new(x, y, topZ);
                            GameObject topTile = Instantiate(hexPrefab, spawnPos, spawnRotation, hexGrid.TileContent);

                            //for debugging the hex tiles and when they are placed
                            if (topTile.TryGetComponent<HexTile>(out HexTile topHexTile))
                            {
                                topHexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            spawnPos = new(x, y, botZ);
                            GameObject botTile = Instantiate(hexPrefab, spawnPos, spawnRotation, hexGrid.TileContent);

                            //for debugging the hex tiles and when they are placed
                            if (botTile.TryGetComponent<HexTile>(out HexTile botHexTile))
                            {
                                botHexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            x += EdgeToEdge;
                        }
                    }
                    depthLayersTraversed++;
                }

            }
            else
            {
                //flat top
                z = startPos.z - (EdgeToEdge * startHalfTiles);

                for (int i = 0; i < hexSO.LayerDepth; i++)
                {
                    if (i == 0)
                    {
                        //makes the middle row
                        for (int j = 0; j < tilesInRow; j++)
                        {
                            spawnPos = new(x, y, z);
                            GameObject tile = Instantiate(hexPrefab, spawnPos, spawnRotation, hexGrid.TileContent);

                            //for debugging the hex tiles and when they are placed
                            if (tile.TryGetComponent<HexTile>(out HexTile hexTile))
                            {
                                hexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            z += EdgeToEdge;
                        }
                    }
                    else
                    {
                        tilesInRow--;
                        startHalfTiles = (int)MathF.Floor((float)tilesInRow / 2f);
                        z = startPos.z - (EdgeToEdge * startHalfTiles);
                        if (depthLayersTraversed % 2 == 1) { z += EdgeToEdgeHalf; }
                        float rightX = startPos.x + (PointToPoint * depthLayersTraversed);
                        float leftX = startPos.x - (PointToPoint * depthLayersTraversed);

                        for (int j = 0; j < tilesInRow; j++)
                        {
                            spawnPos = new(rightX, y, z);
                            GameObject topTile = Instantiate(hexPrefab, spawnPos, spawnRotation, hexGrid.TileContent);

                            //for debugging the hex tiles and when they are placed
                            if (topTile.TryGetComponent<HexTile>(out HexTile topHexTile))
                            {
                                topHexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            spawnPos = new(leftX, y, z);
                            GameObject botTile = Instantiate(hexPrefab, spawnPos, spawnRotation, hexGrid.TileContent);

                            //for debugging the hex tiles and when they are placed
                            if (botTile.TryGetComponent<HexTile>(out HexTile botHexTile))
                            {
                                botHexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            z += EdgeToEdge;
                        }

                    }
                    depthLayersTraversed++;
                }
            }
        }
    }

    void DestroyAllGrids()
    {
        for (int i = gridContent.childCount - 1; i >= 0; i--)
        {
            Destroy(gridContent.GetChild(i).gameObject);
        }
    }

    #endregion

#if UNITY_EDITOR
    #region Editor Tests

    public void EditorRectangleGridCreate()
    {
        //CreateRectangularGrid(hexGrids[1], Vector3.zero);
        RectangleGridInEditor(hexGrids[1], Vector3.zero);
    }
    public void EditorHexagonalGridCreate()
    {
        //CreateHexagonalGrid(hexGrids[0], Vector3.zero);
        HexagonalGridInEditor(hexGrids[0], Vector3.zero);
    }

    public void EditorDestroyAllGrids()
    {
        //DestroyAllGrids();
        DestroyAllGridsEditor();
    }

    void DestroyAllGridsEditor()
    {
        Undo.RegisterFullObjectHierarchyUndo(gameObject, "Destroy All Grids");

        //remove generated boards
        for (int i = gridContent.childCount - 1; i >= 0; i--)
        {
            Undo.DestroyObjectImmediate(gridContent.GetChild(i).gameObject);
        }
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
    }

    void RectangleGridInEditor(SO_HexGrid hexSO, Vector3 startPos)
    {
        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();

        var grid = (GameObject)PrefabUtility.InstantiatePrefab(gridPrefab);
        Undo.RegisterCreatedObjectUndo(grid, "Create Hex Grid");

        Undo.SetTransformParent(grid.transform, gridContent, "Parent Hex Grid");
        grid.transform.position = startPos;

        if (grid.TryGetComponent<HexGrid>(out var hexGrid))
        {
            int rows = hexSO.RectRows;
            int cols = hexSO.RectColumns;
            bool offsetEven = hexSO.OffsetEven;

            Quaternion spawnRotation = Quaternion.Euler(TileRotationX, 0, hexSO.HexOrientation == GridManager.HexOrientation.PointyTop ? TilePointyTopZ : TileFlatTopZ);

            for(int row = 0; row < hexSO.RectRows; row++)
            {
                for(int col = 0; col < hexSO.RectColumns; col++)
                {
                    GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(hexPrefab);
                    Undo.RegisterCreatedObjectUndo(tile, "Create Hex Tile");
                    Undo.SetTransformParent(tile.transform, hexGrid.TileContent, "Parent Hex Tile");

                    //for debugging the hex tiles and when they are placed
                    if(tile.TryGetComponent<HexTile>(out HexTile hexTile))
                    {
                        int tilePlaced = row * hexSO.RectRows + col;
                        hexTile.LabelText = showLabels ? $"{tilePlaced:N0}" : "";
                    }

                    //setup position placement
                    Vector3 spawnPos;
                    float x, y, z;
                    y = startPos.y;
                    //Pointy top offsets 'left/right'; flat offsets 'up/down'
                    if (hexSO.HexOrientation == GridManager.HexOrientation.PointyTop)
                    {
                        z = startPos.z - PointToPoint * row;
                        x = startPos.x;

                        if(row % 2 == 0 && hexSO.OffsetEven) { x -= EdgeToEdgeHalf; }
                        else if(row % 2 != 0 && !hexSO.OffsetEven) { x -= EdgeToEdgeHalf; }

                        x += EdgeToEdge * col;

                        spawnPos = new(x,y,z);
                    }
                    else
                    {
                        x = startPos.x + PointToPoint * col;
                        z = startPos.z;

                        if (col % 2 == 0 && hexSO.OffsetEven) { z -= EdgeToEdgeHalf; }
                        else if (col % 2 != 0 && !hexSO.OffsetEven) { z -= EdgeToEdgeHalf; }

                        z -= EdgeToEdge * row;
                    }

                    spawnPos = new(x, y, z);
                    tile.transform.SetPositionAndRotation(spawnPos, spawnRotation);
                }
            }
        }
        Undo.CollapseUndoOperations(undoGroup);
    }

    void HexagonalGridInEditor(SO_HexGrid hexSO, Vector3 startPos)
    {
        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();

        var grid = (GameObject)PrefabUtility.InstantiatePrefab(gridPrefab);
        Undo.RegisterCreatedObjectUndo(grid, "Create Hex Grid");

        Undo.SetTransformParent(grid.transform, gridContent, "Parent Hex Grid");
        grid.transform.position = startPos;

        if (grid.TryGetComponent<HexGrid>(out var hexGrid))
        {
            Quaternion spawnRotation = Quaternion.Euler(TileRotationX, 0, hexSO.HexOrientation == GridManager.HexOrientation.PointyTop ? TilePointyTopZ : TileFlatTopZ);
            float x = startPos.x;
            float y = startPos.y;
            float z = startPos.z;
            Vector3 spawnPos;

            int numberTilePlaced = 0;
            int tilesInRow = hexSO.LayerDepth + (hexSO.LayerDepth - 1); //this is the starting row (middle row) tile count. Goes down by 1 each depth.
            int startHalfTiles = (int)MathF.Floor((float)tilesInRow / 2f);
            int depthLayersTraversed = 0;    //basically how many depth layers we have gone through

            if (hexSO.HexOrientation == GridManager.HexOrientation.PointyTop)
            {
                //pointy top
                x = startPos.x - (EdgeToEdge * startHalfTiles);
                for(int i = 0; i < hexSO.LayerDepth; i++)
                {
                    if(i == 0)
                    {
                        //makes the middle row
                        for (int j = 0; j < tilesInRow; j++)
                        {
                            GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(hexPrefab);

                            //for debugging the hex tiles and when they are placed
                            if (tile.TryGetComponent<HexTile>(out HexTile hexTile))
                            {
                                hexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            spawnPos = new(x, y, z);
                            tile.transform.SetPositionAndRotation(spawnPos, spawnRotation);
                            Undo.RegisterCreatedObjectUndo(tile, "Create Hex Tile");
                            Undo.SetTransformParent(tile.transform, hexGrid.TileContent, "Parent Hex Tile");
                            x += EdgeToEdge;
                        }
                    }
                    else
                    {
                        tilesInRow--;
                        startHalfTiles = (int)MathF.Floor((float)tilesInRow / 2f);
                        x = startPos.x - (EdgeToEdge * startHalfTiles);
                        if(depthLayersTraversed % 2 == 1) { x += EdgeToEdgeHalf; }
                        float topZ = startPos.z + (PointToPoint * depthLayersTraversed);
                        float botZ = startPos.z - (PointToPoint * depthLayersTraversed);

                        for (int j = 0; j < tilesInRow; j++)
                        {
                            GameObject topTile = (GameObject)PrefabUtility.InstantiatePrefab(hexPrefab);

                            //for debugging the hex tiles and when they are placed
                            if (topTile.TryGetComponent<HexTile>(out HexTile topHexTile))
                            {
                                topHexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            spawnPos = new(x, y, topZ);
                            topTile.transform.SetPositionAndRotation(spawnPos, spawnRotation);
                            Undo.RegisterCreatedObjectUndo(topTile, "Create Hex Tile");
                            Undo.SetTransformParent(topTile.transform, hexGrid.TileContent, "Parent Hex Tile");
                            GameObject botTile = (GameObject)PrefabUtility.InstantiatePrefab(hexPrefab);

                            //for debugging the hex tiles and when they are placed
                            if (botTile.TryGetComponent<HexTile>(out HexTile botHexTile))
                            {
                                botHexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            spawnPos = new(x, y, botZ);
                            botTile.transform.SetPositionAndRotation(spawnPos, spawnRotation);
                            Undo.RegisterCreatedObjectUndo(botTile, "Create Hex Tile");
                            Undo.SetTransformParent(botTile.transform, hexGrid.TileContent, "Parent Hex Tile");
                            x += EdgeToEdge;
                        }

                    }

                    depthLayersTraversed++;
                }
                
            }
            else
            {
                //flat top
                z = startPos.z - (EdgeToEdge * startHalfTiles);

                for (int i = 0; i < hexSO.LayerDepth; i++)
                {
                    if (i == 0)
                    {
                        //makes the middle row
                        for (int j = 0; j < tilesInRow; j++)
                        {
                            GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(hexPrefab);

                            //for debugging the hex tiles and when they are placed
                            if (tile.TryGetComponent<HexTile>(out HexTile hexTile))
                            {
                                hexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            spawnPos = new(x, y, z);
                            tile.transform.SetPositionAndRotation(spawnPos, spawnRotation);
                            Undo.RegisterCreatedObjectUndo(tile, "Create Hex Tile");
                            Undo.SetTransformParent(tile.transform, hexGrid.TileContent, "Parent Hex Tile");
                            z += EdgeToEdge;
                        }
                    }
                    else
                    {
                        tilesInRow--;
                        startHalfTiles = (int)MathF.Floor((float)tilesInRow / 2f);
                        z = startPos.z - (EdgeToEdge * startHalfTiles);
                        if (depthLayersTraversed % 2 == 1) { z += EdgeToEdgeHalf; }
                        float rightX = startPos.x + (PointToPoint * depthLayersTraversed);
                        float leftX = startPos.x - (PointToPoint * depthLayersTraversed);

                        for (int j = 0; j < tilesInRow; j++)
                        {
                            GameObject topTile = (GameObject)PrefabUtility.InstantiatePrefab(hexPrefab);

                            //for debugging the hex tiles and when they are placed
                            if (topTile.TryGetComponent<HexTile>(out HexTile topHexTile))
                            {
                                topHexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            spawnPos = new(rightX, y, z);
                            topTile.transform.SetPositionAndRotation(spawnPos, spawnRotation);
                            Undo.RegisterCreatedObjectUndo(topTile, "Create Hex Tile");
                            Undo.SetTransformParent(topTile.transform, hexGrid.TileContent, "Parent Hex Tile");

                            GameObject botTile = (GameObject)PrefabUtility.InstantiatePrefab(hexPrefab);
                            

                            //for debugging the hex tiles and when they are placed
                            if (botTile.TryGetComponent<HexTile>(out HexTile botHexTile))
                            {
                                botHexTile.LabelText = showLabels ? $"{numberTilePlaced:N0}" : "";
                                numberTilePlaced++;
                            }

                            spawnPos = new(leftX, y, z);
                            botTile.transform.SetPositionAndRotation(spawnPos, spawnRotation);
                            Undo.RegisterCreatedObjectUndo(botTile, "Create Hex Tile");
                            Undo.SetTransformParent(botTile.transform, hexGrid.TileContent, "Parent Hex Tile");
                            z += EdgeToEdge;
                        }

                    }
                    depthLayersTraversed++;
                }
            }
        }

        Undo.CollapseUndoOperations(undoGroup);
    }
    #endregion
#endif
}
