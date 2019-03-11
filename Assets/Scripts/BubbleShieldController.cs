using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShieldController : MonoBehaviour
{
    public float MAX_HEALTH = 5f;
    public Sprite full_shield_sprite;

    public float _current_health;
    private SpriteRenderer _sr;
    private Animator _an;
    private Time startTime;

    public bool GenerateShield()
    {
        if (_an.GetCurrentAnimatorStateInfo(0).IsName("NoBubbleShield"))
        {
            _an.SetBool("GenerateShield", true);
            _current_health = MAX_HEALTH;
            GetComponent<CircleCollider2D>().enabled = true;
            //StartCoroutine(WaitTillBreak());
            return true;
        }
        else return false;
    }

    public bool BreakShield()
    {
        if (_an.GetCurrentAnimatorStateInfo(0).IsName("BubbleShield"))
        {
            _an.SetBool("ShieldBreak", true);
            GetComponent<CircleCollider2D>().enabled = false;
            // print("Collider.enabled = " + GetComponent<CircleCollider2D>().enabled);
            return true;
        }
        else return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _current_health = MAX_HEALTH;
        _sr = GetComponent<SpriteRenderer>();
        _an = GetComponent<Animator>();
        _sr.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_current_health <= 0.0f)
        {
            BreakShield();
        }
    }

    public bool Defense()
    {
        return GenerateShield();
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Bullet"))
        {
            _current_health -= 1;
        }
    }
}
