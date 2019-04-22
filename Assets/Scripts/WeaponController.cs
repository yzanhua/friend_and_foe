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
    public GameObject weaponWarning;
    public GameObject sparkPrefab;
    private GameObject submarine;
    private int remainBullets;
    private bool isAbleToFire = true;
    private RefillController rc;

    void Start()
    {
        submarine = transform.parent.parent.gameObject;

        remainBullets = MaxBullets * 2 / 3;
        rc = refillStation.GetComponent<RefillController>();
        rc.SetBulletStatus(remainBullets == MaxBullets);
        weaponWarning.SetActive(false);
    }

    public void Fire(int playerID)
    {
        if (!isAbleToFire)
            return;
        isAbleToFire = false;
        StartCoroutine(CD());
        if (remainBullets > 0)
        {
            Vector3 offset = (transform.position - submarine.transform.position).normalized * bulletoffset;
            GameObject spark = Instantiate(sparkPrefab, transform.position + offset, transform.rotation);
            Destroy(spark, 1f);
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

            if (TutorialManager.instance != null)
            {
                bool isRight = transform.parent.parent.GetComponent<SubmarineController>().ID == 1;
                TutorialManager.CompleteTask(TutorialManager.TaskType.SHOOT, isRight);
            }

            rc = refillStation.GetComponent<RefillController>();
            rc.SetBulletStatus(false);
            InputSystemManager.SetVibration(playerID, 0.2f, 0.3f);
            weaponWarning.SetActive(true);
        }
    }

    private IEnumerator CD()
    {
        yield return new WaitForSeconds(fireCD);
        isAbleToFire = true;
    }


    public void FillBullets()
    {
        remainBullets = MaxBullets;
        weaponWarning.SetActive(false);
    }

    public float Health()
    {
        return (float)remainBullets / MaxBullets;
    }

}
