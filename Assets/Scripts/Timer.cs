using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField]
    private bool Loop = false;
    public bool active = true;
    [SerializeField]
    private float activeTime = 1.0f;
    [SerializeField]
    private float inactiveTime = 1.0f;

    private float TimeLeft;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(active);
        TimeLeft = active ? activeTime : inactiveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeLeft < 0)
        {
            active = !active;
            this.gameObject.SetActive(active);
            TimeLeft = active ? activeTime : inactiveTime;
        } else
        {
			TimeLeft -= Time.deltaTime;
        }
    }
}
