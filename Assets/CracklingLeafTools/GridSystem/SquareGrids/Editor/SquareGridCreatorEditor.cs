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
        if (GUILayout.Button("Create 2d Grid"))
        {
            gridCreator.EditorCreateSquareGrid(0);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Create 3d Grid"))
        {
            gridCreator.EditorCreateSquareGrid(1);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Destroy all Grids"))
        {
            gridCreator.EditorDestroyAllGrids();
        }
    }

}
