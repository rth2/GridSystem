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
    [SerializeField] bool showLabels;
    #endregion

    #region Properties

    #endregion

    GameObject squarePrefab;

    const float spriteRotationX = 90f;

    void CreateSquareGrid()
    {

    }

    void DestroyAllGrids()
    {

    }

    public void EditorCreateSquareGrid()
    {

    }

    public void EditorDestroyAllGrids()
    {

    }

#if UNITY_EDITOR
    #region Editor Tests
    void CreateSquareGridEditor()
    {

    }
    void DestroyAllGridsEditor()
    {

    }
    #endregion

#endif

}
