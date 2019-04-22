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
    private GameObject weapon_laser;
    public GameObject weapon_charge;
    int submarine_id;
    bool extra_skill_shooted = false;
    private HealthCounter healthCounter;

    void Start()
    {
        status = GetComponent<SeatOnGear>();
        gearRend = weaponGear.GetComponent<SpriteRenderer>();
        if (initialTowardRight)
            weapon.transform.RotateAround(submarine.transform.position, Vector3.forward, 180f);
        weaponController = weapon.GetComponent<WeaponController>();
        weapon_laser = weapon.transform.Find("laser").gameObject;
        submarine_id = submarine.GetComponent<SubmarineController>().ID;
        healthCounter = submarine.GetComponent<HealthCounter>();
    }

    private void Update()
    {
        // update health bar and warning sign of weapon
        healthBar.SetSize(weaponController.Health());

        // update weapon under player control
        if (!status.IsPlayerOnSeat())
            return;

        RotateTheWeapon();

        if (extra_skill_shooted)
            return;

        if (TutorialManager.instance != null)
            TutorialManager.CompleteTask(TutorialManager.TaskType.SEAT_WEAPON, transform.position.x > 0);

        bool rightshoulderTriggered = InputSystemManager.GetRightShoulder1(status.PlayerID()) || InputSystemManager.GetRightShoulder2(status.PlayerID());
        //if (healthCounter.readyToShootLaser && rightshoulderTriggered)
        if (rightshoulderTriggered)
        {
            extra_skill_shooted = true;
            if(TutorialManager.instance != null)
            {
                TutorialManager.CompleteTask(TutorialManager.TaskType.HUGE_CANON, transform.position.x > 0f);
            }
            GameObject temp = Instantiate(weapon_charge, weapon_laser.transform.position, Quaternion.identity);
            temp.transform.parent = weapon.transform;
            StartCoroutine(ExtraSkill());
            return;
        }

        if (InputSystemManager.GetAction1(status.PlayerID()))
        {
            FireBullet(status.PlayerID());
        }
        
    }

    IEnumerator ExtraSkill()
    {
        yield return new WaitForSeconds(3.5f);
        weapon_laser.SetActive(true);
        Global.instance.ExtraSkillEnable[submarine_id] = true;
        Global.instance.ExtraSkillEnableDown[submarine_id] = true;
        InputSystemManager.SetVibrationBySubmarine(submarine_id, 0.8f, 4f);

        yield return new WaitForEndOfFrame();
        Global.instance.ExtraSkillEnableDown[submarine_id] = false;

        healthCounter.ResetChargeBar();
        yield return new WaitForSeconds(4f);
        Global.instance.ExtraSkillEnable[submarine_id] = false;
        weapon_laser.SetActive(false);
        extra_skill_shooted = false;

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

            if (TutorialManager.instance != null)
            {
                TutorialManager.CompleteTask(TutorialManager.TaskType.RO_WEAPON, transform.position.x > 0);
            }
            if (angle < -180f)
                angle += 360f;
            if (angle > 180f)
                angle -= 360f;
            angle = angle * Mathf.Deg2Rad;
            if (extra_skill_shooted)
                angle *= 0.025f;

            weapon.transform.RotateAround(submarine.transform.position, Vector3.forward, angle * rotationSpeed);
        }
    }
}
