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
    private bool inLiftProgress = false;
    private Vector3 targetPos;
    private Sprite inactiveSprite;
    private string spriteNum;
    private Vector3 prevPos;

    private void Start()
    {
        if (station.CompareTag("MovementStation"))
        {
            inactiveSprite = station.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            inactiveSprite = station.GetComponent<SpriteRenderer>().sprite;
        }
        prevPos = transform.position;
    }

    private void Update()
    {
        if (!Global.instance.AllPlayersMovementEnable)
            return;

        //clear linked player components when gear moves
        if (prevPos != transform.position)
        {
            if (playerOnSeat)
            {
                Exit(playerGameObject);
            }
            triggerStay = false;
            player = null;
            playerGameObject = null;
            spriteNum = "-1";
        }
        prevPos = transform.position;

        if (!triggerStay)
            return;
        if (InputSystemManager.GetAction2(player.playerID))
        {
            if (!playerOnSeat)
            {
                player.movementEnable = false;
                StartCoroutine(LiftUp());
            }
            else
                Exit(playerGameObject);
        }
        if (playerOnSeat && CompareTag("MovementStation"))
        {
            Transform wheel = station.transform.GetChild(0);
            Quaternion targetRot = wheel.rotation * Quaternion.Euler(0, 0, 10);
            wheel.rotation = Quaternion.RotateTowards(wheel.rotation, targetRot, 3);
        }
    }

    private void FixedUpdate()
    {
        if (!inLiftProgress || !triggerStay)
            return;
        Transform playerTrans = playerGameObject.transform;
        playerTrans.position = Vector3.Lerp(playerTrans.position, targetPos, 0.2f);
    }

    private void LateUpdate()
    {
        // change sprite of character to back when operating the station
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

    private void Exit(GameObject exitPlayer)
    {
        playerOnSeat = false;
        // player.movementEnable = true;
        // player.gameObject.layer = 15; // 15 == Jump, PlayerMovementController.cs Update();
        //player.__seat_on_gear_exit = true
        exitPlayer.GetComponent<PlayerMovementController>().__seat_on_gear_exit = true;
        exitPlayer.GetComponent<Rigidbody2D>().gravityScale = exitPlayer.GetComponent<PlayerMovementController>().__init_gravity_scale;
        if (!station.CompareTag("MovementStation"))
        {
            station.GetComponent<SpriteRenderer>().sprite = inactiveSprite;
        }
        GameObject proxyPlayer = exitPlayer.GetComponent<PlayerMovementController>().playerProxy;
        proxyPlayer.GetComponent<Animator>().enabled = true;
    }

    private IEnumerator LiftUp()
    {
        inLiftProgress = true;
        playerGameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

        Transform playerTrans = playerGameObject.transform;
        GameObject currPlayer = playerGameObject;
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
        if (currPlayer != playerGameObject)
        {
            inLiftProgress = false;
            yield break;
        }
        playerOnSeat = true;
        if (!CompareTag("MovementStation"))
        {
            station.GetComponent<SpriteRenderer>().sprite = activeSprite;
        }
        inLiftProgress = false;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggerStay)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        playerGameObject = collision.gameObject;
        //initGravityScale = playerGameObject.GetComponent<Rigidbody2D>().gravityScale;
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
            Exit(playerGameObject);
    }

    public bool IsPlayerOnSeat()
    {
        return playerOnSeat;
    }

    public int PlayerID()
    {
        return player.playerID;
    }
}
