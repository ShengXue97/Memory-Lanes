using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activator
{
    [SerializeField]
    private float CloseTime = 1.0f;
    [SerializeField]
    private float OpenTime = 1.0f;

    private bool isOpening = false;
    private bool isClosing = false;
    private float openOrClose = 0;

    private float timeLeft = 0;
    public bool isOpen;

    private Vector3 originalPosition;

    void Start()
    {
        openOrClose = isOpen ? 0 : 1;
        isOpening = isOpen;
        isClosing = !isOpen;
        originalPosition = transform.localPosition;
        if (isOpen)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<Collider>().enabled = false;
        } else
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            this.gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float openSpeed = 1 / OpenTime;
        float closeSpeed = 1 / CloseTime;
        openOrClose = Mathf.Clamp01(openOrClose + (isOpening ? -openSpeed : closeSpeed) * Time.deltaTime);
        if (openOrClose == 0)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<Collider>().enabled = false;
        }

        resizeDoor(openOrClose);
    }

    public void Trigger()
    {
        if (isOpen)
        {
            isOpen = false;
            TriggerOff();
        } else
        {
            isOpen = true;
            TriggerOn();
        }
    }

    public override void TriggerOn()
    {
        Open();
    }

    public override void TriggerOff()
    {
        Close();
    }

    public void Open()
    {
        
        isOpen = true;
        isOpening = true;
        timeLeft = OpenTime;
    }

    public void Close()
    {   
        isOpen = false;
        isOpening = false;
        timeLeft = CloseTime;
        this.gameObject.GetComponent<Collider>().enabled = true;
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public virtual void resizeDoor(float TimePercentage)
    {
        // 0 to be open and 1 to be closed
        float angle = transform.rotation.eulerAngles.y * Mathf.Deg2Rad + Mathf.PI/2;
        Vector3 direction = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

        transform.localPosition = originalPosition + direction * (TimePercentage - 1) * 0.5f;
        //this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y, localZ + (TimePercentage - 1) * 0.5f);
        this.gameObject.transform.localScale = new Vector3(TimePercentage, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
    }
}
