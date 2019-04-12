using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatedShiled : MonoBehaviour
{
    public Transform target;
    public Transform submarine_proxy_transform;

    public bool isAutomated;

    private void Start()
    {
        isAutomated = Global.instance.AutomatedShiled;
    }

    public void GetFakeInput(ref float Xinput, ref float Yinput)
    {
        if (!isAutomated)
            return;
        Vector2 temp = target.position - submarine_proxy_transform.position;
        temp = temp.normalized * 2f;

        Xinput = temp.x;
        Yinput = temp.y;
    }
}
