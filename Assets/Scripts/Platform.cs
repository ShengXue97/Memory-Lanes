using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : Door
{
    private float localY;
    private void Awake()
    {
        localY = transform.localPosition.y;
    }

    public override void resizeDoor(float TimePercentage)
    {
        // 0 to be open and 1 to be closed
        this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, localY + (TimePercentage - 1), this.gameObject.transform.localPosition.z);
        this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, TimePercentage*2, this.gameObject.transform.localScale.z);
    }
}
