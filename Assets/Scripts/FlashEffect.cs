using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    public bool isFlash = true;

    public int frameNumber = 15;


    private bool _isEnabled;
    private int _frame_num;

    // Update is called once per frame
    void Update()
    {
        if (isFlash)
        {
            _frame_num ++;

            if (_frame_num == frameNumber)
            {
                AlterChildComponentState(!_isEnabled);
                _frame_num = 0;
                _isEnabled = !_isEnabled;
            }
        }
        else
        {
            _frame_num = 0;
            AlterChildComponentState(true);
        }
    }

    private void AlterChildComponentState(bool state)
    {

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
