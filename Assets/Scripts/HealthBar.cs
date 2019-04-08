using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public bool isHealth = false;

    private Transform _bar;
    private bool isFlash = false;

    void Awake()
    {
        _bar = transform.Find("Bar");
    }

    public void SetSize(float sizeNormalized)
    {
        if (isHealth)
        {
            if (!isFlash && _bar.localScale.x > sizeNormalized)
            {
                StartCoroutine(Flash(0.5f));
            }
        }
        _bar.localScale = new Vector3(sizeNormalized, 1f);

    }

    public void SetColor(Color color)
    {
        _bar.Find("BarSprite").GetComponent<Image>().color = color;
    }

    IEnumerator Flash(float duration)
    {
        Image sr = _bar.Find("BarSprite").GetComponent<Image>();
        Color originColor = sr.color;
        bool isRed = true;
        isFlash = true;

        while (duration > 0f)
        {
            if (isRed)
            {
                sr.color = new Color(1f, 0f, 0f, 1f);
                isRed = false;
            }
            else
            {
                sr.color = originColor;
                isRed = true;
            }
            duration -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        isFlash = false;
        sr.color = originColor;

    }
}
