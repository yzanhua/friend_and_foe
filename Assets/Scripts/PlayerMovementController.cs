using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    [Range(0f, 10f)]
    public float speed = 2f;
    public int playerID;
    public bool movementEnable = true;

    private int ladderLayer;

    private bool onLadder = false;
    private bool climbingLadder = false;
    private float initGravityScale;
    private Rigidbody2D rb2d;
    private Vector2 target;
    private Animator an;
    private Rigidbody2D submarine_rb;

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
        if (!movementEnable || !Global.instance.AllPlayersMovementEnable)
        {
            rb2d.velocity = submarine_rb.velocity;
            return;
        }

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

        rb2d.velocity = speed * new Vector2(horizontalInput, verticalInput);
        rb2d.velocity += submarine_rb.velocity;
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
        
    }
}
