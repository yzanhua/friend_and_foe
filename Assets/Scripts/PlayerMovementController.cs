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

    public Animator an;

    public float knockBackDistance = 300f;

    private bool onLadder = false;
    private bool climbingLadder = false;
    private GameObject ladder;
    private float initGravityScale;
    private Rigidbody2D rb2d;
    private Vector2 target;
    private bool dizzy = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        initGravityScale = rb2d.gravityScale;
    }

    private void Update()
    {
        an.speed = 0f;
        if (!movementEnable || !Global.instance.AllPlayersMovementEnable)
            return;

        float verticalInput = InputSystemManager.GetLeftSVertical(playerID);
        float horizontalInput = InputSystemManager.GetLeftSHorizontal(playerID);

        if (!onLadder)
        {
            verticalInput = 0f;
            
        }
        else if (climbingLadder)
        {
            horizontalInput = 0f;
        }
        if (Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput)) // horizontalInput = 0f;
        {
            Vector3 temp = new Vector3(0f, verticalInput, 0f);
            temp = temp.normalized * Time.deltaTime * 2.5f;
            temp += transform.position;
            transform.position = new Vector3(ladder.transform.position.x, temp.y, temp.z);
        }
        else // verticalInput = 0f
        {
            Vector2 temp = new Vector2(horizontalInput, 0f);
            temp = temp.normalized * speed;
            rb2d.AddForce(speed * temp * rb2d.mass);
        }

        

        



        // set animation
        if (Mathf.Abs(verticalInput)+ Mathf.Abs(horizontalInput) > 0f)
            an.speed = 1f;
        an.SetFloat("vertical", verticalInput);
        an.SetFloat("horizontal", horizontalInput);

       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            rb2d.gravityScale = 0f;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            onLadder = true;
            ladder = collision.gameObject;
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

        Vector3 forceDirection = (other.transform.position - transform.position);
        if (climbingLadder)
            forceDirection.x = 0f;
        else
            forceDirection.y = 0f;

        forceDirection = forceDirection.normalized;
        movementEnable = false;
        rb2d.AddForce(-forceDirection* knockBackDistance);
        StartCoroutine(KnockBackEffect());
    }

    IEnumerator KnockBackEffect()
    {
        yield return new WaitForSeconds(KnockBackTime);
        rb2d.velocity = Vector2.zero;
        yield return new WaitForSeconds(DizzyTime);
        movementEnable = true;
    }
}
