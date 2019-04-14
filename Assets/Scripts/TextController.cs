using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    Text text;
    Animator anim;

    private void Start()
    {
        text = GetComponent<Text>();
        anim = GetComponent<Animator>();
    }

    public void SetText(string newText)
    {
        anim.SetTrigger("TextChange");
        text.text = newText;
    }
}
