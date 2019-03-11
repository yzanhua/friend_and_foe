using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject submarine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.transform.parent = gameObject.transform;
            bullet.GetComponent<BulletController>().direction = - submarine.transform.position + transform.position;
        }
    }

}
