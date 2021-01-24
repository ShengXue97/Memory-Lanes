using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "Level Database", menuName = "Level Database", order = 0)]
public class LevelDatabase : ScriptableObject
{
    [SerializeField]
    private TextAsset jsonFile;
    
    private List<string> levels;
    private Dictionary<string, int> levelNamesByNumber;

    private void OnEnable()
    {
        JsonLevelDatabase json = JsonUtility.FromJson<JsonLevelDatabase>(jsonFile.text);
        levels = json.GetLevels();

        levelNamesByNumber = new Dictionary<string, int>();
        for (int i = 0; i < levels.Count; i++)
        {
            levelNamesByNumber.Add(levels[i], i);   
        }
    }

    public string GetPreviousLevel(string levelName)
    {
        int levelNumber = levelNamesByNumber[levelName] - 1;
        return levels[levelNumber];
    }

    public string GetNextLevel(string levelName)
    {
        int levelNumber = levelNamesByNumber[levelName] + 1;
        return levels[levelNumber];
    }
}
