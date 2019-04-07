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

    private float radius;

    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        //init_distance = Vector3.Distance(transform.position, transform.parent.position);
        radius = submarine_proxy.GetComponent<CircleCollider2D>().radius * submarine_proxy.transform.localScale.x;
        radius *= submarine_proxy.transform.parent.localScale.x;
        StartCoroutine(CreateBomb());
    }

    private IEnumerator CreateBomb()
    {
        yield return new WaitForSeconds(Random.Range(MinCreateInterval, MaxCreateInterval));

        while (true)
        {
            if (Global.instance.isGameEnd)
                break;

            GameObject targetSign = Instantiate(targetSign_prefab);
            targetSign.transform.position = submarine_proxy.transform.position + new Vector3(radius, 0f, 0f) * 1.2f;
            targetSign.transform.parent = null;
            targetSign.transform.RotateAround(submarine_proxy.transform.position, Vector3.forward, Random.Range(0f, 365f));

            // SoundManager.instance.PlaySound("bomb_bell");
            yield return new WaitForSeconds(3.5f);
            Destroy(targetSign, 0.2f);
            if (Global.instance.isGameEnd)
                break;

            GameObject new_bomb =  Instantiate(Bomb_prefab, targetSign.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(MinCreateInterval, MaxCreateInterval));
        }
    }

}
