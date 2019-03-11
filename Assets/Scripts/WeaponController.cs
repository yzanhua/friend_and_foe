using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;

    GameObject submarine;

    void Start()
    {
        submarine = transform.parent.gameObject;
    }

    public void Fire()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
        //bullet.transform.parent = gameObject.transform;
        bullet.GetComponent<BulletController>().direction = -submarine.transform.position + transform.position;
    }
}
