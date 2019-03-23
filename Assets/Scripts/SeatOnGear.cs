using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatOnGear : MonoBehaviour
{
    private bool playerOnSeat = false;
    private PlayerMovementController player;
    private GameObject playerGameObject;
    private bool triggerStay = false;
    private float initGravityScale;
    private Vector3 offset;
    private bool inLiftProgress = false;

    private Vector3 targetPos;

    private void Update()
    {
        if (!triggerStay)
            return;

        if (!playerOnSeat)
        {
            if (InputSystemManager.GetAction2(player.playerID))
            {
                playerOnSeat = true;
                player.movementEnable = false;
                StartCoroutine(LiftUp());
                //player.SeatedOnGear = true;
            }
        }
        else if (InputSystemManager.GetAction1(player.playerID))
            Exit();
    }

    private void FixedUpdate()
    {
        if (!inLiftProgress)
            return;
        Transform playerTrans = playerGameObject.transform;
        playerTrans.position = Vector3.Lerp(playerTrans.position, targetPos, 0.2f);
    }

    private void Exit()
    {
        playerOnSeat = false;
        player.movementEnable = true;
        playerGameObject.GetComponent<Rigidbody2D>().gravityScale = initGravityScale;
        //player.SeatedOnGear = false;
    }

    private IEnumerator LiftUp()
    {
        inLiftProgress = true;
        playerGameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

        Transform playerTrans = playerGameObject.transform;
        while (playerOnSeat && Mathf.Abs(transform.position.x - playerTrans.position.x) > 0.01)
        {
            targetPos = new Vector3(transform.position.x, playerTrans.position.y, playerTrans.position.z);
            yield return null;
        }
        while (playerOnSeat && Mathf.Abs(transform.position.y - playerTrans.position.y) > 0.01)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, playerTrans.position.z);
            yield return null;
        }
        inLiftProgress = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerStay)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        playerGameObject = collision.gameObject;
        initGravityScale = playerGameObject.GetComponent<Rigidbody2D>().gravityScale;

        player = playerGameObject.GetComponent<PlayerMovementController>();
        triggerStay = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != playerGameObject)
            return;

        triggerStay = false;
        if (playerOnSeat)
            Exit();
    }

    public bool isPlayerOnSeat()
    {
        return playerOnSeat;
    }

    public int playerID()
    {
        return player.playerID;
    }
}
