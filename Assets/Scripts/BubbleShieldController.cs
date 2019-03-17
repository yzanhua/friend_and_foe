using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShieldController : MonoBehaviour
{
    public float ShieldCD = 5f;
    public float MAX_HEALTH = 5f;
    public Sprite full_shield_sprite;
    public float _current_health;

    private SpriteRenderer[] _sr;
    private Animator[] _an;
    private Time startTime;
    private bool _shield_ready = true;
    private int _childNum;

    void Start()
    {
        _current_health = MAX_HEALTH;
        _childNum = transform.childCount;
        _sr = new SpriteRenderer[_childNum];
        _an = new Animator[_childNum];
        for (int i = 0; i < _childNum; ++i)
        {
            _sr[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            _sr[i].sprite = null;
            _an[i] = transform.GetChild(i).GetComponent<Animator>();
        }
        GetComponent<PolygonCollider2D>().enabled = false;
    }

    void Update()
    {
        if (_current_health <= 0.0f)
        {
            BreakShield();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Bullet"))
        {
            _current_health -= 1;
        }
    }

    bool GenerateShield()
    {
        if (!_shield_ready)
            return false;
        _shield_ready = false;

        if (_an[0].GetCurrentAnimatorStateInfo(0).IsName("NoBubbleShield"))
        {
            for (int i = 0; i < _childNum; ++i)
            {
                _an[i].SetBool("GenerateShield", true);
            }
            SoundManager.instance.PlaySound("bubble_generate");
            _current_health = MAX_HEALTH;
            GetComponent<PolygonCollider2D>().enabled = true;
            return true;
        }
        return false;
    }

    public bool BreakShield()
    {
        if (_an[_childNum - 1].GetCurrentAnimatorStateInfo(0).IsName("BubbleShield"))
        {
            for (int i = 0; i < _childNum; ++i)
            {
                _an[i].SetBool("ShieldBreak", true);
            }
            GetComponent<PolygonCollider2D>().enabled = false;
            SoundManager.instance.PlaySound("bubble_break");
            StartCoroutine(WaitShieldCD());
            return true;
        }
        return false;
    }

    IEnumerator WaitShieldCD()
    {
        yield return new WaitForSeconds(ShieldCD);
        _shield_ready = true;
    }

    public bool Defense()
    {
        return GenerateShield();
    }
}
