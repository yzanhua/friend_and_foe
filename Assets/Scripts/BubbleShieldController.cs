using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShieldController : MonoBehaviour
{
    public float ShieldCD = 3f;
    public float MAX_HEALTH = 7f;

    public float _current_health;
    public int particleLayer;

    public GameObject bubbleShieldParticlePrefab;
    public GameObject shieldWarning;
    public HealthBar healthBar;

    public SeatOnGear status = null;

    public bool inUse = false;

    private SpriteRenderer[] _sr;
    private GameObject[] _bubbleParticles;
    private Animator[] _an;
    private Time _startTime;
    private bool _shield_ready = true;
    private int _childNum;

    private void Start()
    {
        _current_health = MAX_HEALTH;
        _childNum = transform.childCount;
        _sr = new SpriteRenderer[_childNum];
        _an = new Animator[_childNum];
        _bubbleParticles = new GameObject[_childNum];
        for (int i = 0; i < _childNum; ++i)
        {
            _sr[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            _sr[i].sprite = null;
            _an[i] = transform.GetChild(i).GetComponent<Animator>();
        }
        GetComponent<PolygonCollider2D>().enabled = false;
        healthBar.SetSize(_current_health / MAX_HEALTH);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Bullet"))
        {
            ModifyHealth(-1f);
        }
        if (other.CompareTag("Weapon") || other.CompareTag("Submarine"))
        {
            ModifyHealth(-MAX_HEALTH / 3f);
        }
    }

    public IEnumerator WaitTillBreak(float time)
    {
        float temp = 0f;
        while (temp < 1f && inUse)
        {
            temp += Time.deltaTime / time;
            yield return null;
            ModifyHealth(-MAX_HEALTH * Time.deltaTime / time);
        }
    }
    public void ModifyHealth(float off)
    {
        if (_current_health <= 0f && off <= 0f)
            return;

        _current_health += off;

        if (_current_health <= 0.0f)
        {
            _current_health = 0f;
            BreakShield();
            shieldWarning.SetActive(true);
            status.Exit();
            StartCoroutine(WaitShieldCD());
        }

        healthBar.SetSize(_current_health / MAX_HEALTH);
    }

    public bool GenerateShield()
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
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("bubble_generate");
            GetComponent<PolygonCollider2D>().enabled = true;
            StartCoroutine(StartParticle());
            inUse = true;
            return true;
        }
        return false;
    }

    private IEnumerator StartParticle()
    {
        while (true)
        {
            if (_an[_childNum - 1].GetCurrentAnimatorStateInfo(0).IsName("BubbleShield"))
            {
                for (int i = 0; i < _childNum; ++i)
                {
                    GameObject bubble = Instantiate(bubbleShieldParticlePrefab, transform.GetChild(i).transform);
                    bubble.transform.localScale = new Vector3(26f, 1f, 26f);
                    bubble.transform.localPosition = new Vector3(0.2f, 2.8f, bubble.transform.position.z);
                    ChangeSortingLayer(bubble);
                    _bubbleParticles[i] = bubble;
                }
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator WaitShieldCD()
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime / ShieldCD;
            healthBar.SetSize(time);
            yield return null;
        }
        _shield_ready = true;
        _current_health = MAX_HEALTH;
        shieldWarning.SetActive(false);
    }

    private void ChangeSortingLayer(GameObject effectobject)
    {
        foreach (Transform child in effectobject.transform)
        {
            ParticleSystemRenderer psr = child.gameObject.GetComponent<ParticleSystemRenderer>();
            if (psr != null)
            {
                psr.sortingOrder = particleLayer;
            }
        }

        ParticleSystemRenderer parent_psr = effectobject.GetComponent<ParticleSystemRenderer>();
        if (parent_psr != null)
        {
            parent_psr.sortingOrder = particleLayer;
        }
    }

    public bool BreakShield()
    {
        if (_an[_childNum - 1].GetCurrentAnimatorStateInfo(0).IsName("BubbleShield"))
        {
            for (int i = 0; i < _childNum; ++i)
            {
                _an[i].SetBool("ShieldBreak", true);
                Destroy(_bubbleParticles[i]);
            }
            GetComponent<PolygonCollider2D>().enabled = false;
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("bubble_break");
            inUse = false;
            return true;
        }
        return false;
    }

}
