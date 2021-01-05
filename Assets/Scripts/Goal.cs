using System.Collections;
using System.Collections.Generic;
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
            string starprogress = "-1;-1;-1;-1;-1;-1;-1;-1;-1;-1";

            if (PlayerPrefs.HasKey("starprogress"))
            {
                starprogress = PlayerPrefs.GetString("starprogress");
            }
            string[] starSplit = starprogress.Split(';');
            int currentLevel = 1;
            string newStarProgress = "";

            foreach (string star in starSplit)
            {
                if (currentLevel == currentScene)
                {
                    GameObject SaveIndicator = GameObject.FindGameObjectWithTag("saveIndicator");
                    int saveCount = SaveIndicator.GetComponent<SaveLoadIndicator>().saveCount;
                    int newScore = 0;
                    if (saveCount <= 1)
                    {
                        newScore = 3;
                    }
                    else if (saveCount <= 2)
                    {
                        newScore = 2;
                    }
                    else
                    {
                        newScore = 1;
                    }
                    int oldScore = int.Parse(star);

                    if (newScore > oldScore)
                    {
                        newStarProgress += newScore.ToString() + ";";
                    }
                    else
                    {
                        newStarProgress += oldScore.ToString() + ";";
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
