using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFlash : MonoBehaviour
{
    //private bool

    // Update is called once per frame


    IEnumerator Flash()
    {
        while (true)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            }
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}
