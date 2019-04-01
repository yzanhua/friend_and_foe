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

    public float knockBackDistance = 1f;

    public GameObject trailPrefab;
    public GameObject playerProxy;
    public GameObject stunnedCirclingStarsPrefab;

    private bool onLadder = false;
    private bool climbingLadder = false;
    private GameObject ladder;
    private float initGravityScale;
    private Rigidbody2D rb2d;
    private Vector2 target;

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
            if (InputSystemManager.GetAction1(playerID))
            {
                if (TutorialManager.instance != null)
                {
                    TutorialManager.CompleteTask(TutorialManager.TaskType.JUMP, transform.position.x > 0f);
                }

                Jump(horizontalInput);
                return;
            }
            horizontalInput = 0f;
        }
        if (Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput))
        {// moves vertically
            horizontalInput = 0f;
            Vector3 temp = new Vector3(horizontalInput, verticalInput);
            temp = temp.normalized * Time.deltaTime * 2.5f;
            temp += transform.position;
            transform.position = new Vector3(ladder.transform.position.x, temp.y, temp.z);
        }
        else
        {// moves horizontally
            verticalInput = 0f;
            Vector2 temp = new Vector2(horizontalInput, verticalInput);
            temp = temp.normalized * speed;
            if (InputSystemManager.GetAction1(playerID) && temp.magnitude > 0f)
            {// dash
                if (TutorialManager.instance != null)
                {
                    TutorialManager.CompleteTask(TutorialManager.TaskType.DASH, transform.position.x > 0f);
                }

                temp = temp * 15f;
                GameObject new_trail = Instantiate(trailPrefab, playerProxy.transform);
                new_trail.transform.position = playerProxy.transform.position;
            }
                
            rb2d.AddForce(speed * temp * rb2d.mass);
        }

        // set animation
        if (Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput) > 0f)
        {
            an.speed = 1f;
            an.SetFloat("vertical", verticalInput);
            an.SetFloat("horizontal", horizontalInput);
        }
        else if (!movementEnable)
        {
            an.speed = 1f;
            an.SetFloat("vertical", 0);
            an.SetFloat("horizontal", 0);
        }
    }

    void Jump(float horizontalInput)
    {
        float verticalInput = -1f;
        rb2d.gravityScale = initGravityScale * 2f;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

        Vector2 horizontalForce = (new Vector2(horizontalInput, 0f)).normalized;
        rb2d.AddForce(horizontalForce * 8f + Vector2.up * 10f, ForceMode2D.Impulse);
        gameObject.layer = 15; // 15=jump
        onLadder = false;
        movementEnable = false;

        if (Mathf.Abs(horizontalInput) > 0f)
            verticalInput = 0f;

        an.SetFloat("horizontal", horizontalInput);
        an.SetFloat("vertical", verticalInput);
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
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
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
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            Vector3 forceDirection = (other.transform.position - transform.position);
            if (climbingLadder)
                forceDirection.x = 0f;
            else
                forceDirection.y = 0f;

            forceDirection = forceDirection.normalized;
            movementEnable = false;
            rb2d.AddForce(-forceDirection * knockBackDistance * collision.relativeVelocity.magnitude);
            StartCoroutine(KnockBackEffect(collision.relativeVelocity / 6f));
        }
        else if (other.layer == 13 && gameObject.layer == 15)
        {   // 13 = floor, 15 == Jump
            gameObject.layer = 14; // 14 = Player
            movementEnable = true;
        }
    }

    IEnumerator KnockBackEffect(Vector2 relativeVelocity)
    {
        yield return new WaitForSeconds(KnockBackTime);
        rb2d.velocity = Vector2.zero;
        GameObject stunnedStar = Instantiate(stunnedCirclingStarsPrefab, playerProxy.transform.position + new Vector3(0, 0.6f)
            , Quaternion.FromToRotation(new Vector3(0f, 1f, 0f), new Vector3(0f, 0.2f, -1f)));
        stunnedStar.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        //stunnedStar.transform.parent = playerProxy.transform;

        for (int i = 0; i < DizzyTime * relativeVelocity.magnitude / Time.deltaTime; i++)
        {
            stunnedStar.transform.position = playerProxy.transform.position + new Vector3(0, 0.6f);
            yield return null;
        }
        movementEnable = true;
        Destroy(stunnedStar);
    }
}
