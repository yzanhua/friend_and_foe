using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShieldController : MonoBehaviour
{
    public float ShieldCD = 5f;
    public float MAX_HEALTH = 5f;
    public Sprite full_shield_sprite;
    public float _current_health;

    private SpriteRenderer _sr;
    private Animator _an;
    private Time startTime;
    private bool _shield_ready = true;


    public bool GenerateShield()
    {
        if (!_shield_ready)
            return false;
        _shield_ready = false;

        if (_an.GetCurrentAnimatorStateInfo(0).IsName("NoBubbleShield"))
        {
            _an.SetBool("GenerateShield", true);
            SoundManager.instance.PlaySound("bubble_generate");
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
            SoundManager.instance.PlaySound("bubble_break");
            // print("Collider.enabled = " + GetComponent<CircleCollider2D>().enabled);
            StartCoroutine(WaitShieldCD());
            return true;
        }
        else return false;
    }

    IEnumerator WaitShieldCD()
    {
        yield return new WaitForSeconds(ShieldCD);
        _shield_ready = true;
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
