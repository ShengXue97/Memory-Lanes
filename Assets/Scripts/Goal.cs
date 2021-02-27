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

    private string PLAYER_MIN_SAVES_KEY = "playerMinSaves";
    private bool isExiting;
    private double exitingTimeLeft;

    void Start()
    {
        exitingTimeLeft = exitingTime;
    }

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
            // Retrieve playerMinSaves
            string playerMinSaves = PlayerPrefs.GetString(PLAYER_MIN_SAVES_KEY);
            string[] minSaves = playerMinSaves.Split(';');
            GameObject SaveIndicator = GameObject.FindGameObjectWithTag("saveIndicator");
            int saveCount = SaveIndicator.GetComponent<SaveLoadIndicator>().saveCount;

            // Update the min save count
            string prevSave = minSaves[currentScene - 1];
            if (saveCount < int.Parse(prevSave) || prevSave == "0" || prevSave == "-1")
                minSaves[currentScene-1] = saveCount.ToString();

            // Unlock the next level
            if (currentScene < minSaves.Length)
                minSaves[currentScene] = "0";

            playerMinSaves = string.Join(";", minSaves);
            PlayerPrefs.SetString(PLAYER_MIN_SAVES_KEY, playerMinSaves);

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
