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

    public bool initialTowardRight = false;

    void Start()
    {
        _weapon = transform.parent.Find("Weapon").gameObject;
        _submarine = transform.parent.gameObject;
        if (initialTowardRight)
            _weapon.transform.RotateAround(_submarine.transform.position, Vector3.forward, 180f);


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

            float inputX = inputController.inputDevice.RightStickX;
            float inputY = inputController.inputDevice.RightStickY;

            if (inputX != 0f || inputY != 0f)
            {
                float angle = Vector2.SignedAngle(Vector2.right, new Vector2(inputX, inputY));
                float curr_angle = _weapon.transform.eulerAngles.z - 180f;
                angle = angle - curr_angle;

                if (angle < -180f)
                    angle += 360f;
                if (angle > 180f)
                    angle -= 360f;
                angle = angle * Mathf.Deg2Rad;
                _weapon.transform.RotateAround(_submarine.transform.position, Vector3.forward, angle * rotationSpeed);
            }
  
            //fire bullet
            if (inputController.inputDevice.Action2 && _lastFireDelta > fireTimeDiff)
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
