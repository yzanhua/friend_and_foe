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

    private int _ladderLayer;

    private Rigidbody2D _rb2d;
 
    private float _initGravityScale;

    private Vector2 _target;

    private Animator _an;

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _ladderLayer = LayerMask.NameToLayer("Ladder");
        _initGravityScale = _rb2d.gravityScale;
        _an = GetComponent<Animator>();
    }

    void Update()
    {
        if ( !movementEnable || !Global.instance.AllPlayersMovementEnable)
        {
            _an.speed = 0f;
            _rb2d.velocity = Vector2.zero;
            return;
        }
        
        //movement for character and for climbing ladder
        float verticalInput = InputSystemManager.GetLeftSVertical(playerID);
        float horizontalInput = InputSystemManager.GetLeftSHorizontal(playerID);
        Debug.Log(verticalInput);
        Debug.Log(horizontalInput);
        RaycastHit2D hit_climb;
        RaycastHit2D hit_down;
        hit_climb = Physics2D.Raycast(transform.position, Vector2.up, 1.0f, 1 << _ladderLayer);
        hit_down = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, 1 << _ladderLayer);

        if (Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput))
        {
            horizontalInput = 0f;
        }
        else
        {
            verticalInput = 0f;
        }
        _an.speed = 1f;
        _an.SetFloat("vertical", verticalInput);
        _an.SetFloat("horizontal", horizontalInput);

        if (verticalInput > 0 && hit_climb)
        {
            if (hit_climb.collider.gameObject.CompareTag("Ladder") && Mathf.Abs(hit_climb.collider.transform.position.x - transform.position.x) < 0.05f)
            {
                transform.position = new Vector3(hit_climb.collider.transform.position.x, transform.position.y, 0);
            }

            _rb2d.velocity = speed * new Vector2(horizontalInput, verticalInput);
        }
        else if (verticalInput < 0 && hit_down)
        {
            if (hit_down.collider.gameObject.CompareTag("Ladder") && Mathf.Abs(hit_down.collider.transform.position.x - transform.position.x) < 0.05f)
            {
                transform.position = new Vector3(hit_down.collider.transform.position.x, transform.position.y, 0);
            }
            _rb2d.velocity = speed * new Vector2(horizontalInput, verticalInput);
        }
        else 
        {
            _rb2d.velocity = speed * new Vector2(horizontalInput, 0);
            if (_rb2d.velocity == Vector2.zero)
            {
                _an.speed = 0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rb2d.gravityScale = 0;
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rb2d.gravityScale = _initGravityScale;
        }
    }
}
