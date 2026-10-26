using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

[CustomEditor(typeof(HexGridCreator))]
public class HexGridCreatorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HexGridCreator gridCreator = (HexGridCreator)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Create Rectangular Grid"))
        {
            gridCreator.EditorRectangleGridCreate();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Create Hexagonal Grid"))
        {
            gridCreator.EditorHexagonalGridCreate();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Destroy all Grids"))
        {
            gridCreator.EditorDestroyAllGrids();
        }

    }

}
