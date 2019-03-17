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
                //offset = playerGameObject.transform.position - transform.position;
            }
        }
        else // playerOnSeat = true
        {
            if (InputSystemManager.GetAction1(player.playerID))
            {
                playerOnSeat = false;
                player.movementEnable = true;
                playerGameObject.GetComponent<Rigidbody2D>().gravityScale = initGravityScale;
            }
            //playerGameObject.transform.position = transform.position + offset;
        }
        
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
        {
            playerOnSeat = false;
            player.movementEnable = true;
            playerGameObject.GetComponent<Rigidbody2D>().gravityScale = initGravityScale;
        }
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
