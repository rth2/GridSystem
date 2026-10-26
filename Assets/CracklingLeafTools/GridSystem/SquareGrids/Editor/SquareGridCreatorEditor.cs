using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SquareGridCreator))]
public class SquareGridCreatorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SquareGridCreator gridCreator = (SquareGridCreator)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Create 3d Grid"))
        {

        }
        GUILayout.Space(10);
        if (GUILayout.Button("Create 2d Grid"))
        {

        }
        GUILayout.Space(10);
        if (GUILayout.Button("Destroy all Grids"))
        {
            gridCreator.EditorDestroyAllGrids();
        }
    }

}
