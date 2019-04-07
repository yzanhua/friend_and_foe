using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeatOnGear))]
public class MovementGearController : MonoBehaviour
{
    [Range(0f, 10f)]
    public float speed;

    public GameObject submarine;
    public GameObject movementGeat;
    public float DashCD = 0.5f;

    private SeatOnGear status;
    private Rigidbody2D submarine_rb;
    private SpriteRenderer rend;

    bool inPlaySoundRoutine = false;

    bool dashOK = true;

    void Start()
    {
        status = GetComponent<SeatOnGear>();
        submarine_rb = submarine.GetComponent<Rigidbody2D>();
        rend = movementGeat.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!Global.instance.AllPlayersMovementEnable)
            return;
        if (!status.IsPlayerOnSeat())
        {
            //rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0.3f);
            return;
        }
        //rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1.0f);

        if (TutorialManager.instance != null)
        {
            bool isRight = transform.position.x > 0f;
            TutorialManager.CompleteTask(TutorialManager.TaskType.SEAT, isRight);
        }

        // move if on seat
        int playerID = status.PlayerID();
        Vector2 temp = new Vector2(InputSystemManager.GetLeftSHorizontal(playerID), InputSystemManager.GetLeftSVertical(playerID));
        temp = temp.normalized;

        if (dashOK && InputSystemManager.GetAction1(playerID) && temp.magnitude > 0f)
        {// dash
            submarine_rb.AddForce(temp * submarine_rb.mass * 20f, ForceMode2D.Impulse);
            dashOK = false;
            if (TutorialManager.instance != null)
            {
                bool isRight = transform.position.x > 0f;
                TutorialManager.CompleteTask(TutorialManager.TaskType.DASH_SUB, isRight);
            }
            StartCoroutine(WaitDashCD());
        }

        if (TutorialManager.instance != null)
        {
            if (temp.magnitude > 0f)
            {
                bool isRight = transform.position.x > 0f;
                TutorialManager.CompleteTask(TutorialManager.TaskType.MOVE, isRight);
            }
        }

        submarine_rb.AddForce(speed * temp * submarine_rb.mass * 2f);
        if (temp != Vector2.zero && !inPlaySoundRoutine)
            StartCoroutine(playMoveSound());
    }

    IEnumerator WaitDashCD()
    {
        yield return new WaitForSeconds(DashCD);
        dashOK = true;
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
