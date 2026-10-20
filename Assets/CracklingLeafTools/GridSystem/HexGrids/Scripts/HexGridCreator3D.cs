using UnityEngine;

public class HexGridCreator3D : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] GameObject hexPrefab;

    float spawnRotation = -90f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var worldPosition = grid.GetCellCenterWorld(new Vector3Int(0, 1));
        Quaternion spawnQuat = Quaternion.Euler(spawnRotation, 0, 0);
        Instantiate(hexPrefab, worldPosition, spawnQuat);

        worldPosition = grid.GetCellCenterWorld(new Vector3Int(1, 0));
        Instantiate(hexPrefab, worldPosition, spawnQuat);
        worldPosition = grid.GetCellCenterWorld(new Vector3Int(1, 1));
        Instantiate(hexPrefab, worldPosition, spawnQuat);
        worldPosition = grid.GetCellCenterWorld(new Vector3Int(2, 0));
        Instantiate(hexPrefab, worldPosition, spawnQuat);
        worldPosition = grid.GetCellCenterWorld(new Vector3Int(2, 1));
        Instantiate(hexPrefab, worldPosition, spawnQuat);
        worldPosition = grid.GetCellCenterWorld(new Vector3Int(1, 2));
        Instantiate(hexPrefab, worldPosition, spawnQuat);
    }

}
