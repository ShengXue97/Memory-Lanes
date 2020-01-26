using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum MapType
{
    Boundary,
    Walking,
    Door
}
public class TileHelper : MonoBehaviour
{
    [SerializeField]
    private int width = 32;
    [SerializeField]
    private int height = 18;
    [SerializeField]
    private Button tileButton;

    private 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
