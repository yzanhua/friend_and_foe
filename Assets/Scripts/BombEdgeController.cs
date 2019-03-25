using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEdgeController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        OnEn();
    }
    void OnEn()
    {
        transform.localScale = Vector3.one * 0.1f;
        //StartCoroutine(ChangeSize());
        Debug.Log("OnEnable");
    }

    private IEnumerator ChangeSize()
    {
        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(1f);
            transform.localScale += Vector3.one * 0.2f;
        }

        yield return new WaitForSeconds(4f);

        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(1f);
            transform.localScale -= Vector3.one * 0.2f;
        }

        Destroy(gameObject);
    }
}
