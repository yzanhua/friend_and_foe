using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    public int playerID;
    [Range(0f, 10f)]
    public float speed = 2f;

    public float KnockBackTime = 0.2f;
    public float DizzyTime = 1.5f;
    
    public bool movementEnable = true;
    //public bool SeatedOnGear = false;
    

    private int ladderLayer;

    private bool onLadder = false;
    private bool climbingLadder = false;
    private float initGravityScale;
    private Rigidbody2D rb2d;
    private Vector2 target;
    private Animator an;
    private Rigidbody2D submarine_rb;

    private bool dizzy = false;
    private Vector2 self_speed;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ladderLayer = LayerMask.NameToLayer("Ladder");
        initGravityScale = rb2d.gravityScale;
        an = GetComponent<Animator>();
        submarine_rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        an.speed = 0f;
        rb2d.velocity = submarine_rb.velocity;

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

        if (Mathf.Abs(verticalInput)+ Mathf.Abs(horizontalInput) > 0f)
            an.speed = 1f;

        an.SetFloat("vertical", verticalInput);
        an.SetFloat("horizontal", horizontalInput);

        rb2d.velocity += speed * new Vector2(horizontalInput, verticalInput);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            rb2d.gravityScale = 0;
            onLadder = true;
            print("encounter ladder");
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
