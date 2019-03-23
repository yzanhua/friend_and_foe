using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
