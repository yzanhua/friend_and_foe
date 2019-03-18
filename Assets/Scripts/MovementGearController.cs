using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeatOnGear))]
public class MovementGearController : MonoBehaviour
{
    [Range(0f, 2f)]
    public float speed;

    GameObject submarine;
    private SeatOnGear status;
    Rigidbody2D submarine_rb;

    bool inPlaySoundRoutine = false;
    bool previous_status_onseat = false;

    void Start()
    {
        submarine = transform.parent.parent.gameObject;
        status = GetComponent<SeatOnGear>();
        submarine_rb = submarine.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        /*
        if (!status.isPlayerOnSeat())
        {
            bool isRight = transform.position.x > 0f;
            if (TutorialManager.tutorialMode)
            {
                TutorialManager.TaskComplete(2, isRight);
            }
            return;
        }
        else if(TutorialManager.tutorialMode)
        {
            bool isRight = transform.position.x > 0f;
            TutorialManager.TaskComplete(0, isRight);

            if (isRight)
            {
                if (TutorialManager.rightTutorialState <= 1)
                {
                    return;
                }
            }
            else
            {
                if (TutorialManager.leftTutorialState <= 1)
                {
                    return;
                }
            }
        }*/


        // check status (current + previous)
        bool current_status_onseat = status.isPlayerOnSeat();
        if (previous_status_onseat && !current_status_onseat)
            submarine_rb.velocity = Vector2.zero;
        previous_status_onseat = current_status_onseat;
        if (!current_status_onseat)
            return;

        // move if on seat
        int playerID = status.playerID();
        Vector3 temp = new Vector3(InputSystemManager.GetLeftSHorizontal(playerID), InputSystemManager.GetLeftSVertical(playerID));
        temp = temp.normalized;
        submarine_rb.velocity = speed * temp;
        if (temp != Vector3.zero && !inPlaySoundRoutine)
        {
            StartCoroutine(playMoveSound());
        }
    }

    IEnumerator playMoveSound()
    {
        inPlaySoundRoutine = true;
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySound("move");
        yield return new WaitForSeconds(7.8f);
        inPlaySoundRoutine = false;
    }
}
