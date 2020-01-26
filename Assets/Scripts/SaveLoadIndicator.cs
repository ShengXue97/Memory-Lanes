using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveLoadIndicator : MonoBehaviour
{
    [Header("camera")]
    [SerializeField]
    private Camera mainCamera;
    [Header("Material")]
    [SerializeField]
    private Material savedMat1;
    [SerializeField]
    private Material savedMat2;
    [SerializeField]
    private Material emptyMat1;
    [SerializeField]
    private Material emptyMat2;
    [SerializeField]
    private GameObject indicatorMesh;
    [SerializeField]
    private ParticleSystem effectSave;
    [SerializeField]
    private ParticleSystem effectLoad;

    [SerializeField]
    private TextMeshPro text;
    [Header("Player settings for save and load")]
    [SerializeField]
    private PlayerMovement player;
    private bool isSaved = false;
    private RaycastHit hit;
    private Material[] materials;
    private int saveCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        isSaved = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "loadIndicator") Load();
                else if (hit.collider.gameObject.tag == "saveIndicator") Save();
            }
        } 
    }

    public void Save()
    {
        isSaved = true;
        player.saveState();
        materials = indicatorMesh.GetComponent<Renderer>().materials;
        materials[0] = savedMat1;
        materials[1] = savedMat2;
        indicatorMesh.GetComponent<Renderer>().materials = materials;
        effectSave.gameObject.SetActive(true);
        effectSave.Play();
        saveCount++;
        text.text = saveCount.ToString();
    }

    public void Load()
    {
        if(isSaved)
        {
            player.loadState();
            materials = indicatorMesh.GetComponent<Renderer>().materials;
            materials[0] = emptyMat1;
            materials[1] = emptyMat2;
            indicatorMesh.GetComponent<Renderer>().materials = materials;
            effectLoad.gameObject.SetActive(true);
            effectLoad.Play();
            isSaved = false;
        }  
    }
}
