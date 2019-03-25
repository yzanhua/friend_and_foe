using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCreate : MonoBehaviour
{
    public float MinCreateInterval = 30f;
    public float MaxCreateInterval = 45f;
    public GameObject Bomb_prefab;
    public GameObject submarine_proxy;
    public int submarine_id = 1;

    private void Start()
    {
        //init_distance = Vector3.Distance(transform.position, transform.parent.position);
        StartCoroutine(CreateBomb());
    }

    private IEnumerator CreateBomb()
    {
        yield return new WaitForSeconds(MinCreateInterval * (2 - submarine_id) * 0.5f);
        while (true)
        {
            GameObject new_bomb =  Instantiate(Bomb_prefab);
            new_bomb.transform.position = submarine_proxy.transform.position + new Vector3(2.77f, 1.15f);
            new_bomb.transform.parent = null;
            new_bomb.transform.RotateAround(submarine_proxy.transform.position, Vector3.forward, Random.Range(0f, 365f));
            while (new_bomb != null)
            {
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(Random.Range(MinCreateInterval, MaxCreateInterval));
        }
    }

}
