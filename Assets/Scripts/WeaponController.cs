using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int MaxBullets = 15;

    GameObject submarine;
    int remainBullets;

    void Start()
    {
        submarine = transform.parent.gameObject;
        remainBullets = MaxBullets;
    }

    public void Fire()
    {
        if (remainBullets > 0)
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
            //bullet.transform.parent = gameObject.transform;
            bullet.GetComponent<BulletController>().direction = -submarine.transform.position + transform.position;
            SoundManager.instance.PlaySound("shoot");
            remainBullets--;
        }
        else
        {
            SoundManager.instance.PlaySound("warning");
        }

    }

    public void fillBullets()
    {
        remainBullets = MaxBullets;
    }
}
