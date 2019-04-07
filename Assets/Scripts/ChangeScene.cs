using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public float targetTime = 10f;
    public int flashCount = 3;
    public int gearNum = 3;

    private float currTime;
    private Transform[] gears;

    void Start()
    {
        gears = new Transform[3];
        for (int i = 0; i < gearNum; ++i)
        {
            gears[i] = transform.GetChild(i);
        }
        currTime = 0f;
        StartCoroutine(CountTime());
    }

    void check()
    {
        if (currTime >= targetTime)
        {
            StartCoroutine(ChangeGearsPositions());
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

    IEnumerator ChangeGearsPositions()
    {
        StartCoroutine(GameController.instance.SwitchStation());
        for (int t = 0; t < flashCount; t += 1)
        {
            for (int i = 0; i < gearNum; ++i)
            {
                SetSpriteStatus(gears[i].GetComponent<SeatOnGear>().station.transform, false);
            }
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < gearNum; ++i)
            {
                //gears[i].GetComponent<SpriteRenderer>().enabled = true;
                SetSpriteStatus(gears[i].GetComponent<SeatOnGear>().station.transform, true);
            }
            yield return new WaitForSeconds(0.3f);
        }


        Vector3 temp_pos = gears[0].position;
        gears[0].position = gears[1].position;
        gears[1].position = gears[2].position;
        gears[2].position = temp_pos;
    }

    private void SetSpriteStatus(Transform t, bool status)
    {
        if (t.GetComponent<SpriteRenderer>())
        {
            t.GetComponent<SpriteRenderer>().enabled = status;
        }
        for (int i = 0; i < t.childCount; ++i)
        {
            if (t.GetChild(i).GetComponent<SpriteRenderer>())
            {
                t.GetChild(i).GetComponent<SpriteRenderer>().enabled = status;
            }
        }
    }
}
