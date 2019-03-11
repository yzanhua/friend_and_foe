using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Bullet"))
        {
            GetComponent<HealthCounter>().AlterHealth(-5);
        }
    }

    public void TerrainChange()
    {
        print("Change called");
        transform.Find("WeaponGear").localPosition = new Vector3(1.753f, -0.357f);
        transform.Find("MovementGear").localPosition = new Vector3(1.283f, 1.173f);
        transform.Find("ShieldGear").localPosition = new Vector3(-0.49f, -1.79f);
    }
}
