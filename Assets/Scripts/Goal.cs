using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private bool isRotating = true;
    [SerializeField]
    private double rotateSpeed = 30;
    [SerializeField]
    private int currentScene;
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private double exitingTime = 2.0;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;

    private bool isExiting;
    private double exitingTimeLeft;
    private Hashtable starInfo = new Hashtable();

    // Read information about number of saves per star required of each level
    private void ParseStarInfo()
    {
        string text = File.ReadAllText("./Assets/Resources/starinfo.txt");
        string[] lines = text.Split('\n');

        starInfo = new Hashtable();
        foreach (string line in lines)
        {
            string[] info = line.Split(' '); // level number, 1 star, 2 star, 3 star
            int[] saves = { int.Parse(info[1]), int.Parse(info[2]), int.Parse(info[3]) };
            starInfo.Add(info[0], saves);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        exitingTimeLeft = exitingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            this.gameObject.transform.Rotate(new Vector3(0, 1, 0), (float)rotateSpeed * Time.deltaTime);
        }
        if (isExiting)
        {
            exitingTimeLeft -= Time.deltaTime;
        }
        if (exitingTimeLeft < 0)
        {
            ParseStarInfo();
            string starprogress = "-1;-1;-1;-1;-1;-1;-1;-1;-1;-1"; // stores stars information for every level, -1 indicates uncompleted

            if (PlayerPrefs.HasKey("starprogress"))
            {
                starprogress = PlayerPrefs.GetString("starprogress");
            }
            string[] starSplit = starprogress.Split(';');
            int currentLevel = 1;
            string newStarProgress = "";

            // Update star progress information each time user enters the goal
            foreach (string star in starSplit)
            {
                if (currentLevel == currentScene)
                {
                    GameObject SaveIndicator = GameObject.FindGameObjectWithTag("saveIndicator");
                    int saveCount = SaveIndicator.GetComponent<SaveLoadIndicator>().saveCount;
                    int newStars = 0;
                    var savesNeeded = (int[]) starInfo[currentLevel.ToString()];
                    if (saveCount <= savesNeeded[2])
                    {
                        newStars = 3;
                    }
                    else if (saveCount <= savesNeeded[1])
                    {
                        newStars = 2;
                    }
                    else
                    {
                        newStars = 1;
                    }
                    int oldStars = int.Parse(star);

                    if (newStars > oldStars)
                    {
                        newStarProgress += newStars.ToString() + ";";
                    }
                    else
                    {
                        newStarProgress += oldStars.ToString() + ";";
                    }
                }
                else if (currentLevel == currentScene + 1)
                {
                    if (star == "-1")
                    {
                        newStarProgress += "0;";
                    }
                    else
                    {
                        newStarProgress += star + ";";
                    }

                }
                else
                {
                    newStarProgress += star + ";";
                }

                currentLevel++;
            }
            newStarProgress = newStarProgress.Substring(0, newStarProgress.Length - 1);
            PlayerPrefs.SetString("starprogress", newStarProgress);

            SceneManager.LoadScene(nextSceneName);
        }

        if (Input.GetKey("-"))
        {
            audioSource.PlayOneShot(audioClip);
            // Go to the next Scene
            isExiting = true;
            player.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioClip);
            // Go to the next Scene
            isExiting = true;
            player.SetActive(false);
        }
    }
}
