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

    private float initGravityScale;
    private float lastFireDelta;
    private SeatOnGear status;
    private SpriteRenderer gearRend;
    private BubbleShieldController bubbleShieldController;


    // Start is called before the first frame update
    void Start()
    {
        gearRend = shieldGear.GetComponent<SpriteRenderer>();
        status = GetComponent<SeatOnGear>();
        bubbleShieldController = shield.GetComponent<BubbleShieldController>();
    }

    void Update()
    {
        // update cd bar and warning
        float health = bubbleShieldController.Health();
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
            return;
        }
        int playerID = status.playerID();

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
        if (TutorialManager.instance != null)
        {
            TutorialManager.CompleteTask(TutorialManager.TaskType.SHIELD, transform.position.x > 0f);
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
