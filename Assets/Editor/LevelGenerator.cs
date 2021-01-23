using System;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : ScriptableWizard
{
    // Path variables
    private static string _pathToSettings = "Assets/Prefabs/Level Generator Settings.asset";

    // Inspector fields for Level Generator 

    [Header("Level Generator Settings"), SerializeField]
    private LevelGeneratorSettings settings;

    [Header("Json Level"), SerializeField]
    private int levelNumber;

    // Scriptable Wizard Functions

    [MenuItem("GameObject/Memory Lanes/Create Level")]
    private static void CreateWizard()
    {
        DisplayWizard<LevelGenerator>("Level Generator");
    }

    private void Awake()
    {
        settings = AssetDatabase.LoadAssetAtPath<LevelGeneratorSettings>(_pathToSettings);
    }

    /**
     * Perform the following when the Create button is selected in the dialog window.
     */
    void OnWizardCreate()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        Level levelPrefab = null;
        try
        {
            settings.Load();
            
            levelPrefab = CreateLevelPrefab();

            JsonLevel jsonLevel = LoadJsonLevel($"level-{levelNumber}.json");

            GenerateLevelLayout(levelPrefab, jsonLevel);
            SaveLevelPrefab(levelPrefab, jsonLevel);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            if (levelPrefab != null)
            {
                DestroyImmediate(levelPrefab.gameObject);
            }
        }
    }
    
    // Helper Functions

    /**
     * Returns an instance of a GameObject prefab with an attached Level component.
     * The prefab can be changed in the level generator settings.
     */
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

    /**
     * Saves a GameObject prefab with an attached Level component.
     * It will be saved in the levelPrefabsFolder path found in the settings.
     */
    private void SaveLevelPrefab(Level levelPrefab, JsonLevel jsonLevel)
    {
        PrefabUtility.SaveAsPrefabAsset(levelPrefab.gameObject, $"Assets/{settings.levelPrefabsFolder}/{jsonLevel.name}.prefab");
    }

    /**
     * Loads a JsonLevel with the given filename.
     * It will be loaded from the jsonLevelFolder path found in the settings.
     */
    private JsonLevel LoadJsonLevel(string filename)
    {
        string jsonLevelString = AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/{settings.jsonLevelFolder}/{filename}").text;
        return JsonUtility.FromJson<JsonLevel>(jsonLevelString);
    }
    
    /**
     * Builds a Level from a JsonLevel.
     */
    private void GenerateLevelLayout(Level levelPrefab, JsonLevel jsonLevel)
    {
        AudioClip music = AssetDatabase.LoadAssetAtPath<AudioClip>($"Assets/{settings.musicFolder}/{jsonLevel.music}");
        Texture2D tilemap = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/{settings.tilemapFolder}/{jsonLevel.tilemap}");
    
        CreateLevelLayout(levelPrefab, tilemap);
        jsonLevel.ApplyLevelSettings(levelPrefab);

        levelPrefab.music = music;
    }
    
    /**
     * Creates all level objects in a grid fashion from a tilemap image.
     * The bottom left-corner of the level should be (x, y) = (1, 1)
     */
    private void CreateLevelLayout(Level levelPrefab, Texture2D tilemap)
    {
        // Get grid dimensions
        int width = tilemap.width;
        int height = tilemap.height;
    
        Color[] pixels = tilemap.GetPixels();
        int k = 0;
    
        // Create empty grid
        levelPrefab.backgroundObjects = new GameObject[width, height];
        levelPrefab.foregroundObjects = new GameObject[width, height];
    
        // For every row
        for (int j = 0; j < height; j++)
        {
            // For every column
            for (int i = 0; i < width; i++)
            {
                // Get tile position
                Vector3 tilePosition = new Vector3(i, 0f, j);

                LevelTile backgroundTile = settings.GetBackgroundTile(pixels[k]);
                LevelTile foregroundTile = settings.GetForegroundTile(pixels[k]);
                
                levelPrefab.backgroundObjects[i, j] = InstantiateTile(levelPrefab, backgroundTile, tilePosition);
                levelPrefab.foregroundObjects[i, j] = InstantiateTile(levelPrefab, foregroundTile, tilePosition);
    
                k++;
            }
        }
    }

    /**
     * Creates and returns an instance of a tile prefab.
     */
    private GameObject InstantiateTile(Level levelPrefab, LevelTile tile, Vector3 tilePosition)
    {
        // Skip object creation if tile is not specified
        if (tile == null)
        {
            return null;
        }
        
        // Get parent transform for the tile
        Transform parentTransform = GetParentTransform(levelPrefab, tile);

        // Since tile.prefab is a GameObject, tileInstance (a prefab instance) must also be a GameObject.
        GameObject tileInstance = (GameObject)PrefabUtility.InstantiatePrefab(tile.Prefab, parentTransform);
        
        // If required, assign components in tileCopy to the named fields in Level.
        // eg. If tile is a trigger, then add tileCopy to level.triggers.
        AddTileToLevel(levelPrefab, tile, tileInstance);

        // Position tile in world
        tileInstance.transform.position = tilePosition;

        return tileInstance;
    }
    
    /**
     * Choose how to organise tiles in the level prefab.
     */
    public Transform GetParentTransform(Level level, LevelTile tile)
    {
        switch (tile.Type)
        {
            case LevelTileType.Boundary:
                return level.boundaryContainer;

            case LevelTileType.Path:
                return level.pathContainer;

            case LevelTileType.Switch:
            case LevelTileType.Goal:
                return level.triggerContainer;

            case LevelTileType.Door:
            case LevelTileType.Platform:
                return level.activatorContainer;
            
            case LevelTileType.Npc:
                return level.agentContainer;
            
            default:
                return level.transform;
        }
    }
    
    /**
     * Choose how to assign tiles in the level prefab.
     */
    public void AddTileToLevel(Level level, LevelTile tile, GameObject tileInstance)
    {
        switch (tile.Type)
        {
            case LevelTileType.Player:
                level.player = tileInstance.GetComponent<Agent>();
                return;

            case LevelTileType.Goal:
                level.goal = tileInstance.GetComponent<MyGoal>();
                return;

            case LevelTileType.Switch:
                level.triggers.Add(tileInstance.GetComponent<MyTrigger>());
                return;

            case LevelTileType.Door:
            case LevelTileType.Platform:
                level.activators.Add(tileInstance.GetComponent<MyActivator>());
                return;
            
            case LevelTileType.Npc:
                level.npcs.Add(tileInstance.GetComponent<Enemy>());
                return;

            default:
                return;
        }
    }
}
