using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "Level Database", menuName = "Level Database", order = 0)]
public class LevelDatabase : ScriptableObject
{
    [SerializeField]
    private string[] levelNames;
    private Dictionary<string, int> levelIdsByName;

    private void OnEnable()
    {
        levelIdsByName = new Dictionary<string, int>();

        if (levelNames == null)
        {
            return;
        }
        
        for (int i = 0; i < levelNames.Length; i++)
        {
            levelIdsByName[levelNames[i]] = i;
        }
    }
    
    public string GetPreviousLevel(string levelName)
    {
        int levelId = levelIdsByName[levelName] - 1;
        return levelNames[levelId];
    }

    public string GetNextLevel(string levelName)
    {
        int levelId = levelIdsByName[levelName] + 1;
        return levelNames[levelId];
    }
}
