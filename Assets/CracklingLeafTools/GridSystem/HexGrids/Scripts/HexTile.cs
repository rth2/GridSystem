using UnityEngine;
using TMPro;

public class HexTile : MonoBehaviour
{

    [SerializeField] bool pointyTopByDefault;
    [SerializeField] TextMeshPro labelText;

    [Header("Will Remove")]
    [SerializeField] Mesh mesh;

    public string LabelText
    {
        get { return labelText.text; }
        set { labelText.text = value; }
    }

    //private void Start()
    //{

    //    Bounds bounds = mesh.bounds;

    //    float width = bounds.size.x;
    //    float height = bounds.size.y;
    //    float depth = bounds.size.z;

    //    Debug.Log($"width: {width}; height: {height}; depth: {depth}");

    //}

}
