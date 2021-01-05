using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "Level Settings", menuName = "Level Settings", order = 0)]
public class LevelGeneratorSettings : ScriptableObject
{
    public GameObject levelPrefab;
    
    [SerializeField]
    private LevelTile[] tiles;

    private Dictionary<Color, LevelTile> tilesByColor;
    private Dictionary<LevelTileType, LevelTile> tilesByType;

    public void Load()
    {
        tilesByColor = new Dictionary<Color, LevelTile>();
        tilesByType = new Dictionary<LevelTileType, LevelTile>();
            
        foreach (var tile in tiles)
        {
            tilesByColor[tile.color] = tile;
            tilesByType[tile.type] = tile;
        }
    }

    public LevelTile GetTile(Color color)
    {
        if (!tilesByColor.TryGetValue(color, out LevelTile tile))
        {
            throw new KeyNotFoundException($"Undefined tilemap color: {color}");
        }
        return tile;
    }
    
    public LevelTile GetTile(LevelTileType type)
    {
        if (!tilesByType.TryGetValue(type, out LevelTile tile))
        {
            throw new KeyNotFoundException($"Undefined tilemap type: {type}");
        }
        return tile;
    }
}
