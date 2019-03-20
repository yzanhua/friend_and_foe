using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticToProxy : MonoBehaviour
{
    public Transform transform_static;
    void Update()
    {
        transform.localPosition = transform_static.localPosition;
    }
}
