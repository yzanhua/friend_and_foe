using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeatOnGear))]
public class WeaponGearController : MonoBehaviour
{
    public GameObject weapon;
    public  GameObject submarine;
    private  SeatOnGear status;
    public HealthBar healthBar;

    public float rotationSpeed;
    public float fireTimeDiff;
    public bool initialTowardRight = false;

    void Start()
    {
        status = GetComponent<SeatOnGear>();
        if (initialTowardRight)
            weapon.transform.RotateAround(submarine.transform.position, Vector3.forward, 180f);
    }

    private void Update()
    {
        // update health bar of weapon
        healthBar.SetSize(weapon.GetComponent<WeaponController>().Health());

        if (!status.isPlayerOnSeat())
            return;

        if (TutorialManager.instance != null && TutorialManager.instance.tutorialMode)
            TutorialManager.TaskComplete(4, transform.position.x > 0f);

        if (InputSystemManager.GetAction2(status.playerID()))
            FireBullet();
        RotateTheWeapon();
    }

    private void FireBullet()
    {
        weapon.GetComponent<WeaponController>().Fire();
    }

    private void RotateTheWeapon()
    {
        float inputX = InputSystemManager.GetLeftSHorizontal(status.playerID());
        float inputY = InputSystemManager.GetLeftSVertical(status.playerID());

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
