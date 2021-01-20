using System;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : ScriptableWizard
{
    [Header("Settings")]
    [SerializeField] private LevelGeneratorSettings settings;

    [Header("Level")]
    [SerializeField] private Texture2D tilemap;

    [MenuItem("GameObject/Memory Lanes/Create Level")]
    private static void CreateWizard()
    {
        DisplayWizard<LevelGenerator>("Level Generator");
    }

    private void Awake()
    {
        settings = AssetDatabase.LoadAssetAtPath<LevelGeneratorSettings>(
                "Assets/Prefabs/Level Generator Settings.asset");
    }

    void OnWizardCreate()
    {
        settings.Load();

        Level levelPrefab = CreateLevelPrefab();
        CreateLevel(levelPrefab);
        SaveTriggers(levelPrefab);
        SaveActivators(levelPrefab);
        SaveLevelPrefab(levelPrefab, tilemap.name);
        DestroyImmediate(levelPrefab.gameObject);
    }

    private Level CreateLevelPrefab()
    {
        GameObject prefab = PrefabUtility.InstantiatePrefab(settings.levelPrefab) as GameObject;
        if (prefab == null)
        {
            throw new NullReferenceException("Invalid prefab used to generate a level");
        }

        Level levelPrefab = prefab.GetComponent<Level>();
        if (levelPrefab == null)
        {
            throw new NullReferenceException("Prefab is missing a Level component");
        }

        return levelPrefab;
    }

    private void SaveLevelPrefab(Level levelPrefab, string filename)
    {
        PrefabUtility.SaveAsPrefabAsset(levelPrefab.gameObject, $"Assets/Prefabs/{filename}.prefab");
    }
    
    private void CreateLevel(Level levelPrefab)
    {
        int width = tilemap.width;
        int height = tilemap.height;

        Color[] pixelColors = tilemap.GetPixels();
        int k = 0;

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Vector3 tilePosition = new Vector3(i, 0f, j);
                Color pixelColor = pixelColors[k++];

                try
                {
                    LevelTile tile = settings.GetTile(pixelColor);

                    CreateGridTile(levelPrefab, tile, tilePosition);
                    CreateGridObject(levelPrefab, tile, tilePosition);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }

    private void CreateGridTile(Level levelPrefab, LevelTile tile, Vector3 tilePosition)
    {
        switch (tile.Type)
        {
            case LevelTileType.Boundary:
                InstantiateTile(settings.GetTile(LevelTileType.Boundary).Prefab, tilePosition, levelPrefab.boundaryContainer);
                break;
            default:
                InstantiateTile(settings.GetTile(LevelTileType.Path).Prefab, tilePosition, levelPrefab.pathContainer);
                break;
        }
    }

    private void CreateGridObject(Level levelPrefab, LevelTile tile, Vector3 tilePosition)
    {
        switch (tile.Type)
        {
            case LevelTileType.Player:
                levelPrefab.player = InstantiateTile(tile.Prefab, tilePosition, levelPrefab.transform).GetComponent<Agent>();
                break;
            case LevelTileType.Goal:
                levelPrefab.goal = InstantiateTile(tile.Prefab, tilePosition, levelPrefab.triggerContainer).GetComponent<MyGoal>();
                break;
            case LevelTileType.Switch:
                InstantiateTile(tile.Prefab, tilePosition, levelPrefab.triggerContainer);
                break;
            case LevelTileType.Door:
            case LevelTileType.Platform:
                InstantiateTile(tile.Prefab, tilePosition, levelPrefab.activatorContainer);
                break;
            case LevelTileType.Npc:
                InstantiateTile(tile.Prefab, tilePosition, levelPrefab.transform);
                break;
            default:
                break;
        }
    }

    private GameObject InstantiateTile(GameObject prefab, Vector3 tilePosition, Transform transform)
    {
        GameObject tile = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;

        if (tile == null)
        {
            throw new NullReferenceException($"Undefined tile @ {tilePosition}");
        }

        tile.transform.position = tilePosition;
        return tile;
    }

    private void SaveTriggers(Level levelPrefab)
    {
        levelPrefab.triggers = levelPrefab.triggerContainer.GetComponentsInChildren<MyTrigger>();
    }
    
    private void SaveActivators(Level levelPrefab)
    {
        levelPrefab.activators = levelPrefab.activatorContainer.GetComponentsInChildren<MyActivator>();
    }
}
