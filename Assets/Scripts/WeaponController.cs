using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject refillStation;
    public int MaxBullets = 15;
    public float bulletoffset;
    public float fireCD = 1f;

    GameObject submarine;
    int remainBullets;
    bool isAbleToFire = true;
    RefillController rc;

    void Start()
    {
        submarine = transform.parent.gameObject;
        remainBullets = 0;
        rc = refillStation.GetComponent<RefillController>();
        rc.SetBulletStatus(remainBullets == MaxBullets);
    }

    public void Fire()
    {
        if (!isAbleToFire)
            return;
        isAbleToFire = false;
        StartCoroutine(CD());
        if (remainBullets > 0)
        {
            Vector3 offset = (transform.position - submarine.transform.position).normalized * bulletoffset;
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + offset, transform.rotation);
            bullet.GetComponent<BulletController>().direction = -submarine.transform.position + transform.position;
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("shoot");
            remainBullets--;
            rc = refillStation.GetComponent<RefillController>();
            rc.SetBulletStatus(false);
        }
        // bullets empty
        else
        {
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("warning");
            rc = refillStation.GetComponent<RefillController>();
            rc.SetBulletStatus(false);
        }

    }

    IEnumerator CD()
    {
        yield return new WaitForSeconds(fireCD);
        isAbleToFire = true;
    }
    public void FillBullets()
    {
        remainBullets = MaxBullets;
    }

    public float Health()
    {
        return (float)remainBullets / MaxBullets;
    }

}
