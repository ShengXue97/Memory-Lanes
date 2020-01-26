using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartLevel()
    {
        //StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.Out,nextSceneName));
        SceneManager.LoadScene(nextSceneName);
    }
}
