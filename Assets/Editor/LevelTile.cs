using System;
using UnityEngine;

public enum LevelTileType
{
    Boundary,
    Path,
    Player,
    Goal,
    Switch,
    Door,
    Platform,
    Npc,
}

[Serializable]
public class LevelTile
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Color color;

    private LevelTileType type;
    private bool isBackground;

    public LevelTile(LevelTileType type, bool isBackground)
    {
        this.prefab = default(GameObject);
        this.color = default(Color);
        this.type = type;
        this.isBackground = isBackground;
    }

    public GameObject Prefab => prefab;
    public Color Color => color;
    public LevelTileType Type => type;
    public bool IsBackground => isBackground;
}
