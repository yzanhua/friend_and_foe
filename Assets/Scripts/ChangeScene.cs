using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public float targetTime = 10f;
    public int flashCount = 3;
    public int gearNum = 3;

    private float currTime;
    private Transform[] gears_transforms;
    private Transform[] proxy_gears_transforms;
    private SeatOnGear[] seatOnGears;

    void Start()
    {
        gears_transforms = new Transform[gearNum];
        proxy_gears_transforms = new Transform[gearNum];
        seatOnGears = new SeatOnGear[gearNum];
        for (int i = 0; i < gearNum; ++i)
        {
            gears_transforms[i] = transform.GetChild(i);
            proxy_gears_transforms[i] = gears_transforms[i].GetComponent<SeatOnGear>().proxy_gear.transform;
            seatOnGears[i] = gears_transforms[i].GetComponent<SeatOnGear>();
        }
        currTime = 0f;
        StartCoroutine(CountTime());
    }

    IEnumerator CountTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currTime += 1f;
            if (currTime >= targetTime)
            {
                StartCoroutine(ChangeGearsPositions());
                currTime = 0f;
            }
        }
    }

    IEnumerator ChangeGearsPositions()
    {
        StartCoroutine(GameController.instance.SwitchStation());
        for (int t = 0; t < flashCount; t += 1)
        {
            SetSpriteStatus(false);
            yield return new WaitForSeconds(0.3f);
            SetSpriteStatus(true);
            yield return new WaitForSeconds(0.3f);
        }

        Vector3 temp_pos = gears_transforms[0].position;
        gears_transforms[0].position = gears_transforms[1].position;
        gears_transforms[1].position = gears_transforms[2].position;
        gears_transforms[2].position = temp_pos;
        InformEachGear();
    }

    private void InformEachGear()
    {
        for (int i = 0; i < gearNum; ++i)
        {
            seatOnGears[i].GearPostionChanged();
        }
    }

    private void SetSpriteStatus(bool status)
    {
        for (int i = 0; i < gearNum; ++i)
        {
            Transform t = proxy_gears_transforms[i];
            if (t.GetComponent<SpriteRenderer>())
            {
                t.GetComponent<SpriteRenderer>().enabled = status;
            }
            for (int j = 0; j < t.childCount; ++j)
            {
                if (t.GetChild(j).GetComponent<SpriteRenderer>())
                {
                    t.GetChild(j).GetComponent<SpriteRenderer>().enabled = status;
                }
            }
        }  
    }
}
