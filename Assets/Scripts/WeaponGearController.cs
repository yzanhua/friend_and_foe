using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeatOnGear))]
public class WeaponGearController : MonoBehaviour
{
    public GameObject weapon;
    public GameObject submarine;
    //public GameObject weaponWarning;
    public GameObject weaponGear;
    public HealthBar healthBar;
    public float rotationSpeed;
    public float fireTimeDiff;
    public bool initialTowardRight = false;

    private SeatOnGear status;
    private SpriteRenderer gearRend;
    private WeaponController weaponController;

    void Start()
    {
        status = GetComponent<SeatOnGear>();
        gearRend = weaponGear.GetComponent<SpriteRenderer>();
        if (initialTowardRight)
            weapon.transform.RotateAround(submarine.transform.position, Vector3.forward, 180f);
        weaponController = weapon.GetComponent<WeaponController>();
    }

    private void Update()
    {
        // update health bar and warning sign of weapon
        healthBar.SetSize(weaponController.Health());

        // update weapon under player control
        if (!status.IsPlayerOnSeat())
        {
            return;
        }
        if (InputSystemManager.GetAction1(status.PlayerID()))
        {
            if (TutorialManager.instance != null)
            {
                TutorialManager.CompleteTask(TutorialManager.TaskType.SHOOT, transform.position.x > 0f);
            }
            FireBullet(status.PlayerID());
        }
        RotateTheWeapon();
    }

    private void FireBullet(int playerID)
    {
        weaponController.Fire(playerID);
    }

    private void RotateTheWeapon()
    {
        float inputX = InputSystemManager.GetLeftSHorizontal(status.PlayerID());
        float inputY = InputSystemManager.GetLeftSVertical(status.PlayerID());

        if (inputX != 0f || inputY != 0f)
        {
            float angle = Vector2.SignedAngle(Vector2.right, new Vector2(inputX, inputY));
            float curr_angle = weapon.transform.eulerAngles.z - 180f;
            angle = angle - curr_angle;

            if (angle < -180f)
                angle += 360f;
            if (angle > 180f)
                angle -= 360f;
            angle = angle * Mathf.Deg2Rad;
            weapon.transform.RotateAround(submarine.transform.position, Vector3.forward, angle * rotationSpeed);
        }
    }
}
