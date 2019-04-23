using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    public bool isFlash = true;

    private float second = 0.4f;


    private bool _isEnabled;
    private float _frame_num;

    // Update is called once per frame
    void Update()
    {
        if (isFlash)
        {
            _frame_num += Time.deltaTime;

            if (_frame_num > second)
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
