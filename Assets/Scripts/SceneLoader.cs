using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class SceneLoader : MonoBehaviour
{
    public GameObject LevelSelector;
    public GameObject LevelContent;
    private string playerMinSaves = "0;-1;-1;-1;-1;-1;-1;-1;-1;-1";

    // Stores max number of saves required per star per level
    private List<List<int>> savesRequired = new List<List<int>>();

    // Read information about max number of saves required per star per level
    private void ParseStarInfo()
    {
        string text = File.ReadAllText("./Assets/Resources/starinfo.txt");
        string[] lines = text.Split('\n');

        savesRequired = new List<List<int>>();
        foreach (string line in lines)
        {
            if (line == "") break;
            string[] info = line.Split(' '); // level number, 1 star, 2 star, 3 star
            List<int> saves = new List<int> (new int[] { int.Parse(info[1]), int.Parse(info[2]), int.Parse(info[3]) } );
            savesRequired.Add(saves);
        }
    }

    // Retrieve data about player's smallest ever no. of saves per level
    // -1 indicates the level is locked, 0 indicates the level is unlocked but not attempted
    private void GetPlayerProgress()
    {
        if (PlayerPrefs.HasKey("playerMinSaves"))
        {
            playerMinSaves = PlayerPrefs.GetString("playerMinSaves");
        }
        else
        {
            PlayerPrefs.SetString("playerMinSaves", playerMinSaves);
        }
    }

    // Populate menu stars according to playerMinSaves and savesRequired
    private void PopulateStars()
    {
        int[] minSaves = Array.ConvertAll(playerMinSaves.Split(';'), s=> int.Parse(s));
        int currentLevel = 1;
        foreach (int saves in minSaves)
        {
            List<int> savesNeeded = savesRequired[currentLevel-1];

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
            else if (saves <= savesNeeded[2]) // 3 stars
            {
                level.transform.GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
                level.transform.GetChild(1).gameObject.SetActive(false);
                level.transform.GetChild(2).gameObject.SetActive(true);
                level.transform.GetChild(3).gameObject.SetActive(true);
                level.transform.GetChild(4).gameObject.SetActive(true);
            }
            else if (saves <= savesNeeded[1]) // 2 stars
            {
                level.transform.GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
                level.transform.GetChild(1).gameObject.SetActive(false);
                level.transform.GetChild(2).gameObject.SetActive(true);
                level.transform.GetChild(3).gameObject.SetActive(true);
                level.transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (saves <= savesNeeded[0]) // 1 star
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
        GetPlayerProgress();
        PopulateStars();
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
