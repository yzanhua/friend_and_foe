using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeatOnGear))]
public class ShieldGearController : MonoBehaviour
{
    public float ShieldTime = 6f;
    public float PlayerCD = 2f;
    public float rotationSpeed;

    GameObject _shield;
    GameObject _submarine;
    float _initGravityScale;
    float _lastFireDelta;
    SeatOnGear _status;
    HealthBar _healthBar;

    // Start is called before the first frame update
    void Start()
    {
        _shield = transform.parent.Find("BubbleShield").gameObject;
        _submarine = transform.parent.parent.gameObject;
        _status = GetComponent<SeatOnGear>();
        _healthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
    }

    void Update()
    {
        // update cd bar
        _healthBar.SetSize(_shield.GetComponent<BubbleShieldController>().health());

        if (!_status.isPlayerOnSeat())
            return;
        int playerID = _status.playerID();
        if (InputSystemManager.GetAction2(playerID))
        {
            bool success = _shield.GetComponent<BubbleShieldController>().Defense();
            if (success)
            {
                StartCoroutine(WaitTillBreak());
            }
        }
        RotateShield();
    }

    IEnumerator WaitTillBreak()
    {
        yield return new WaitForSeconds(ShieldTime);
        _shield.GetComponent<BubbleShieldController>().BreakShield();
    }

    void RotateShield()
    {
        float inputX = InputSystemManager.GetLeftSHorizontal(_status.playerID());
        float inputY = InputSystemManager.GetLeftSVertical(_status.playerID());

        if (inputX != 0f || inputY != 0f)
        {
            float angle = Vector2.SignedAngle(Vector2.left, new Vector2(inputX, inputY));
            float curr_angle = _shield.transform.eulerAngles.z - 180f;
            angle = angle - curr_angle;

            if (angle < -180f)
                angle += 360f;
            if (angle > 180f)
                angle -= 360f;
            angle = angle * Mathf.Deg2Rad;
            _shield.transform.RotateAround(_submarine.transform.position, Vector3.forward, angle * rotationSpeed);
        }
    }
}
