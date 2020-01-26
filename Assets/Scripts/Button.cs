using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Trigger
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    private GameObject mesh;

    private Color colorForStorage;
    private Vector3 localScaleForStorage;
    // Start is called before the first frame update
    void Start()
    {
        colorForStorage = mesh.GetComponent<MeshRenderer>().material.color;
        localScaleForStorage = mesh.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "aPlayer" | collider.tag == "NPC")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            pressMesh();
            TriggerOn();
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "NPC")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Player Collision Enter");
            TriggerOn();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "aPlayer" | collider.tag == "NPC")
        {
            mesh.GetComponent<MeshRenderer>().material.color = colorForStorage;
            mesh.transform.localScale = localScaleForStorage;
        }
    }

    void pressMesh()
    {
        audioSource.PlayOneShot(audioClip);
        mesh.GetComponent<MeshRenderer>().material.color = mesh.GetComponent<MeshRenderer>().material.color * (float)0.5;
        mesh.transform.localScale = new Vector3(mesh.transform.localScale.x, mesh.transform.localScale.y * 0.5f, mesh.transform.localScale.z);
    }
}
