using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(test());
    }

    // Update is called once per frame
    void Update()
    {

        if (InputSystemManager.GetAction1(0))
        {
            Time.timeScale = 1f;
        }
    }


    IEnumerator test()
    {
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0f;
    }
}
