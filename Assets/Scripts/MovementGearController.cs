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

    void Start()
    {
        submarine = transform.parent.parent.gameObject;
        status = GetComponent<SeatOnGear>();
        submarine_rb = submarine.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!status.isPlayerOnSeat())
            return;
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
        //SoundManager.instance.PlaySound("move");
        yield return new WaitForSeconds(7.8f);
        inPlaySoundRoutine = false;
    }
}
