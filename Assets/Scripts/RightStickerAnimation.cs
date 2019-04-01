using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightStickerAnimation : MonoBehaviour
{
    public Sprite emptySticker;

    public Sprite fullSticker;

    public bool isFlash;

    public int frameNumber = 15;


    private bool _isFull;
    private int _frame_num;

    // Update is called once per frame
    void Update()
    {
        if (isFlash)
        {
            _frame_num++;

            if (_frame_num == frameNumber)
            {
                if (_isFull)
                {
                    AlterImage(emptySticker);
                }
                else
                {
                    AlterImage(fullSticker);
                }

                _frame_num = 0;
                _isFull = !_isFull;
            }
        }
        else
        {
            _frame_num = 0;
            AlterImage(emptySticker);
        }
    }
     
    private void AlterImage(Sprite im)
    {
        GetComponent<Image>().sprite = im;
    }
}
