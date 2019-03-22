using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeatOnGear))]
public class ShieldGearController : MonoBehaviour
{
    public float ShieldTime = 6f;
    public float PlayerCD = 2f;
    public float rotationSpeed;

    public GameObject shield;
    public GameObject submarine;
    public GameObject shieldWarning;
    public GameObject shieldGear;
    public HealthBar healthBar;

    float initGravityScale;
    float lastFireDelta;
    SeatOnGear status;
    SpriteRenderer gearRend;


    // Start is called before the first frame update
    void Start()
    {
        gearRend = shieldGear.GetComponent<SpriteRenderer>();
        status = GetComponent<SeatOnGear>();
    }

    void Update()
    {
        // update cd bar and warning
        float health = shield.GetComponent<BubbleShieldController>().Health();
        healthBar.SetSize(health);
        if (health <= 0)
        {
            shieldWarning.SetActive(true);
        }
        else
        {
            shieldWarning.SetActive(false);
        }

        if (!status.isPlayerOnSeat())
        {
            gearRend.color = new Color(gearRend.color.r, gearRend.color.g, gearRend.color.b, 0.3f);
            return;
        }
        gearRend.color = new Color(gearRend.color.r, gearRend.color.g, gearRend.color.b, 1.0f);
        int playerID = status.playerID();
        //if (InputSystemManager.GetAction2(playerID))
        //{
        //    bool success = _shield.GetComponent<BubbleShieldController>().Defense();
        //    if (success)
        //    {
        //        StartCoroutine(WaitTillBreak());
        //    }
        //}
        GenerateShield();
        RotateShield();
    }

    IEnumerator WaitTillBreak()
    {
        yield return new WaitForSeconds(ShieldTime);
        shield.GetComponent<BubbleShieldController>().BreakShield();
    }

    void GenerateShield()
    {
        if (TutorialManager.instance != null && TutorialManager.instance.tutorialMode)
        {
            if (!TutorialManager.TaskComplete(6, transform.position.x > 0f))
                return;
        }
        bool success = shield.GetComponent<BubbleShieldController>().Defense();
        if (success)
        {

            StartCoroutine(WaitTillBreak());
        }
    }

    void RotateShield()
    {
        float inputX = InputSystemManager.GetLeftSHorizontal(status.playerID());
        float inputY = InputSystemManager.GetLeftSVertical(status.playerID());

        if (inputX != 0f || inputY != 0f)
        {
            float angle = Vector2.SignedAngle(Vector2.left, new Vector2(inputX, inputY));
            float curr_angle = shield.transform.eulerAngles.z - 180f;
            angle = angle - curr_angle;

            if (angle < -180f)
                angle += 360f;
            if (angle > 180f)
                angle -= 360f;
            angle = angle * Mathf.Deg2Rad;
            shield.transform.RotateAround(submarine.transform.position, Vector3.forward, angle * rotationSpeed);
        }
    }
}
