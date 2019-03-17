using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeatOnGear))]
public class MovementGearController : MonoBehaviour
{
    [Range(0f, 2f)]
    public float speed;

    Transform submarine;
    private SeatOnGear status;

    bool inPlaySoundRoutine = false;

    void Start()
    {
        submarine = transform.parent.parent;
        status = GetComponent<SeatOnGear>();
    }

    void Update()
    {
        if (!status.isPlayerOnSeat())
            return;
        int playerID = status.playerID();
        Vector3 temp = new Vector3(InputSystemManager.GetLeftSHorizontal(playerID), InputSystemManager.GetLeftSVertical(playerID));
        temp = temp.normalized;
        submarine.position += speed * temp * Time.deltaTime;
        if (temp != Vector3.zero && !inPlaySoundRoutine)
        {
            StartCoroutine(playMoveSound());
        }
    }

    IEnumerator playMoveSound()
    {
        inPlaySoundRoutine = true;
        SoundManager.instance.PlaySound("move");
        yield return new WaitForSeconds(7.8f);
        inPlaySoundRoutine = false;
    }
}
