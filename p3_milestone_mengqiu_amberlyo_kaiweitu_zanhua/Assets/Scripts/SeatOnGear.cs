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
                //player.SeatedOnGear = true;
            }
        }
        else if (InputSystemManager.GetAction1(player.playerID))
            exit();

    }
    private void exit()
    {
        playerOnSeat = false;
        player.movementEnable = true;
        playerGameObject.GetComponent<Rigidbody2D>().gravityScale = initGravityScale;
        //player.SeatedOnGear = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerStay)
            return;
 
        if (!collision.gameObject.CompareTag("Player"))
            return;

        playerGameObject = collision.gameObject;
        initGravityScale = playerGameObject.GetComponent<Rigidbody2D>().gravityScale;
        playerGameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

        player = playerGameObject.GetComponent<PlayerMovementController>();
        triggerStay = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != playerGameObject)
            return;

        triggerStay = false;
        if (playerOnSeat)
            exit();
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
