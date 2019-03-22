using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningController : MonoBehaviour
{
    public Material glowMaterial;

    private SpriteRenderer rend;
    private Material orgMaterial;
    private Color dimColor;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        dimColor = rend.color;
        orgMaterial = rend.material;
        StartCoroutine(Flash());
    }

    void OnEnable()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        while (true)
        {
            rend.material = glowMaterial;
            rend.color = Color.white;
            yield return new WaitForSeconds(0.3f);
            rend.material = orgMaterial;
            rend.color = dimColor;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
