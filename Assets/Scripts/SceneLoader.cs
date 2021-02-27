using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class SceneLoader : MonoBehaviour
{
    public GameObject LevelSelector;
    public GameObject LevelContent;
    private string playerMinSaves = "0;-1;-1;-1;-1;-1;-1;-1;-1;-1";
    private string PLAYER_MIN_SAVES_KEY = "playerMinSaves";
    private string STAR_INFO_PATH = "/Resources/starinfo.txt";
    private int numLevels = 10; // number of levels in the game (should be read from starInfo.txt)

    struct SaveRequirements
    {
        public int levelNum;
        public int threeStarsReq;
        public int twoStarsReq;
        public int oneStarReq;

        public SaveRequirements(int levelNum, int oneStarReq, int twoStarsReq, int threeStarsReq)
        {
            this.levelNum = levelNum;
            this.threeStarsReq = threeStarsReq;
            this.twoStarsReq = twoStarsReq;
            this.oneStarReq = oneStarReq;
        }
    }

    // Stores max number of saves required per star per level
    private List<SaveRequirements> savesRequired = new List<SaveRequirements>();

    // Read information about max number of saves required per star per level
    private void ParseStarInfo()
    {
        string text = File.ReadAllText(Application.dataPath + STAR_INFO_PATH); // use Application.dataPath for cross-platform compatibility
        string[] lines = text.Split('\n');

        savesRequired = new List<SaveRequirements>();
        foreach (string line in lines)
        {
            if (line == "") break;
            int[] info = Array.ConvertAll<string, int>(line.Split(' '), int.Parse);
            SaveRequirements saves = new SaveRequirements(info[0], info[1], info[2], info[3]); // level number, 1 star, 2 star, 3 star
            savesRequired.Add(saves);
        }
        numLevels = savesRequired.Count;
    }

    // Initialises playerMinSaves to the appropriate length for the number of levels in starInfo
    private void InitPlayerMinSaves()
    {
        int playerMinSavesLen = playerMinSaves.Split(';').Length;
        if (numLevels != 0 && playerMinSavesLen != numLevels) // if level count has changed, reset playerMinSaves
        {
            playerMinSaves = "0;";
            playerMinSaves += string.Concat(Enumerable.Repeat("-1;", numLevels-1));
        }
    }

    // Retrieve data about player's smallest ever no. of saves per level
    // -1 indicates the level is locked, 0 indicates the level is unlocked but not attempted
    private void GetPlayerProgress()
    {
        string existingPlayerMinSaves = "0;";
        if (PlayerPrefs.HasKey(PLAYER_MIN_SAVES_KEY))
            existingPlayerMinSaves = PlayerPrefs.GetString(PLAYER_MIN_SAVES_KEY);

        // use existing playerMinSaves only if it is consistent with numLevels, otherwise reset progress
        if (existingPlayerMinSaves.Split(';').Length == playerMinSaves.Split(';').Length)
            playerMinSaves = existingPlayerMinSaves;

        PlayerPrefs.SetString(PLAYER_MIN_SAVES_KEY, playerMinSaves);
    }


    // Populate menu and stars according to playerMinSaves and savesRequired
    private void PopulateMenu()
    {
        int[] minSaves = Array.ConvertAll<string, int>(playerMinSaves.Substring(0, playerMinSaves.Length-1).Split(';'), int.Parse);
        int currentLevel = 1;
        foreach (int saves in minSaves)
        {
            SaveRequirements savesNeeded = savesRequired[currentLevel-1];

            GameObject level = Instantiate(LevelSelector, LevelContent.transform.position, Quaternion.identity);
            level.transform.parent = LevelContent.transform;
            level.transform.localScale = new Vector3(1, 1, 1);
            level.name = currentLevel.ToString() + "-level" + currentLevel.ToString();

            if (saves == -1) // locked level
            {
                level.transform.GetChild(0).gameObject.SetActive(false);
                level.transform.GetChild(1).gameObject.SetActive(true);
                level.transform.GetChild(2).gameObject.SetActive(false);
                level.transform.GetChild(3).gameObject.SetActive(false);
                level.transform.GetChild(4).gameObject.SetActive(false);
                continue;
            }

            level.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                LoadScene(level.name);
            });
            if (saves == 0) // not attempted
            {
                level.transform.GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
                level.transform.GetChild(1).gameObject.SetActive(false);
                level.transform.GetChild(2).gameObject.SetActive(false);
                level.transform.GetChild(3).gameObject.SetActive(false);
                level.transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (saves <= savesNeeded.threeStarsReq)
            {
                level.transform.GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
                level.transform.GetChild(1).gameObject.SetActive(false);
                level.transform.GetChild(2).gameObject.SetActive(true);
                level.transform.GetChild(3).gameObject.SetActive(true);
                level.transform.GetChild(4).gameObject.SetActive(true);
            }
            else if (saves <= savesNeeded.twoStarsReq)
            {
                level.transform.GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
                level.transform.GetChild(1).gameObject.SetActive(false);
                level.transform.GetChild(2).gameObject.SetActive(true);
                level.transform.GetChild(3).gameObject.SetActive(true);
                level.transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (saves <= savesNeeded.oneStarReq)
            {
                level.transform.GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
                level.transform.GetChild(1).gameObject.SetActive(false);
                level.transform.GetChild(2).gameObject.SetActive(true);
                level.transform.GetChild(3).gameObject.SetActive(false);
                level.transform.GetChild(4).gameObject.SetActive(false);
            }

            currentLevel++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ParseStarInfo();
        InitPlayerMinSaves();
        GetPlayerProgress();
        PopulateMenu();
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
