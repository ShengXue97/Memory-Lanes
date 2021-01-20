using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// [CreateAssetMenu(fileName = "Level Settings", menuName = "Level Settings", order = 0)]
public class LevelGeneratorSettings : ScriptableObject
{
    [Header("Level Tiles")]
    public GameObject levelPrefab;

    // Background Tiles
    public LevelTile boundary = new LevelTile(type: LevelTileType.Boundary, isBackground: true);
    public LevelTile path = new LevelTile(type: LevelTileType.Path, isBackground: true);

    // Foreground Tiles
    public LevelTile player = new LevelTile(type: LevelTileType.Player, isBackground: false);
    public LevelTile goal = new LevelTile(type: LevelTileType.Goal, isBackground: false);
    public LevelTile button = new LevelTile(type: LevelTileType.Switch, isBackground: false);
    public LevelTile door = new LevelTile(type: LevelTileType.Door, isBackground: false);
    public LevelTile platform = new LevelTile(type: LevelTileType.Platform, isBackground: false);
    public LevelTile npc = new LevelTile(type: LevelTileType.Npc, isBackground: false);

    // Lookup Tables
    private Dictionary<Color, LevelTile> backgroundTilesByColor;
    private Dictionary<Color, LevelTile> foregroundTilesByColor;

    public void Load()
    {
        backgroundTilesByColor = Tiles.Where(x => x.IsBackground).ToDictionary(k => k.Color, v => v);
        foregroundTilesByColor = Tiles.Where(x => !x.IsBackground).ToDictionary(k => k.Color, v => v);
    }

    public LevelTile GetBackgroundTile(Color color)
    {
        if (!backgroundTilesByColor.TryGetValue(color, out LevelTile tile))
        {
            return path;
        }
        return tile;
    }
    
    public LevelTile GetForegroundTile(Color color)
    {
        if (!foregroundTilesByColor.TryGetValue(color, out LevelTile tile))
        {
            return null;
        }
        return tile;
    }

    public LevelTile[] Tiles => new [] 
    {
        boundary,
        path,
        player,
        goal,
        button,
        door,
        platform,
        npc,
    };
}
