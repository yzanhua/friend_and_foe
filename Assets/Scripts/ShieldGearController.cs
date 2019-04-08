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
    public Transform submarine_proxy_transform;

    private float initGravityScale;
    private float lastFireDelta;

    private SeatOnGear status;
    private BubbleShieldController bubbleShieldController;
    
    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<SeatOnGear>();
        bubbleShieldController = shield.GetComponent<BubbleShieldController>();
        bubbleShieldController.status = status;
    }

    void Update()
    {
        if (!status.IsPlayerOnSeat())
            return;

        GenerateShield();
        RotateShield();
    }

    IEnumerator WaitTillBreak()
    {
        float temp = 0f;
        while (temp < 1f)
        {
            temp += Time.deltaTime / ShieldTime;
            yield return null;
            bubbleShieldController.ModifyHealth(-bubbleShieldController.MAX_HEALTH * Time.deltaTime / ShieldTime);
        }
    }

    void GenerateShield()
    {
        if (TutorialManager.instance != null)
        {
            TutorialManager.CompleteTask(TutorialManager.TaskType.SHIELD, transform.position.x > 0f);
        }

        bool success = shield.GetComponent<BubbleShieldController>().GenerateShield();
        if (success)
        {
            StartCoroutine(WaitTillBreak());
        }
    }

    void RotateShield()
    {
        float inputX = InputSystemManager.GetLeftSHorizontal(status.PlayerID());
        float inputY = InputSystemManager.GetLeftSVertical(status.PlayerID());

        if (inputX != 0f || inputY != 0f)
        {
            if (TutorialManager.instance != null)
            {
                TutorialManager.CompleteTask(TutorialManager.TaskType.BOUNCE, transform.position.x > 0);
            }
            float angle = Vector2.SignedAngle(Vector2.left, new Vector2(inputX, inputY));
            float curr_angle = shield.transform.eulerAngles.z - 180f;
            angle = angle - curr_angle;

            if (angle < -180f)
                angle += 360f;
            if (angle > 180f)
                angle -= 360f;
            angle = angle * Mathf.Deg2Rad;
            shield.transform.RotateAround(submarine_proxy_transform.position, Vector3.forward, angle * rotationSpeed);
        }
    }
}
