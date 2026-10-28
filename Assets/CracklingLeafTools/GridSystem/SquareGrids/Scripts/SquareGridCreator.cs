using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

public class SquareGridCreator : MonoBehaviour
{
    #region Serialized
    [SerializeField] List<SO_SquareGrid> squareSO;
    [SerializeField] GameObject gridPrefab;
    [SerializeField] Transform gridContent;
    //[SerializeField] bool showLabels;
    #endregion

    #region Properties

    #endregion

    //Creates tiles each column from left to right, then in row order. [row, col] -> [0,0], [0,1],..., [1,0], [1,1], ....
    public void CreateSquareGrid(SO_SquareGrid gridSO, Vector3 startPos)
    {
        GameObject grid = Instantiate(gridPrefab, startPos, Quaternion.identity, gridContent);

        if (grid.TryGetComponent<SquareGrid>(out SquareGrid squareGrid))
        {
            int rows = gridSO.Rows;
            int cols = gridSO.Columns;
            GameObject squarePrefab = gridSO.SquarePrefab;
            Quaternion spawnRotation = Quaternion.Euler(gridSO.SpawnRotation.x, gridSO.SpawnRotation.y, gridSO.SpawnRotation.z);
            Vector3 prefabScalar = gridSO.GetTileSize();

            for (int curRow = 0; curRow < rows; curRow++)
            {
                for (int curCol = 0; curCol < cols; curCol++)
                {
                    //setup position placement
                    float x, y, z;
                    x = startPos.x + (prefabScalar.x * curCol);
                    y = startPos.y;
                    z = startPos.z - (prefabScalar.z * curRow);
                    Vector3 spawnPoint = new Vector3(x, y, z);

                    GameObject tile = Instantiate(squarePrefab, spawnPoint, spawnRotation, squareGrid.TileContent);

                    if (tile.TryGetComponent<SquareTile>(out SquareTile squareTile))
                    {
                        //if we need to customize tile fields, that would go here.
                        //  probably use square tile as a base, and have 2d and 3d inherited if needed.
                    }
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

    public void EditorCreateSquareGrid(int index)
    {
        //CreateSquareGrid(squareSO[index], Vector3.zero);
        CreateSquareGridEditor(squareSO[index], Vector3.zero);
    }

    public void EditorDestroyAllGrids()
    {
        //DestroyAllGrids();
        DestroyAllGridsEditor();
    }

#if UNITY_EDITOR
    #region Editor Tests
    //Creates tiles each column from left to right, then in row order. [row, col] -> [0,0], [0,1],..., [1,0], [1,1], ....
    void CreateSquareGridEditor(SO_SquareGrid gridSO, Vector3 startPos)
    {
        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();

        GameObject grid = (GameObject)PrefabUtility.InstantiatePrefab(gridPrefab);
        Undo.RegisterCreatedObjectUndo(grid, "Create Square Grid");

        Undo.SetTransformParent(grid.transform, gridContent, "Parent Square Grid");
        grid.transform.position = startPos;

        if(grid.TryGetComponent<SquareGrid>(out SquareGrid squareGrid))
        {
            int rows = gridSO.Rows;
            int cols = gridSO.Columns;
            GameObject squarePrefab = gridSO.SquarePrefab;
            Quaternion spawnRotation = Quaternion.Euler(gridSO.SpawnRotation.x, gridSO.SpawnRotation.y, gridSO.SpawnRotation.z);
            Vector3 prefabScalar = gridSO.GetTileSize();

            Debug.Log($"{prefabScalar}");

            for (int curRow = 0; curRow < rows; curRow++)
            {
                for(int curCol = 0;curCol < cols; curCol++)
                {
                    GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(squarePrefab);
                    Undo.RegisterCreatedObjectUndo(tile, "Create square tile");
                    Undo.SetTransformParent(tile.transform, squareGrid.TileContent, "Parent Square Tile");

                    if(tile.TryGetComponent<SquareTile>(out SquareTile squareTile))
                    {
                        //if we need to customize tile fields, that would go here.
                        //  probably use square tile as a base, and have 2d and 3d inherited if needed.
                    }

                    //setup position placement
                    float x, y, z;
                    x = startPos.x + (prefabScalar.x * curCol);
                    y = startPos.y;
                    z = startPos.z - (prefabScalar.z * curRow);
                    Vector3 spawnPoint = new Vector3(x, y, z);
                    tile.transform.SetPositionAndRotation(spawnPoint, spawnRotation);
                }
            }
        }
        Undo.CollapseUndoOperations(undoGroup);
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
    #endregion

#endif

}
