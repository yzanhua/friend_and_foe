using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShieldController : MonoBehaviour
{
    public float MAX_HEALTH = 5f;
    public Sprite full_shield_sprite;


    private float _current_health;
    private SpriteRenderer _sr;
    private Animator _an;

    public bool GenerateShield()
    {
        if (_an.GetCurrentAnimatorStateInfo(0).IsName("NoBubbleShield"))
        {
            _an.SetBool("GenerateShield", true);
            return true;
        }
        else return false;
    }

    public bool BreakShield()
    {
        if (_an.GetCurrentAnimatorStateInfo(0).IsName("BubbleShield"))
        {
            _an.SetBool("ShieldBreak", true);
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GenerateShield();
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BreakShield();
        }
    }


}
