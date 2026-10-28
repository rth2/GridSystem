using UnityEngine;
using TMPro;

public class HexTile : MonoBehaviour
{

    [SerializeField] bool pointyTopByDefault;
    [SerializeField] TextMeshPro labelText;

    public string LabelText
    {
        get { return labelText.text; }
        set { labelText.text = value; }
    }
}
