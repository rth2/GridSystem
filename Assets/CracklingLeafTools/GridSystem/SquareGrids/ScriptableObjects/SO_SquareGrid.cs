using System;
using UnityEngine;


[CreateAssetMenu(fileName = "SquareGrid", menuName = "Scriptable Objects/CracklingLeafTools/GridSystem/SquareGrid")]
public class SO_SquareGrid : ScriptableObject
{
    [SerializeField] GameObject squarePrefab;
    [SerializeField] int rows;
    [SerializeField] int columns;
    [SerializeField] Vector3 spawnRotation;

    #region properties
    public int Rows { get => rows; }
    public int Columns { get => columns; }
    public GameObject SquarePrefab { get => squarePrefab; }
    public Vector3 SpawnRotation { get => spawnRotation; }
    #endregion

    public Vector3 GetTileSize()
    {
        if(squarePrefab.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            Vector3 spriteBounds = spriteRenderer.bounds.size;
            spriteBounds.x *= squarePrefab.transform.lossyScale.x;
            spriteBounds.y *= squarePrefab.transform.lossyScale.y;
            spriteBounds.z *= squarePrefab.transform.lossyScale.z;

            return new Vector3( spriteBounds.x * squarePrefab.transform.lossyScale.x,
                                spriteBounds.y * squarePrefab.transform.lossyScale.y,
                                spriteBounds.z * squarePrefab.transform.lossyScale.z);
        }

        if(squarePrefab.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer) &&
            squarePrefab.TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
        {
            Vector3 localSize = meshFilter.sharedMesh.bounds.size;
            Vector3 scale = squarePrefab.transform.lossyScale;

            return new Vector3( localSize.x * Mathf.Abs(scale.x),
                                localSize.y *  Mathf.Abs(scale.y),
                                localSize.z * Mathf.Abs(scale.z));
        }

        throw new System.InvalidOperationException("Tile prefab needs a SpriteRenderer or MeshRenderer with a MeshFilter.");
    }

}
