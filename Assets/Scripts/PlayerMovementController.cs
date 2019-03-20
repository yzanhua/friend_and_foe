using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    public int playerID;
    //[Range(0f, 100f)]
    public float speed = 2f;

    public float KnockBackTime = 0.2f;
    public float DizzyTime = 1.5f;
    
    public bool movementEnable = true;
    //public bool SeatedOnGear = false;

    public Animator an;
    //public Rigidbody2D submarine_rb;

    private int ladderLayer;

    private bool onLadder = false;
    private bool climbingLadder = false;
    private float initGravityScale;
    private Rigidbody2D rb2d;
    private Vector2 target;
    

    private bool dizzy = false;
    private Vector2 self_speed;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ladderLayer = LayerMask.NameToLayer("Ladder");
        initGravityScale = rb2d.gravityScale;
    }

    private void Update()
    {
        an.speed = 0f;

        if (dizzy)
        {
            rb2d.velocity += self_speed;
            return;
        }

        if (!movementEnable || !Global.instance.AllPlayersMovementEnable)
            return;

        float verticalInput = InputSystemManager.GetLeftSVertical(playerID);
        float horizontalInput = InputSystemManager.GetLeftSHorizontal(playerID);

        if (!onLadder && verticalInput > 0f)
            verticalInput = 0f;
        if (climbingLadder)
            horizontalInput = 0f;

        if (Mathf.Abs(verticalInput) < Mathf.Abs(horizontalInput))
            verticalInput = 0f;
        else
            horizontalInput = 0f;

        if (Mathf.Abs(verticalInput)+ Mathf.Abs(horizontalInput) > 0f)
            an.speed = 1f;

        an.SetFloat("vertical", verticalInput);
        an.SetFloat("horizontal", horizontalInput);

        Vector2 temp = new Vector2(horizontalInput, verticalInput);
        temp = temp.normalized * Time.deltaTime * speed;
        rb2d.AddForce(speed * temp * rb2d.mass);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            rb2d.gravityScale = 0;
            onLadder = true;
        }
        if (collision.gameObject.CompareTag("LadderForbidHorizontal"))
        {
            climbingLadder = true;
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            rb2d.gravityScale = initGravityScale;
            onLadder = false;
        }
        if (collision.gameObject.CompareTag("LadderForbidHorizontal"))
        {
            climbingLadder = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (!other.CompareTag("Player"))
            return;

        Vector3 forceDirection = (transform.position - other.transform.position).normalized;
        if (climbingLadder)
            forceDirection.x = 0f;
        else
            forceDirection.y = 0f;
        dizzy = true;
        self_speed = forceDirection * 3;
        StartCoroutine(KnockBackEffect());
    }

    IEnumerator KnockBackEffect()
    {
        yield return new WaitForSeconds(KnockBackTime);
        self_speed = Vector2.zero;
        yield return new WaitForSeconds(DizzyTime);
        dizzy = false;
    }
}
