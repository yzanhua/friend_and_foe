using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeatOnGear))]
public class WeaponGearController : MonoBehaviour
{
    private GameObject _weapon;
    private GameObject _submarine;
    private SeatOnGear status;

    public float rotationSpeed;
    public float fireTimeDiff;
    public bool initialTowardRight = false;

    void Start()
    {
        status = GetComponent<SeatOnGear>();
        _weapon = transform.parent.Find("Weapon").gameObject;
        _submarine = transform.parent.gameObject;
        if (initialTowardRight)
            _weapon.transform.RotateAround(_submarine.transform.position, Vector3.forward, 180f);
    }

    private void Update()
    {
        if (!status.isPlayerOnSeat())
            return;

        if (InputSystemManager.GetAction2(status.playerID()))
            fireBullet();
        RotateTheWeapon();
    }

    private void fireBullet()
    {
        _weapon.GetComponent<WeaponController>().Fire();
    }

    private void RotateTheWeapon()
    {
        float inputX = InputSystemManager.GetLeftSHorizontal(status.playerID());
        float inputY = InputSystemManager.GetLeftSVertical(status.playerID());

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
    }
}
