using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeatOnGear))]
public class WeaponGearController : MonoBehaviour
{
    public GameObject weapon;
    public GameObject submarine;
    public GameObject weaponWarning;
    public GameObject weaponGear;
    public HealthBar healthBar;
    public float rotationSpeed;
    public float fireTimeDiff;
    public bool initialTowardRight = false;

    private SeatOnGear status;
    private SpriteRenderer gearRend;

    void Start()
    {
        status = GetComponent<SeatOnGear>();
        gearRend = weaponGear.GetComponent<SpriteRenderer>();
        weaponWarning.SetActive(false);
        if (initialTowardRight)
            weapon.transform.RotateAround(submarine.transform.position, Vector3.forward, 180f);
    }

    private void Update()
    {
        // update health bar and warning sign of weapon
        healthBar.SetSize(weapon.GetComponent<WeaponController>().Health());

        if (weapon.GetComponent<WeaponController>().Health() <= 0)
        {
            weaponWarning.SetActive(true);
        }
        else
        {
            weaponWarning.SetActive(false);
        }

        // update weapon under player control
        if (!status.isPlayerOnSeat())
        {
            gearRend.color = new Color(gearRend.color.r, gearRend.color.g, gearRend.color.b, 0.3f);
            return;
        }
        gearRend.color = new Color(gearRend.color.r, gearRend.color.g, gearRend.color.b, 1.0f);
        if (TutorialManager.instance != null && TutorialManager.instance.tutorialMode)
            TutorialManager.TaskComplete(4, transform.position.x > 0f);
        if (InputSystemManager.GetAction1(status.playerID()))
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
