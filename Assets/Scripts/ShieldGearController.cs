using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGearController : MonoBehaviour
{

    GameObject _currPlayer;
    GameObject _shield;
    GameObject _submarine;
    float _initGravityScale;
    float _lastFireDelta;
    bool hasPlayer = false;

    public float ShieldTime = 6f;
    public float PlayerCD = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _shield = transform.parent.Find("BubbleShield").gameObject;
        _submarine = transform.parent.parent.gameObject;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _currPlayer = collision.gameObject;
            _currPlayer.transform.position = transform.position;
            _initGravityScale = _currPlayer.GetComponent<Rigidbody2D>().gravityScale;
            _currPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
            hasPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _currPlayer.GetComponent<Rigidbody2D>().gravityScale = _initGravityScale;
            hasPlayer = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasPlayer)
            return;

        int playerID = _currPlayer.GetComponent<PlayerMovementController>().playerID;
        if (InputSystemManager.GetAction2(playerID))
        {
            bool success = _shield.GetComponent<BubbleShieldController>().Defense();
            if (success)
            {
                _currPlayer.GetComponent<PlayerMovementController>().movementEnable = false;
                StartCoroutine(WaitTillBreak());
                StartCoroutine(WaitPlayerCD());
            }
        }
    }

    IEnumerator WaitTillBreak()
    {
        yield return new WaitForSeconds(ShieldTime);
        _shield.GetComponent<BubbleShieldController>().BreakShield();
    }

    IEnumerator WaitPlayerCD()
    {
        yield return new WaitForSeconds(PlayerCD);
        _currPlayer.GetComponent<PlayerMovementController>().movementEnable = true;
    }
}
