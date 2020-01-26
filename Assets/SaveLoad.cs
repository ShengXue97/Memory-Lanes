using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    SaveLoadIndicator control;
    // Start is called before the first frame update
    void Start()
    {
        control = FindObjectOfType<SaveLoadIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(){
        control.Save();
    }

    public void Load(){
        control.Load();
    }
}
