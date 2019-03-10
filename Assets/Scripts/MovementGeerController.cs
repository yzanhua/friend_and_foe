using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGeerController : MonoBehaviour
{
    GameObject _currPlayer;
    float _initGravityScale;

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
}
