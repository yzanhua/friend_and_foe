using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGearController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float speed;

    GameObject _currPlayer;
    SubmarineController _submarineController;
    float _initGravityScale;

    void Start()
    {
        _submarineController = transform.parent.GetComponent<SubmarineController>();
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

    void Update()
    {
        if (_currPlayer != null)
        {
            Transform submarine = transform.parent;
            PlayerInputController inputController = _currPlayer.GetComponent<PlayerInputController>();
            submarine.position += speed * new Vector3(inputController.inputDevice.RightStickX, inputController.inputDevice.RightStickY);
        }
    }
}
