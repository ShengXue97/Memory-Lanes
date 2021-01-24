using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject LevelSelector;
    public GameObject LevelContent;
    private string starprogress = "";

    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.HasKey("starprogress"))
        {
            starprogress = PlayerPrefs.GetString("starprogress");
        }
        else
        {
            starprogress = "0;-1;-1;-1;-1;-1;-1;-1;-1;-1";
            PlayerPrefs.SetString("starprogress", starprogress);
        }

        string[] starSplit = starprogress.Split(';');
        int currentLevel = 1;
        foreach (string star in starSplit)
        {
            GameObject level = Instantiate(LevelSelector, LevelContent.transform.position, Quaternion.identity);

            level.transform.parent = LevelContent.transform;
            level.transform.localScale = new Vector3(1, 1, 1);

            if (star == "-1")
            {
                level.name = currentLevel.ToString() + "-level" + currentLevel.ToString();
                level.transform.GetChild(0).gameObject.SetActive(false);
                level.transform.GetChild(1).gameObject.SetActive(true);
                level.transform.GetChild(2).gameObject.SetActive(false);
                level.transform.GetChild(3).gameObject.SetActive(false);
                level.transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (star == "0")
            {
                level.name = currentLevel.ToString() + "-level" + currentLevel.ToString();
                level.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    LoadScene(level.name);
                });

                level.transform.GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
                level.transform.GetChild(1).gameObject.SetActive(false);
                level.transform.GetChild(2).gameObject.SetActive(false);
                level.transform.GetChild(3).gameObject.SetActive(false);
                level.transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (star == "1")
            {
                level.name = currentLevel.ToString() + "-level" + currentLevel.ToString();
                level.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    LoadScene(level.name);
                });
                level.transform.GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
                level.transform.GetChild(1).gameObject.SetActive(false);
                level.transform.GetChild(2).gameObject.SetActive(true);
                level.transform.GetChild(3).gameObject.SetActive(false);
                level.transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (star == "2")
            {
                level.name = currentLevel.ToString() + "-level" + currentLevel.ToString();
                level.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    LoadScene(level.name);
                });
                level.transform.GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
                level.transform.GetChild(1).gameObject.SetActive(false);
                level.transform.GetChild(2).gameObject.SetActive(true);
                level.transform.GetChild(3).gameObject.SetActive(true);
                level.transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (star == "3")
            {
                level.name = currentLevel.ToString() + "-level" + currentLevel.ToString();
                level.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    LoadScene(level.name);
                });
                level.transform.GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
                level.transform.GetChild(1).gameObject.SetActive(false);
                level.transform.GetChild(2).gameObject.SetActive(true);
                level.transform.GetChild(3).gameObject.SetActive(true);
                level.transform.GetChild(4).gameObject.SetActive(true);
            }

            currentLevel++;
        }
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
