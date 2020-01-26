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
        if(isRotating)
        {
            this.gameObject.transform.Rotate(new Vector3(0, 1, 0), (float)rotateSpeed * Time.deltaTime);
        }
        if(isExiting)
        {
            exitingTimeLeft -= Time.deltaTime;
        }
        if(exitingTimeLeft < 0)
        {
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
        audioSource.PlayOneShot(audioClip);
        // Go to the next Scene
        isExiting = true;
        player.SetActive(false);
    }
}
