using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGearController : MonoBehaviour
{
    [Range(0f, 2f)]
    public float speed;

    GameObject _currPlayer;
    Transform submarine;
    float _initGravityScale;

    void Start()
    {
        submarine = transform.parent.parent;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_currPlayer == null && collision.gameObject.tag.Contains("Player"))
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
            PlayerInputController inputController = _currPlayer.GetComponent<PlayerInputController>();
            Vector3 temp = new Vector3(inputController.inputDevice.RightStickX, inputController.inputDevice.RightStickY);
            temp = temp.normalized;
            submarine.position += speed * temp * Time.deltaTime;
        }
    }
}
