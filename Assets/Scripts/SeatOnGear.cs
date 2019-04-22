using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatOnGear : MonoBehaviour
{
    public Sprite activeSprite;
    public GameObject proxy_gear;
    public GameObject buttonHint;
    public GameObject buttonHintOnStation;

    private bool playerOnSeat = false;
    private PlayerMovementController playerController;
    //private GameObject playerGameObject;

    private bool triggerStay = false;
    private bool inLiftProgress = false;

    private Vector3 player_targetPos;
    private Sprite inactiveSprite;
    private string spriteNum;
    private Vector3 prevPos;
    private bool LiftUpbreak = false;

    private void Start()
    {
        if (proxy_gear.CompareTag("MovementStation"))
            inactiveSprite = proxy_gear.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        else
            inactiveSprite = proxy_gear.GetComponent<SpriteRenderer>().sprite;

        prevPos = transform.position;
    }

    private void Update()
    {
        if (!Global.instance.AllPlayersMovementEnable)
            return;

        // button hint
        if (triggerStay && !playerOnSeat)
            buttonHint.SetActive(true);
        else
            buttonHint.SetActive(false);
        if (playerOnSeat)
            buttonHintOnStation.SetActive(true);
        else
            buttonHintOnStation.SetActive(false);

        if (!triggerStay)
            return;
        if (InputSystemManager.GetAction2(playerController.playerID))
        {
            if (!playerOnSeat)
            {
                playerController.movementEnable = false;
                StartCoroutine(LiftUp());
            }
            else
                Exit();
        }
        if (playerOnSeat && CompareTag("MovementStation"))
        {
            Transform wheel = proxy_gear.transform.GetChild(0);
            Quaternion targetRot = wheel.rotation * Quaternion.Euler(0, 0, 10);
            wheel.rotation = Quaternion.RotateTowards(wheel.rotation, targetRot, 3);
        }
    }

    private void FixedUpdate()
    {
        if (!inLiftProgress || !triggerStay)
            return;
        Transform playerTrans = playerController.transform;
        playerTrans.position = Vector3.Lerp(playerTrans.position, player_targetPos, 0.2f);
    }

    private void LateUpdate()
    {
        if (playerOnSeat)
        {
            GameObject proxyPlayer = playerController.playerProxy;
            string backSpriteName = "char" + spriteNum + "_back";
            int spriteNumFolder = int.Parse(spriteNum) - 1;
            string folderPath = "Player/Player" + spriteNumFolder + "/";
            proxyPlayer.GetComponent<Animator>().enabled = false;
            Sprite backSprite = Resources.LoadAll<Sprite>(folderPath + backSpriteName)[0];
            proxyPlayer.GetComponent<SpriteRenderer>().sprite = backSprite;
        }
    }

    public void Exit()
    {
        if (playerOnSeat || inLiftProgress || LiftUpbreak)
        {
            playerOnSeat = false;
            playerController.__seat_on_gear_exit = true;
            playerController.gameObject.GetComponent<Rigidbody2D>().gravityScale = playerController.__init_gravity_scale;
            if (!proxy_gear.CompareTag("MovementStation"))
            {
                proxy_gear.GetComponent<SpriteRenderer>().sprite = inactiveSprite;
            }
            playerController.playerProxy.GetComponent<Animator>().enabled = true;
            LiftUpbreak = false;
            inLiftProgress = false;
        }

    }

    private IEnumerator LiftUp()
    {
        inLiftProgress = true;

        GameObject currPlayer = playerController.gameObject;
        currPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
        Transform playerTrans = currPlayer.transform;

        player_targetPos = new Vector3(transform.position.x, playerTrans.position.y, playerTrans.position.z);
        while (triggerStay && inLiftProgress && Mathf.Abs(transform.position.x - playerTrans.position.x) > 0.01)
            yield return null;

        player_targetPos = new Vector3(transform.position.x, transform.position.y, playerTrans.position.z);
        while (triggerStay && inLiftProgress && Mathf.Abs(transform.position.y - playerTrans.position.y) > 0.01)
            yield return null;

        if (triggerStay && inLiftProgress)
            playerOnSeat = true;

        if (!CompareTag("MovementStation"))
            proxy_gear.GetComponent<SpriteRenderer>().sprite = activeSprite;
        inLiftProgress = false;
    }

    public void GearPostionChanged()
    {
        LiftUpbreak = true;
        inLiftProgress = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerStay || !collision.gameObject.CompareTag("Player"))
            return;

        playerController = collision.gameObject.GetComponent<PlayerMovementController>();
        spriteNum = playerController.playerProxy.GetComponent<SpriteRenderer>().sprite.name.Substring(4, 1);
        triggerStay = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (collision.gameObject.GetComponent<PlayerMovementController>() != playerController)
            return;

        Exit();

        playerController = null;
        triggerStay = false;
    }

    public bool IsPlayerOnSeat()
    {
        return playerOnSeat;
    }

    public int PlayerID()
    {
        return playerController.playerID;
    }
}
