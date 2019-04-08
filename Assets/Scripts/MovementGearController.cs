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
    public int maxDash = 10;
    public HealthBar healthBar;

    private SeatOnGear status;
    private Rigidbody2D submarine_rb;
    private SpriteRenderer rend;
    private bool inPlaySoundRoutine = false;
    //private bool dashOK = true;
    private int availableDash;
    private float dashInterval;

    void Start()
    {
        status = GetComponent<SeatOnGear>();
        submarine_rb = submarine.GetComponent<Rigidbody2D>();
        rend = movementGeat.GetComponent<SpriteRenderer>();
        availableDash = maxDash;
    }

    void Update()
    {
        if (!Global.instance.AllPlayersMovementEnable)
            return;

        // increase dash health
        dashInterval += Time.deltaTime;
        if (dashInterval > DashCD && availableDash < maxDash)
        {
            availableDash += 1;
            dashInterval = 0;
        }
        healthBar.SetSize(Health());

        if (!status.IsPlayerOnSeat())
            return;
        // tutorial
        if (TutorialManager.instance != null)
        {
            bool isRight = transform.position.x > 0f;
            TutorialManager.CompleteTask(TutorialManager.TaskType.SEAT, isRight);
        }

        // move if on seat
        int playerID = status.PlayerID();
        Vector2 temp = new Vector2(InputSystemManager.GetLeftSHorizontal(playerID), InputSystemManager.GetLeftSVertical(playerID));
        temp = temp.normalized;
        // dash
        if (availableDash > 0 && InputSystemManager.GetAction1(playerID) && temp.magnitude > 0f)
        {
            submarine_rb.AddForce(temp * submarine_rb.mass * 20f, ForceMode2D.Impulse);
            availableDash -= 1;
            if (TutorialManager.instance != null)
            {
                bool isRight = transform.position.x > 0f;
                TutorialManager.CompleteTask(TutorialManager.TaskType.DASH_SUB, isRight);
            }
            //StartCoroutine(WaitDashCD());
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

    //IEnumerator WaitDashCD()
    //{
    //    yield return new WaitForSeconds(DashCD);
    //    dashOK = true;
    //}

    IEnumerator playMoveSound()
    {
        inPlaySoundRoutine = true;
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySound("move");
        yield return new WaitForSeconds(7.8f);
        inPlaySoundRoutine = false;
    }

    private float Health()
    {
        return (float)availableDash / maxDash;
    }
}
