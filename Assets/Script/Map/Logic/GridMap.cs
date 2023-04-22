using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData;//地图数据
    public GridType gridType;//地图类型
    private Tilemap currentTilemap;//当前的地图

    private void OnEnable()
    {
        if (!Application.IsPlaying(this))//这个代码是否在运行
        {
            currentTilemap = GetComponent<Tilemap>();

            if (mapData != null)
                mapData.tileProperties.Clear();
        }
    }

    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();

            UpdateTileProperties();
#if UNITY_EDITOR
            if (mapData != null)
                EditorUtility.SetDirty(mapData);
#endif
        }
    }

    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds();

        if (!Application.IsPlaying(this))
        {
            if (mapData != null)
            {
                //已绘制范围的左下角坐标
                Vector3Int startPos = currentTilemap.cellBounds.min;
                //已绘制范围的右上角坐标
                Vector3Int endPos = currentTilemap.cellBounds.max;

                for (int x = startPos.x; x < endPos.x; x++)//循环x和y，去循环地图
                {
                    for (int y = startPos.y; y < endPos.y; y++)
                    {
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));

                        if (tile != null)
                        {
                            TileProperty newTile = new TileProperty
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                gridType = this.gridType,
                                boolTypeValue = true
                            };

                            mapData.tileProperties.Add(newTile);
                        }
                    }
                }
            }
        }
    }
}
