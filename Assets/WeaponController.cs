using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position - new Vector3(2, 0 ,0), transform.rotation);
            bullet.transform.parent = gameObject.transform;
            //bullet.GetComponent<BulletController>().Shoot();
        }
    }

    //public static BulletController Create()
    //{
    //    GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position - new Vector3(2, 0, 0), Quaternion.identity);
    //    BulletController yourObject = bullet.GetComponent<BulletController>();
    //    //do additional initialization steps here

    //    return yourObject;
    //}
}
