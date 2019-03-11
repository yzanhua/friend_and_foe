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
    bool shieldOn = false;
    bool hasPlayer = false;

    public float ShieldCD = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        _shield = transform.parent.Find("BubbleShield").gameObject;
        _submarine = transform.parent.parent.gameObject;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name=="Player_Shield")
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
        if (collision.gameObject.name == "Player_Shield")
        {
            _currPlayer.GetComponent<Rigidbody2D>().gravityScale = _initGravityScale;
            hasPlayer = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (shieldOn || !hasPlayer)
            return;

        PlayerInputController inputController = _currPlayer.GetComponent<PlayerInputController>();
        if (inputController.inputDevice.Action2)
        {
            _currPlayer.GetComponent<PlayerMovementController>().movementEnable = false;
            bool success = _shield.GetComponent<BubbleShieldController>().Defense();
            if (success)
            {
                StartCoroutine(WaitTillBreak());
                shieldOn = true;
            }
        }
    }

    IEnumerator WaitTillBreak()
    {
        yield return new WaitForSeconds(ShieldCD);
        _shield.GetComponent<BubbleShieldController>().BreakShield();
        yield return new WaitForSeconds(1.5f);
        _currPlayer.GetComponent<PlayerMovementController>().movementEnable = true;
        shieldOn = false;
    }
}
