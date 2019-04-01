using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatOnGear : MonoBehaviour
{
    public Sprite activeSprite;
    public GameObject station;

    private bool playerOnSeat = false;
    private PlayerMovementController player;
    private GameObject playerGameObject;
    private bool triggerStay = false;
    private float initGravityScale;
    private Vector3 offset;
    private bool inLiftProgress = false;
    private Vector3 targetPos;
    private Sprite inactiveSprite;
    private string spriteNum;

    private void Start()
    {
        inactiveSprite = station.GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        if (!triggerStay)
            return;
        if (InputSystemManager.GetAction2(player.playerID))
        {
            if (!playerOnSeat)
            {
                player.movementEnable = false;
                if (!CompareTag("MovementStation"))
                {
                    station.GetComponent<SpriteRenderer>().sprite = activeSprite;
                }
                StartCoroutine(LiftUp());
            }
            else
                Exit();
        }
        if (playerOnSeat && CompareTag("MovementStation"))
        {
            Quaternion targetRot = station.transform.rotation * Quaternion.Euler(0, 0, 10);
            station.transform.rotation = Quaternion.RotateTowards(station.transform.rotation, targetRot, 3);
        }
    }

    private void FixedUpdate()
    {
        if (!inLiftProgress)
            return;
        Transform playerTrans = playerGameObject.transform;
        playerTrans.position = Vector3.Lerp(playerTrans.position, targetPos, 0.2f);
    }

    private void LateUpdate()
    {
        if (playerOnSeat)
        {
            GameObject proxyPlayer = playerGameObject.GetComponent<PlayerMovementController>().playerProxy;
            string backSpriteName = "char" + spriteNum + "_back";
            int spriteNumFolder = int.Parse(spriteNum) - 1;
            string folderPath = "Player/Player" + spriteNumFolder + "/";
            proxyPlayer.GetComponent<Animator>().enabled = false;
            Sprite backSprite = Resources.LoadAll<Sprite>(folderPath + backSpriteName)[0];
            proxyPlayer.GetComponent<SpriteRenderer>().sprite = backSprite;
        }
    }

    private void Exit()
    {
        playerOnSeat = false;
        player.movementEnable = true;
        playerGameObject.GetComponent<Rigidbody2D>().gravityScale = initGravityScale;
        station.GetComponent<SpriteRenderer>().sprite = inactiveSprite;
        GameObject proxyPlayer = playerGameObject.GetComponent<PlayerMovementController>().playerProxy;
        proxyPlayer.GetComponent<Animator>().enabled = true;
    }

    private IEnumerator LiftUp()
    {
        inLiftProgress = true;
        playerGameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

        Transform playerTrans = playerGameObject.transform;
        while (triggerStay && Mathf.Abs(transform.position.x - playerTrans.position.x) > 0.01)
        {
            targetPos = new Vector3(transform.position.x, playerTrans.position.y, playerTrans.position.z);
            yield return null;
        }
        while (triggerStay && Mathf.Abs(transform.position.y - playerTrans.position.y) > 0.01)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, playerTrans.position.z);
            yield return null;
        }
        playerOnSeat = true;
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
        spriteNum = playerGameObject.GetComponent<PlayerMovementController>().playerProxy.GetComponent<SpriteRenderer>().sprite.name.Substring(4, 1);

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
