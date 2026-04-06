using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSwapper : MonoBehaviour
{
    [Header("Tile References")]
    public TileBase normalTile;
    public TileBase sterilizedTile;   // the "after AoE" tile variant

    public Tilemap tilemap;

    public void SwapTilesInRadius(Vector3 worldCenter, float radius)
    {
        Vector3Int centerCell = tilemap.WorldToCell(worldCenter);
        int cellRadius = Mathf.CeilToInt(radius / tilemap.cellSize.x);

        Debug.Log($"SwapTilesInRadius called | Center: {centerCell} | CellRadius: {cellRadius}");

        int swapped = 0;

        for (int x = -cellRadius; x <= cellRadius; x++)
        {
            for (int y = -cellRadius; y <= cellRadius; y++)
            {
                Vector3Int cellPos = centerCell + new Vector3Int(x, y, 0);
                Vector3 cellWorld = tilemap.GetCellCenterWorld(cellPos);
                if (Vector3.Distance(cellWorld, worldCenter) > radius) continue;

                TileBase currentTile = tilemap.GetTile(cellPos);
                Debug.Log($"Cell {cellPos} has tile: {(currentTile != null ? currentTile.name : "NULL")}");

                if (currentTile == normalTile)
                {
                    tilemap.SetTile(cellPos, sterilizedTile);
                    swapped++;
                }
            }
        }

        Debug.Log($"Total tiles swapped: {swapped}");
    }
}