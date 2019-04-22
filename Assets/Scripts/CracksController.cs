using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CracksController : MonoBehaviour
{
    private GameObject[] cracks;

    private void Start()
    {
        cracks = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            cracks[i] = transform.GetChild(i).gameObject;
            cracks[i].SetActive(false);
        }
    }

    public void UpdateCracks(float damgePercentage)
    {
        int temp = 4 - (int)Mathf.Floor(damgePercentage / 0.2f);
        for (int i = 0; i < temp; i++)
        {
            cracks[i].SetActive(true);
        }
    }

}
