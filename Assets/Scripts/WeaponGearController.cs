using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGearController : MonoBehaviour
{
    public float rotationSpeed;
    public float fireTimeDiff;

    GameObject _currPlayer;
    GameObject _weapon;
    GameObject _submarine;
    float _initGravityScale;
    float _lastFireDelta;

    void Start()
    {
        _weapon = transform.parent.Find("Weapon").gameObject;
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

    void Update()
    {
        if (_currPlayer != null)
        {
            //adjust wapon angle
            PlayerInputController inputController = _currPlayer.GetComponent<PlayerInputController>();
            float direction = 0;
            if (inputController.inputDevice.RightStickY > 0.2 || inputController.inputDevice.RightStickY < -0.2)
            {
                direction = Mathf.Sign(inputController.inputDevice.RightStickY);
            }
            _weapon.transform.RotateAround(_submarine.transform.position, Vector3.back, direction * rotationSpeed);

            //fire bullet
            if (inputController.inputDevice.Action1 && _lastFireDelta > fireTimeDiff)
            {
                _weapon.GetComponent<WeaponController>().Fire();
                _lastFireDelta = 0;
            }
            else
            {
                _lastFireDelta += Time.deltaTime;
            }
        }
    }
}
