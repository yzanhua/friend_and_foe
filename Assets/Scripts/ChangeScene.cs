using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public float targetTime = 100000000f;
    private float currTime;
    // Start is called before the first frame update

    private Transform gear1;
    private Transform gear2;
    private Transform gear3;


    void Start()
    {
        gear1 = transform.GetChild(0);
        gear2 = transform.GetChild(1);
        gear3 = transform.GetChild(2);
        currTime = 0f;
        StartCoroutine(CountTime());
    }

    public void changeGearsPositions()
    {
        Vector3 temp_pos = gear1.position;
        gear1.position = gear2.position;
        gear2.position = gear3.position;
        gear3.position = temp_pos;
    }

    void check()
    {
        if (currTime >= targetTime)
        {
            changeGearsPositions();
            currTime = 0f;
        }
    }

    IEnumerator CountTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currTime += 1f;
            check();
        }
    }


}
