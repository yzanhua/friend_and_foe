using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Text timer;
    [Range(0, 9)]
    public int start_time = 5;

    private int _min;
    private int _second;

    void Start()
    {
        _min = start_time;
        _second = 0;
        StartCoroutine(countDown());
    }

    IEnumerator countDown()
    {
        while(true) {
            if (_second == 0)
            {
                _min--;
                _second = 59;
            }
            else
            {
                _second--;
            }

            if (_min == 0 && _second == 0)
            {
                // Game end call game controller
                GameController.instance.GameEnd();
                break;
            }

            if (_second < 10)
            {
                timer.text = "0" + _min + ":0" + _second;
            }
            else
            {
                timer.text = "0" + _min + ":" + _second;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
