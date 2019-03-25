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

    private SeatOnGear status;
    private Rigidbody2D submarine_rb;
    private SpriteRenderer rend;

    bool inPlaySoundRoutine = false;
    bool previous_status_onseat = false;

    void Start()
    {
        status = GetComponent<SeatOnGear>();
        submarine_rb = submarine.GetComponent<Rigidbody2D>();
        rend = movementGeat.GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        // check status (current + previous)
        bool current_status_onseat = status.isPlayerOnSeat();
        if (previous_status_onseat && !current_status_onseat)
            submarine_rb.velocity = Vector2.zero;
        previous_status_onseat = current_status_onseat;
        if (!current_status_onseat)
        {
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0.3f);
            return;
        }
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1.0f);

        if (TutorialManager.instance != null)
        {
            bool isRight = transform.position.x > 0f;
            TutorialManager.CompleteTask(TutorialManager.TaskType.SEAT, isRight);
        }

        // move if on seat
        int playerID = status.playerID();
        Vector2 temp = new Vector2(InputSystemManager.GetLeftSHorizontal(playerID), InputSystemManager.GetLeftSVertical(playerID));
        temp = temp.normalized;
        submarine_rb.AddForce(speed * temp * submarine_rb.mass * 2f);
        if (temp != Vector2.zero && !inPlaySoundRoutine)
            StartCoroutine(playMoveSound());
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
