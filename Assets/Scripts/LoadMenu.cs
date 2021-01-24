using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    public void loadMenu()
    {
        //StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.Out,nextSceneName));
        SceneManager.LoadScene("menu");
    }
}
