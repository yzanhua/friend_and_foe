using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject refillStation;
    public int MaxBullets = 15;
    public float bulletoffset;

    GameObject submarine;
    int remainBullets;
    RefillController rc;

    void Start()
    {
        submarine = transform.parent.gameObject;
        remainBullets = 0;
        rc = refillStation.GetComponent<RefillController>();
        rc.SetBulletStatus(false);
    }

    public void Fire()
    {
        if (remainBullets > 0)
        {
            Vector3 offset = (transform.position - submarine.transform.position).normalized * bulletoffset;
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + offset, transform.rotation);
            bullet.GetComponent<BulletController>().direction = -submarine.transform.position + transform.position;
            SoundManager.instance.PlaySound("shoot");
            remainBullets--;
            Debug.Log(remainBullets);
        }
        else
        {
            SoundManager.instance.PlaySound("warning");
            // Debug.Log("Refill the station");
            rc = refillStation.GetComponent<RefillController>();
            rc.SetBulletStatus(false);
        }

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
