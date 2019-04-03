using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : MonoBehaviour
{
    public GameObject impactParticlePrefab;

    public GameObject projectileParticlePrefab;

    public GameObject muzzleParticlePrefab;

    public GameObject hitParticlePrefab;

    [Range(0f, 1f)]
    public float firstStageDamage = 0.15f;

    [Range(0f, 1f)]
    public float secondStageDamage = 0.1f;

    [Range(0f, 20f)]
    public float firstStageForce = 3f;

    [Range(0f, 20f)]
    public float secondStageForce = 13f;


    private float _scale = 8.0f;
    private bool _flash = false;
    private CircleCollider2D _cc;
    private GameObject _projectileParticle;
    private GameObject _muzzleParticle;
    private bool _trigger_explosion = true;
    private bool _is_explosion = false;
    // Start is called before the first frame update
    void Start()
    {
        _projectileParticle = Instantiate(projectileParticlePrefab, transform.position, transform.rotation) as GameObject;
        _projectileParticle.transform.parent = transform;
        _projectileParticle.transform.localScale = new Vector3(_scale, _scale, _scale);
        _cc = GetComponent<CircleCollider2D>();
        _cc.enabled = false;
        ChangeSortingLayer(impactParticlePrefab);
        ChangeSortingLayer(projectileParticlePrefab);
        ChangeSortingLayer(muzzleParticlePrefab);

    }

    // Update is called once per frame
    void Update()
    {
        if (_scale > 1f)
        {
            _scale -= Time.deltaTime * 20f;
            _projectileParticle.transform.localScale = new Vector3(_scale, _scale, _scale);
        }
        else 
        {
            if (!_flash)
            {
                _flash = true;
                _cc.enabled = true;
                _cc.radius = 1.2f;
                _muzzleParticle = Instantiate(muzzleParticlePrefab, transform.position, transform.rotation) as GameObject;
                Destroy(_muzzleParticle, 1.5f);
                CameraShakeEffect.ShakeCamera(0.3f, 0.5f);
                StartCoroutine(Explosion());
            }
        }
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(_projectileParticle);
        _is_explosion = true;
        _cc.radius = 3.8f;
        _trigger_explosion = true;
        GameObject impactParticle = Instantiate(impactParticlePrefab, transform.position, Quaternion.FromToRotation(new Vector3(0, 1f, 0), new Vector3(0, 0, 1f))) as GameObject;
        impactParticle.transform.localScale = new Vector3(1f, 1f, 1f);

        yield return new WaitForSeconds(1.5f);
        _cc.enabled = false;
        Destroy(impactParticle); // Lifetime of muzzle effect.
        _is_explosion = false;
        Destroy(this.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (_trigger_explosion && collider.tag.Contains("Submarine"))
        {
            Rigidbody2D rd2D = collider.GetComponent<Rigidbody2D>();
            HealthCounter hc = collider.GetComponent<HealthCounter>();
            SubmarineController sc = collider.GetComponent<SubmarineController>();

            Vector3 direction = (collider.transform.position - transform.position).normalized;

            float force_magnitude = _is_explosion ? secondStageForce : firstStageForce;
            float damage = _is_explosion ? hc.maxHealth * secondStageDamage : hc.maxHealth * firstStageDamage;
            float vibration_mag = _is_explosion ? 0.6f : 1.0f;

            InputSystemManager.SetVibration(sc.playerID1, vibration_mag, 0.2f);
            InputSystemManager.SetVibration(sc.playerID2, vibration_mag, 0.2f);

            rd2D.AddForce(rd2D.mass * direction * force_magnitude, ForceMode2D.Impulse);
            hc.AlterHealth(-1 * damage);
            GameObject hitSpark = Instantiate(hitParticlePrefab, collider.transform);
            hitSpark.transform.position -= direction * 2.27f;
            _trigger_explosion = false;
            Destroy(hitSpark, 1.5f);
        }
    }

    private void ChangeSortingLayer(GameObject effectobject)
    {

        foreach(Transform child in effectobject.transform)
        {
            ParticleSystemRenderer psr = child.gameObject.GetComponent<ParticleSystemRenderer>();
            if (psr != null)
            {
                psr.sortingOrder = 10;
            }
        }

        ParticleSystemRenderer parent_psr = effectobject.GetComponent<ParticleSystemRenderer>();
        if (parent_psr != null)
        {
            parent_psr.sortingOrder = 10;
        }
    }
}
