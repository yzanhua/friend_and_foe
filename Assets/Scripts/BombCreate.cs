using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCreate : MonoBehaviour
{
    public float MinCreateInterval = 15f;
    public float MaxCreateInterval = 25f;
    public GameObject Bomb_prefab;
    public GameObject targetSign_prefab;
    public GameObject submarine_proxy;
    // public int submarine_id = 1;

    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        //init_distance = Vector3.Distance(transform.position, transform.parent.position);
        StartCoroutine(CreateBomb());
    }

    private IEnumerator CreateBomb()
    {
        yield return new WaitForSeconds(Random.Range(MinCreateInterval, MaxCreateInterval));

        while (true)
        {
            if (Global.instance.isGameEnd)
                break;
            Transform target_transform = transform;
            target_transform.position = submarine_proxy.transform.position + new Vector3(2.44f, 1.15f);
            target_transform.parent = null;
            target_transform.RotateAround(submarine_proxy.transform.position, Vector3.forward, Random.Range(0f, 365f));
            GameObject targetSign = Instantiate(targetSign_prefab, target_transform);
            // SoundManager.instance.PlaySound("bomb_bell");
            yield return new WaitForSeconds(2.5f);
            Destroy(targetSign, 0.2f);
            if (Global.instance.isGameEnd)
                break;
            GameObject new_bomb =  Instantiate(Bomb_prefab, target_transform);
            yield return new WaitForSeconds(Random.Range(MinCreateInterval, MaxCreateInterval));
        }
    }

}
