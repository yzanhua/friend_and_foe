using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeController : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public GameObject up;
    public GameObject down;

    // Update is called once per frame
    void Start()
    {
        Vector3 temp = Vector3.zero;

        // Left position
        temp.x = -Global.instance.maxScreenSize / 9f * 16f - 0.5f;
        left.transform.position = temp;

        // Right position
        temp.x = -temp.x;
        right.transform.position = temp;

        // Up Down scale
        temp.x *= 2f;
        temp.z = 1f;
        temp.y = 1f;
        up.transform.localScale = temp;
        down.transform.localScale = temp;

        // down position:
        temp = Vector3.zero;
        temp.y = -Global.instance.maxScreenSize - 0.5f;
        down.transform.position = temp;

        // up position
        temp *= -1f;
        up.transform.position = temp;

        // left right scale
        temp.y *= 2f;
        temp.z = 1f;
        temp.x = 1f;
        left.transform.localScale = temp;
        right.transform.localScale = temp;


    }
}
