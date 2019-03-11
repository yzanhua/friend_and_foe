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

    // Start is called before the first frame update
    void Start()
    {
        _shield = transform.parent.Find("BubbleShield").gameObject;
        _submarine = transform.parent.gameObject;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_currPlayer == null && collision.gameObject.CompareTag("Player"))
        {
            _currPlayer = collision.gameObject;
            _currPlayer.transform.position = transform.position;
            _initGravityScale = _currPlayer.GetComponent<Rigidbody2D>().gravityScale;
            _currPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _currPlayer)
        {
            _currPlayer.GetComponent<Rigidbody2D>().gravityScale = _initGravityScale;
            _currPlayer = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_currPlayer != null)
        {
            //adjust wapon angle
            PlayerInputController inputController = _currPlayer.GetComponent<PlayerInputController>();
            if (inputController.inputDevice.Action1)
                _shield.GetComponent<BubbleShieldController>().Defense();
        }
    }
}
